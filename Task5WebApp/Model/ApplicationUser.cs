using System.ComponentModel.DataAnnotations;

namespace Task5WebApp.Model
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        [Required, MinLength(1)]
        public string Name { get; set; }
    }
}
