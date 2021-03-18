using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain_models
{
    public class SubTopic
    {
        // Database for undertemaer
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        // Relasjoner
        [ForeignKey("Topic")]
        public int TopicId { get; set; }
        public Topic Topic { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
