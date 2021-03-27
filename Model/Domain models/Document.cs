using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain_models
{
    public class Document
    {
        // Database for dokumenter
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public DateTime Uploaded { get; set; }

        // Informasjon for Azure Storage
        public string UniqueName { get; set; }
        public string Container { get; set; }

        // Relasjoner
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Post")]
        public int? PostId { get; set; }
        public Post Post { get; set; }

        [ForeignKey("Comment")]
        public int? CommentId { get; set; }
        public Comment Comment { get; set; }

        [ForeignKey("InfoTopic")]
        public int? InfoTopicId { get; set; }
        public InfoTopic InfoTopic { get; set; }
    }
}
