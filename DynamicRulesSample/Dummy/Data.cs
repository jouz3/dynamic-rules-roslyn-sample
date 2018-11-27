using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicRulesSample.Dummy
{
    public class Data
    {
        public static List<Customer> Customers(int Instances)
        {
            var customers = new List<Customer>();

            for (int i = 1; i <= Instances; i++)
            {
                customers.Add(new Customer
                {
                    ID = i,
                    Name = $"customer_{i}",
                    CreatedOn = new DateTime(2000, 1, 1).AddDays(i-1),
                    Products = Enumerable.Range(0, i)
                        .Select(r => new Product($"P{r}"))
                        .ToList()
                });
            }

            return customers;
        }
    }

    public class Model
    {
        public Model(long id, Customer customer)
        {
            ID = id;
            Customer = customer;
        }

        public long ID { get; set; }
        public Customer Customer { get; set; }
    }

    public class Customer
    {
        public Customer()
        {
            Products = new List<Product>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public List<Product> Products { get; set; }
    }

    public class Product
    {
        public Product(string code)
        {
            Code = code;
        }
        
        public string Code { get; set; }
    }
}
