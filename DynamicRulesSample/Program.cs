using System;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicRulesSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Get dummy data and rule to evaluate
            var customers = Dummy.Data.Customers(9);
            var ruleExpression = @"
Customer.Products.Exists(p => p.Code == ""P5"") 
&& Customer.CreatedOn > new DateTime(2000, 1, 6)";

            // Precompile rule for reuse
            var rule = new Evaluate(typeof(Dummy.Model), ruleExpression);
            Console.WriteLine($"########## Rule ##########\n{ruleExpression}\n\n");
            Console.WriteLine($"########## Results ##########\n");

            // Evaluate rule for each record
            foreach (var customer in customers)
            {
                var result = await rule.RunAsync(new Dummy.Model(customer.ID, customer));
                Console.ForegroundColor = (bool)result ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
                Console.WriteLine($"Customer ID: {customer.ID} CreatedOn: {customer.CreatedOn.ToString("yyyy-MM-dd")} Products: [{string.Join(", ", customer.Products.Select(p => p.Code))}]");
            }

            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
