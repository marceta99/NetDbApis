using System.ComponentModel.DataAnnotations.Schema;

namespace PostgresSqlApi.Model
{
    public class Order
    {
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
