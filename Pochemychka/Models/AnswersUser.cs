using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pochemychka.Models
{
    public partial class AnswersUser
    {
       
        [Key]
        public int IdAnswersUser { get; set; }
        public string IdUser { get; set; }


        //[StringLength(1, MinimumLength = 1)]
        public int CountFullPoints { get; set; }
        public DateTime Time { get; set; }

        //public virtual ICollection<Answer>? Answers { get; set; }
    }
}
