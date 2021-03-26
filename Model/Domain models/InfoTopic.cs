using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain_models
{
    public class InfoTopic
    {
        // Database for emner til kunnskapsportalen
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        // Relasjoner
        public ICollection<Document> Documents { get; set; }
        public ICollection<Video> Videos { get; set; }
    }
}
