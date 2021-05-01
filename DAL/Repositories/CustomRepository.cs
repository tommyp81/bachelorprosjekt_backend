using Azure.Storage.Blobs;
using DAL.Database_configuration;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CustomRepository : ICustomRepository
    {
        private readonly DBContext _context;
        private readonly IConfiguration _config;

        public CustomRepository(DBContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        // POST: AddDocument
        public async Task<Document> AddDocument(IFormFile file, int? userId, int? postId, int? commentId, int? infoTopicId)
        {
            // Tidssone
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);

            // Informasjon om filnavn og type
            var fileName = Path.GetFileName(file.FileName);
            var fileType = Path.GetExtension(fileName);

            // Vise filstørrelse som MB, kB eller byte
            string fileSize = null;
            if (file.Length >= 1048576)
            {
                fileSize = Math.Round((file.Length / 1024f) / 1024f).ToString() + " MB";
            }
            else if (file.Length >= 1024)
            {
                fileSize = Math.Round((file.Length / 1024f)).ToString() + " kB";
            }
            else if (file.Length <= 1024)
            {
                fileSize = file.Length.ToString() + " byte";
            }

            // Unikt navn for Azure Storage Blob. Eks: sysadmin/testfil.txt (xmwqo80924ng9430n)
            var user = await _context.Users.FindAsync(userId);
            string uniqueName = user.Username + "/" + fileName + " (" + Guid.NewGuid().ToString() + ")";

            // Navn på container. Eks: mai-2021
            string container = "default";
            int caseSwitch = now.Month;
            switch (caseSwitch)
            {
                case 1:
                    container = "januar-" + now.Year;
                    break;
                case 2:
                    container = "februar-" + now.Year;
                    break;
                case 3:
                    container = "mars-" + now.Year;
                    break;
                case 4:
                    container = "april-" + now.Year;
                    break;
                case 5:
                    container = "mai-" + now.Year;
                    break;
                case 6:
                    container = "juni-" + now.Year;
                    break;
                case 7:
                    container = "juli-" + now.Year;
                    break;
                case 8:
                    container = "august-" + now.Year;
                    break;
                case 9:
                    container = "september-" + now.Year;
                    break;
                case 10:
                    container = "oktober-" + now.Year;
                    break;
                case 11:
                    container = "november-" + now.Year;
                    break;
                case 12:
                    container = "desember-" + now.Year;
                    break;
            }

            // Azure Storage connection
            var containerClient = new BlobContainerClient(_config.GetConnectionString("AzureStorageKey"), container);

            // Lag ny container om den ikke eksisterer
            if (!await containerClient.ExistsAsync())
            {
                await containerClient.CreateAsync();
            }

            // Referanse til Blob (selve filen)
            BlobClient blobClient = containerClient.GetBlobClient(uniqueName);

            // Last opp filen
            using (var uploadFileStream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(uploadFileStream, true);
                uploadFileStream.Close();
            }

            // Legge til informasjon om filen
            var document = new Document
            {
                FileName = fileName,
                FileType = fileType,
                FileSize = fileSize,
                Uploaded = now,
                UniqueName = uniqueName,
                Container = container,
                UserId = userId,
                PostId = postId,
                CommentId = commentId,
                InfoTopicId = infoTopicId
            };

            // Legg til den nye filen i databasen
            var result = await _context.Documents.AddAsync(document);

            // Lagre alle endringer til databasen
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        // POST: UploadDocument
        public async Task<Document> UploadDocument(IFormFile file, int? userId, int? postId, int? commentId, int? infoTopicId)
        {
            // Legg til det nye dokumentet
            var document = await AddDocument(file, userId, postId, commentId, infoTopicId);

            // Oppdater eventuell post med referanse til dette dokumentet
            if (postId != null)
            {
                // Slett dokumentet om posten har dokument fra før
                var post = await _context.Posts.FindAsync(postId);
                if (post.DocumentId != null)
                {
                    await DeleteDocument((int)post.DocumentId);
                }

                // Oppdater posten med nytt dokument
                post.DocumentId = document.Id;
            }

            // Oppdater eventuell kommentar med referanse til dette dokumentet
            if (commentId != null)
            {
                // Slett dokumentet om kommentaren har dokument fra før
                var comment = await _context.Comments.FindAsync(commentId);
                if (comment.DocumentId != null)
                {
                    await DeleteDocument((int)comment.DocumentId);
                }

                // Oppdater denne kommentaren med DocumentId
                comment.DocumentId = document.Id;
            }

            // Lagre endringer til databasen
            await _context.SaveChangesAsync();
            return document;
        }

        // GET: GetDocuments
        public async Task<IEnumerable<Document>> GetDocuments()
        {
            var documents = await _context.Documents.ToListAsync();
            if (documents != null)
            {
                return documents;
            }
            else
            {
                return null;
            }
        }

        // GET: GetDocumentInfo/1
        public async Task<Document> GetDocumentInfo(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                return document;
            }
            else
            {
                return null;
            }
        }

        // GET: GetDocument/1
        public async Task<FileStreamResult> GetDocument(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                // Azure Storage connection, hent unikt navn fra databasen med ID
                var blobClient = new BlobContainerClient(_config.GetConnectionString("AzureStorageKey"), document.Container).GetBlobClient(document.UniqueName);
                if (await blobClient.ExistsAsync())
                {
                    // Finn filen i Azure Storage som skal lastes ned
                    var file = await blobClient.DownloadAsync();
                    return new FileStreamResult(file.Value.Content, file.Value.ContentType)
                    {
                        // Returner filen med filnavn fra databasen (så bruker ikke laster ned fil med unikt navn)
                        FileDownloadName = document.FileName
                    };
                }
            }

            return null;
        }

        // DELETE: DeleteDocument/1
        public async Task<Document> DeleteDocument(int id)
        {
            var result = await _context.Documents.FindAsync(id);
            if (result != null)
            {
                // Azure Storage connection
                var containerClient = new BlobContainerClient(_config.GetConnectionString("AzureStorageKey"), result.Container);
                var blobClient = containerClient.GetBlobClient(result.UniqueName);

                // Slette filen fra Azure Storage
                if (await blobClient.ExistsAsync())
                {
                    await blobClient.DeleteAsync();
                }

                // Slette container fra Azure Storage om den er tom
                if (!containerClient.GetBlobs().Any())
                {
                    // Slett med en gang siden denne er tom!
                    await containerClient.DeleteAsync();
                }

                // Oppdater eventuell post så den ikke har en referanse til dette dokumentet
                if (result.PostId != null)
                {
                    var update = await _context.Posts.FindAsync(result.PostId);
                    if (update != null)
                    {
                        update.DocumentId = null;
                    }
                }

                // Oppdater eventuell kommentar så den ikke har en referanse til dette dokumentet
                if (result.CommentId != null)
                {
                    var update = await _context.Comments.FindAsync(result.CommentId);
                    if (update != null)
                    {
                        update.DocumentId = null;
                    }
                }

                // Slett fil fra databasen og lagre endringer
                _context.Documents.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            else
            {
                return null;
            }
        }

        // POST: Login
        public async Task<User> Login(string username, string email, string password)
        {
            // Liste over alle eksisterende brukere
            var users = await _context.Users.ToListAsync();
            if (users != null)
            {
                foreach (var user in users)
                {
                    // Sjekk om brukernavn eller epost stemmer
                    if (user.Username == username || user.Email == email)
                    {
                        // Sjekk passord med kryptering
                        const int keyLength = 24;
                        var pbkdf2 = new Rfc2898DeriveBytes(password, user.Salt, 1000);
                        byte[] passwordTest = pbkdf2.GetBytes(keyLength);
                        bool authUser = user.Password.SequenceEqual(passwordTest);
                        if (authUser == true)
                        {
                            // Sende tilbake brukerobjekt hvis ok
                            return user;
                        }
                    }
                }
            }

            // Sende tilbake null hvis bruker eller passord er feil
            return null;
        }

        // POST: SetAdmin
        public async Task<User> SetAdmin(int id, bool admin)
        {
            var result = await _context.Users.FindAsync(id);
            if (result != null)
            {
                result.Admin = admin;
                await _context.SaveChangesAsync();
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
