using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public  string Origin { get; set; }

        
        public Product()
        {
            
        }
        public Product(int id, string name, int price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}
