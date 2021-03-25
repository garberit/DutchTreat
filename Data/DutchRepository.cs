using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
	public class DutchRepository : IDutchRepository
	{
		private readonly DutchContext ctx;
		private readonly ILogger<DutchRepository> logger;

		public DutchRepository(DutchContext ctx, ILogger<DutchRepository> logger)
		{
			this.ctx = ctx;
			this.logger = logger;
		}

		public void AddEntity(object model)
		{
			ctx.Add(model);
		}

		public IEnumerable<Order> GetAllOrders(bool includeItems)
		{
			if (includeItems)
			{
				return ctx.Orders
				.Include(o => o.Items)
				.ThenInclude(i => i.Product)
				.ToList();
			} else
			{
				return ctx.Orders.ToList();
			}
			
		}

		

		public IEnumerable<Product> GetAllProducts()
		{
			try
			{
				logger.LogInformation("GetAllProducts was called");
				return ctx.Products
					.OrderBy(p => p.Title)
					.ToList();
			}
			catch (Exception ex)
			{
				logger.LogError($"Failed to get all products: {ex}");
				return null;
			}
		}

		public Order GetOrderById(int id)
		{
			return ctx.Orders
				.Include(o => o.Items)
				.ThenInclude(i => i.Product)
				.Where(o => o.Id == id)
				.FirstOrDefault();
		}

		public IEnumerable<Product> GetProductsByCategory(string category)
		{
			return ctx.Products
				.Where(p => p.Category == category)
				.ToList();
		}

		public bool SaveChanges()
		{
			return ctx.SaveChanges() > 0;
		}
	}
}
