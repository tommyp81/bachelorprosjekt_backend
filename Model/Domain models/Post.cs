using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain_models
{
    public class Post
    {
        // Database for poster
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public DateTime? EditDate { get; set; }
        public bool Edited { get; set; }
        public int Comment_Count { get; set; }
        public int Like_Count { get; set; }

        // Relasjoner
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("SubTopic")]
        public int SubTopicId { get; set; }
        public SubTopic SubTopic { get; set; }

        [ForeignKey("Document")]
        public int? DocumentId { get; set; }
        public Document Document { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
