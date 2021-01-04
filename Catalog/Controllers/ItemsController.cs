﻿using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Controllers
{
	// GET /items

	[ApiController]
	[Route("items")]
	public class ItemsController : ControllerBase
	{
		private readonly IItemsRepository repository;

		public ItemsController(IItemsRepository repository)
		{
			this.repository = repository;
		}

		// GET /items
		[HttpGet]
		public async Task<IEnumerable<ItemDto>> GetItemsAsync()
		{
			var items = (await repository.GetItemsAsync())
							.Select(item => item.AsDto());

			return items;
		}

		// GET /items/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
		{
			var item = await repository.GetItemAsync(id);

			if (item is null)
			{
				return NotFound();
			}

			return item.AsDto();
		}

		// POST /items
		[HttpPost]
		public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
		{
			Item item = new()
			{
				Id = Guid.NewGuid(),
				Name = itemDto.Name,
				Price = itemDto.Price,
				CreatedDate = DateTimeOffset.UtcNow
			};

			await repository.CreateItemAsync(item);

			return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
		}

		// PUT /items/{id}
		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateItem(Guid id, UpdateItemDto itemDto)
		{
			var existingItem = await repository.GetItemAsync(id);

			if (existingItem is null)
			{
				return NotFound();
			}

			Item updatedItem = existingItem with
			{
				Name = itemDto.Name,
				Price = itemDto.Price
			};

			await repository.UpdateItemAsync(updatedItem);

			return NoContent();
		}

		// DELETE /items/{id}
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteItem(Guid id)
		{
			var existingItem = await repository.GetItemAsync(id);

			if (existingItem is null)
			{
				return NotFound();
			}

			await repository.DeleteItemAsync(id);

			return NoContent();
		}
	}
}
