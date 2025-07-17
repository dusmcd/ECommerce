using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
    }
}
