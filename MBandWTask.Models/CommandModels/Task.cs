using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MBAndWTask.Model
{
    public class Task
    {
       public int id { get; set; }
        [Required]
        [DisplayName("Name")]
        public string name { get; set; }
        [Required]
        [DisplayName("Description")]
        public string description { get; set; }
        [Required]
        [DisplayName("Due Date")]
        public DateTime dueDate { get; set; }
        [Required]
        [DisplayName("Start Date")]
        public DateTime startDate { get; set;}
        [Required]
        [DisplayName("End Date")]
        public DateTime endDate { get; set;}
        [Required]
        [DisplayName("Priority")]
        public Priority priority { get; set; }

        [Required]
        [DisplayName("Status")]
        public Status status { get; set; }
        
    }

    public enum Priority { High = 1, Medium, Low }   

    public enum Status { New = 1, InProgress, Finished }
}
