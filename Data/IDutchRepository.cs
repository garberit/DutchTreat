using DutchTreat.Data.Entities;
using System.Collections.Generic;

namespace DutchTreat.Data
{
	public interface IDutchRepository
	{
		IEnumerable<Product> GetAllProducts();
		IEnumerable<Product> GetProductsByCategory(string category);

		IEnumerable<Order> GetAllOrders(bool includeItems);
		Order GetOrderById(string username, int id);
		bool SaveChanges();
		void AddEntity(object model);
		IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems);
	}
}