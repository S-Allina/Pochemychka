
using System.ComponentModel.DataAnnotations;

namespace Pochemychka.Models
{
 
    public class ResultDiapason
    {
        [Key]
        public int IdDiapasonResult { get; set; }
        public int IdTest { get; set; }
        public int StartDiapason { get; set; }
        public int EndDiapason { get; set; }
        public string TextResult { get; set; } = null!;


    }
}
