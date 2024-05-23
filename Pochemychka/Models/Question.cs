using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pochemychka.Models
{
    public partial class Question
    {
       
        [Key]
        public int IdQuestion { get; set; }
        public int IdTest { get; set; }


[RegularExpression(@"^[а-яА-Я0-9,\.-]{6,60}$", ErrorMessage = "Некорректные данные. Вопрос должен содержать от 6 до 60 русских символов, цифр, запятых, тире и точек")]
        [StringLength(60, MinimumLength = 6)]
        public string TextQuetion { get; set; } = null!;


[RegularExpression(@"^[а-яА-Я0-9,\.-]{6,30}$", ErrorMessage = "Некорректные данные. Вариант должен содержать от 6 до 30 русских символов, цифр, запятых, тире и точек")]
        [StringLength(30, MinimumLength = 6)]
        public string Option1 { get; set; }


[RegularExpression(@"^[а-яА-Я0-9,\.-]{6,30}$", ErrorMessage = "Некорректные данные. Вариант должен содержать от 6 до 30 русских символов, цифр, запятых, тире и точек")]
        [StringLength(30, MinimumLength = 6)]
        public string Option2 { get; set; }


[RegularExpression(@"^[а-яА-Я0-9,\.-]{6,30}$", ErrorMessage = "Некорректные данные. Вариант должен содержать от 6 до 30 русских символов, цифр, запятых, тире и точек")]
        [StringLength(30, MinimumLength = 6)]
        public string? Option3 { get; set; }


[RegularExpression(@"^[а-яА-Я0-9,\.-]{6,30}$", ErrorMessage = "Некорректные данные. Вариант должен содержать от 6 до 30 русских символов, цифр, запятых, тире и точек")]
        [StringLength(30, MinimumLength = 6)]
        public string? Option4 { get; set; }


[RegularExpression(@"^[а-яА-Я0-9,\.-]{6,30}$", ErrorMessage = "Некорректные данные. Вариант должен содержать от 6 до 30 русских символов, цифр, запятых, тире и точек")]
        [StringLength(30, MinimumLength = 6)]
        public string? Option5 { get; set; }
        public int CountPointOfOption1 { get; set; }
        public int CountPointOfOption2 { get; set; }
        public int? CountPointOfOption3 { get; set; }
        public int? CountPointOfOption4 { get; set; }
        public int? CountPointOfOption5 { get; set; }

        //public virtual ICollection<Answer> Answers { get; set; }
    }
}
