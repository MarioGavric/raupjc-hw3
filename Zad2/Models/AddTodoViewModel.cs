using System;
using System.ComponentModel.DataAnnotations;

namespace Zad2.Models
{
    public class AddTodoViewModel
    {
        [Required]
        public string Text { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateDue { get; set; }
        public string Labels { get; set; }
    }
}