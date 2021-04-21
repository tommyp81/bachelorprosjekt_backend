using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain_models
{
    public class Topic
    {
        // Database for temaer
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        // Relasjoner
        public ICollection<SubTopic> SubTopics { get; set; }
    }
}
