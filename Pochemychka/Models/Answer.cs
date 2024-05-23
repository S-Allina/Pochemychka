
using System.ComponentModel.DataAnnotations;

namespace Pochemychka.Models
{
 
    public partial class Answer
    {
        [Key]
        public int IdAnswer { get; set; }
        public int IdAnswersUser { get; set; }
        public int IdQuestion { get; set; }
        public string Answer1 { get; set; } = null!;
        public int CountPoints { get; set; }

    }
}
