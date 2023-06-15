namespace Library.WebModels
{
    public class WebText
    {
        public Guid Id { get; set; }

        public IEnumerable<IEnumerable<string>>? Pages { get; set; }
    }
}
