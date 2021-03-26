﻿using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
	[Route("api/[Controller]")]
	[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : Controller
    {
		private readonly IDutchRepository repository;
		private readonly ILogger<OrdersController> logger;
		private readonly IMapper mapper;

		public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger, IMapper mapper)
		{
			this.repository = repository;
			this.logger = logger;
			this.mapper = mapper;
		}

		[HttpGet]
		public IActionResult Get(bool includeItems = true)
		{
			try
			{
				var result = repository.GetAllOrders(includeItems);
				return Ok(mapper.Map<IEnumerable<OrderViewModel>>(result));
			}
			catch (Exception ex)
			{
				var error = $"Failed to get orders: {ex}";
				logger.LogError(error);
				return BadRequest(error);
			}
		}


		[HttpGet("{id:int}")]
		public IActionResult Get(int id)
		{
			try
			{
				var order = repository.GetOrderById(id);
				if (order != null) return Ok(mapper.Map<Order, OrderViewModel> (order));
				else return NotFound();
			}
			catch (Exception ex)
			{
				var error = $"Failed to get orders: {ex}";
				logger.LogError(error);
				return BadRequest(error);
			}
		}

		[HttpPost]
		public IActionResult Post([FromBody]OrderViewModel model)
		{
			var error = $"Failed to save new order ";

			//add to DB
			try
			{
				if (ModelState.IsValid)
				{
					var newOrder = mapper.Map<OrderViewModel, Order>(model);
					if (newOrder.OrderDate == DateTime.MinValue)
					{
						newOrder.OrderDate = DateTime.Now;
					}
					repository.AddEntity(newOrder);
					if (repository.SaveChanges())
					{
						
						return Created($"/api/orders/{newOrder.Id}", mapper.Map<Order, OrderViewModel>(newOrder));
					}
				}
				else
				{
					return BadRequest(ModelState);
				}
				
				
			}
			catch (Exception ex)
			{

				logger.LogError(error + ex);
			}
			return BadRequest(error);

		}
	}
}
