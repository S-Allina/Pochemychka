using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pochemychka.Models
{
    public partial class Test
    {
        
        [Key]
        public int IdTest { get; set; }

[RegularExpression(@"^[а-яА-Я0-9,\.-]{6,30}$", ErrorMessage = "Некорректные данные. Вариант должен содержать от 6 до 30 русских символов, цифр, запятых, тире и точек")]
        [StringLength(30, MinimumLength = 6)]
        public string NameTest { get; set; } = null!;


        //public virtual ICollection<ResultDiapason>? ResultDiapasons { get; set; }
        //public virtual ICollection<Question>? Questions { get; set; }
    }
}
