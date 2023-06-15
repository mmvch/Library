namespace Library.WebModels
{
    public class WebBook
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime CreationDate { get; set; }

        public IFormFile? BookText { get; set; }

        public IFormFile? Image { get; set; }
    }
}
