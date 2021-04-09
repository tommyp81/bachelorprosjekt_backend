using Azure.Storage.Blobs;
using DAL.Database_configuration;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            // Unikt navn for Azure Storage Blob
            string uniqueName = fileName + " (" + Guid.NewGuid().ToString() + ")";

            // Navn på container (her brukes username nå)
            var user = await _context.Users.FindAsync(userId);
            string container = user.Username;

            // Azure Storage connection
            BlobContainerClient containerClient = new BlobContainerClient(_config.GetConnectionString("AzureStorageKey"), container);

            // Lag ny container om den ikke eksisterer
            //await containerClient.CreateIfNotExistsAsync();
            // Lag ny container om den ikke eksisterer -> Dette burde gi mindre errors i Azure Portal
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

            var timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

            // Legge til informasjon om filen
            Document document = new Document
            {
                FileName = fileName,
                FileType = fileType,
                FileSize = fileSize,
                Uploaded = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone),
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
        public async Task<ICollection<Document>> GetDocuments()
        {
            return await _context.Documents.ToListAsync();
        }

        // GET: GetDocumentInfo/1
        public async Task<Document> GetDocumentInfo(int id)
        {
            return await _context.Documents.FindAsync(id);
        }

        // DELETE: DeleteDocument/1
        public async Task<Document> DeleteDocument(int id)
        {
            var result = await _context.Documents.FindAsync(id);
            if (result != null)
            {
                // Azure Storage connection
                BlobContainerClient containerClient = new BlobContainerClient(_config.GetConnectionString("AzureStorageKey"), result.Container);
                BlobClient blobClient = containerClient.GetBlobClient(result.UniqueName);

                // Slette filen fra Azure Storage
                //await blobClient.DeleteIfExistsAsync();
                // Sjekk om den finnes først, og slett hvis. Burde gi mindre feilmeldinger i Azure Portal
                if (await blobClient.ExistsAsync())
                {
                    await blobClient.DeleteAsync();
                }

                // Slette container fra Azure Storage om den er tom
                if (!containerClient.GetBlobs().Any())
                {
                    //await containerClient.DeleteIfExistsAsync();
                    // Slett med en gang siden denne er tom! Burde gi mindre feilmeldinger i Azure Portal
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
            return null;
        }
    }
}
