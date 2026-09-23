using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.DTOs
{
    public class TaskUpdateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }
}