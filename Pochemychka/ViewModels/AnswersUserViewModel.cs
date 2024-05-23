using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pochemychka.ViewModels
{
    public partial class AnswersUserViewModel
    {
        [Key]
        public int IdAnswersUser { get; set; }
        public string IdUser { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string NameTest { get; set; }
        public int IdTest { get; set; }

        //[StringLength(1, MinimumLength = 1)]
        public int CountFullPoints { get; set; }
        public string? TextResult { get; set; }
        public DateTime Time { get; set; }
    }
}
