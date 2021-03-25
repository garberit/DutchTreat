using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
		private readonly DutchContext ctx;
		private readonly IWebHostEnvironment env;

		public DutchSeeder(DutchContext ctx, IWebHostEnvironment env)
		{
			this.ctx = ctx;
			this.env = env;
		}

		public void Seed()
		{
			ctx.Database.EnsureCreated();
			if (!ctx.Products.Any())
			{
				//create sample data
				var filePath = Path.Combine(env.ContentRootPath, "Data/art.json");
				var json = File.ReadAllText(filePath);
				var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

				ctx.Products.AddRange(products);

				var order = new Order()
				{
					OrderDate = DateTime.Today,
					OrderNumber = "10000",
					Items = new List<OrderItem>()
					{
						new OrderItem()
						{
							Product = products.First(),
							Quantity = 5,
							UnitPrice = products.First().Price
						}
					}
				};

				ctx.Orders.Add(order);

				ctx.SaveChanges();
			}
		}
    }
}
