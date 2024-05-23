using Pochemychka.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pochemychka.ViewModels
{
    public partial class TestViewModel
    {
        
        public int IdTest { get; set; }

        public string NameTest { get; set; } = null!;
        public virtual List<ResultDiapason>? ResultDiapasons { get; set; }

        //public virtual ICollection<AnswersUser>? AnswersUsers { get; set; }
        //public virtual ICollection<Question>? Questions { get; set; }
    }
}
