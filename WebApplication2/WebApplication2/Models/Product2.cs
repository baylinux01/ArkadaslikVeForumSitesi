using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Product2
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public  string Origin { get; set; }
        public int Number { get; set; }
        public User User { get; set; }
        public Product2()
        {
            
        }
        //public Product2(int id, string name, int price)
        //{
        //    Id = id;
        //    Name = name;
        //    Price = price;
        //}
    }
}
