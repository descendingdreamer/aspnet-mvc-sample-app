using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebForm.Models;

public class Widget
{
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? Created { get; set; }
        public string? Type { get; set; }
        public string? SubType { get; set; }
        
        [NotMapped]
        public IEnumerable<SelectListItem>? Types { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? SubTypes { get; set; }
}