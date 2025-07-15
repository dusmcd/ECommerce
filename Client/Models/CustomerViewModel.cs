namespace Client.Models
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        public List<OrderViewModel> Orders { get; set; } = [];
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
    }
}
