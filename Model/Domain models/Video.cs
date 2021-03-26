using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain_models
{
    public class Video
    {
        // Database for videoer
        [Key]
        public int Id { get; set; }
        public string YouTubeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        // Relasjoner
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Document")]
        public int? DocumentId { get; set; }
        public Document Document { get; set; }

        [ForeignKey("InfoTopic")]
        public int InfoTopicId { get; set; }
        public InfoTopic InfoTopic { get; set; }
    }
}
