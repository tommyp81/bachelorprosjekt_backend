using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain_models
{
    public class Comment
    {
        // Database for kommentarer
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public DateTime? EditDate { get; set; }
        public bool Edited { get; set; }
        public int Like_Count { get; set; }

        // Relasjoner
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [ForeignKey("Document")]
        public int? DocumentId { get; set; }
        public Document Document { get; set; }
    }
}
