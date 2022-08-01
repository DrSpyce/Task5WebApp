using System.ComponentModel.DataAnnotations;


namespace Task5WebApp.Model
{
    public class Message
    {
        public int Id { get; set; }

        [Required, MinLength(1)]
        public string Title { get; set; } = "";

        [Required, MinLength(1)]
        public string Body { get; set; } = "";

        [Required, MinLength(1)]
        public string Recipient { get; set; } = "";

        public string Sender { get; set; } = "";

        public string TimeOfSending { get; set; } = "";
    }
}
