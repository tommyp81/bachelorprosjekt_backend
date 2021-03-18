using Azure.Storage.Blobs;
using DAL.Database_configuration;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
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
        public async Task<Document> AddDocument(IFormFile file, int? userId, int? postId, int? commentId)
        {
            // Informasjon om fil
            var fileName = Path.GetFileName(file.FileName);
            var fileType = Path.GetExtension(fileName);
            var fileSize = Math.Round((file.Length / 1024f) / 1024f, 2).ToString() + " MB";

            // Unikt navn for Azure
            string uniqueName = fileName + "_" + Guid.NewGuid().ToString();

            // Navn på container
            string container = "test";

            // Azure Storage connection
            BlobContainerClient containerClient = new BlobContainerClient(_config.GetConnectionString("AzureStorageKey"), container);

            // Lag ny container om den ikke eksisterer
            await containerClient.CreateIfNotExistsAsync();

            // Referanse til Blob (selve filen)
            BlobClient blobClient = containerClient.GetBlobClient(uniqueName);

            // Last opp filen
            using (var uploadFileStream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(uploadFileStream, true);
                uploadFileStream.Close();
            }

            // Legge til informasjon om filen
            Document document = new Document
            {
                FileName = fileName,
                FileType = fileType,
                FileSize = fileSize,
                Uploaded = DateTime.UtcNow,
                UniqueName = uniqueName,
                Container = container,
                UserId = userId,
                PostId = postId,
                CommentId = commentId
            };

            // Legg til den nye filen i databasen
            var result = await _context.Documents.AddAsync(document);

            // Lagre alle endringer til databasen
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        // POST: UploadDocument
        public async Task<Document> UploadDocument(IFormFile file, int? userId, int? postId, int? commentId)
        {
            var result = await AddDocument(file, userId, postId, commentId);

            // Oppdater eventuell post med referanse til dette dokumentet
            if (postId != null)
            {
                var update = await _context.Posts.FindAsync(postId);
                if (update != null)
                {
                    update.DocumentId = result.Id;
                }
            }

            // Oppdater eventuell kommentar med referanse til dette dokumentet
            if (commentId != null)
            {
                // Oppdater denne kommentaren med DocumentId
                var update = await _context.Comments.FindAsync(commentId);
                if (update != null)
                {
                    update.DocumentId = result.Id;
                }
            }

            // Lagre endringer til databasen
            await _context.SaveChangesAsync();
            return result;
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
                await blobClient.DeleteIfExistsAsync();

                // Slette container fra Azure Storage om den er tom
                if (!containerClient.GetBlobs().Any())
                {
                    await containerClient.DeleteIfExistsAsync();
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

                // Oppdater eventuell post så den ikke har en referanse til dette dokumentet
                if (result.PostId != null)
                {
                    var update = await _context.Posts.FindAsync(result.PostId);
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
