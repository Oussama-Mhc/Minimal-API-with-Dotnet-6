using System.ComponentModel.DataAnnotations;

namespace MauiAPI.Models
{
    public class ToDo
    {
        [Key]
        public int Id { get; set; }
        public string? ToDoName { get; set; }
    }
}
