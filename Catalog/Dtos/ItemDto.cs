﻿using System;

namespace Catalog.Dtos
{
	public record ItemDto
	{
		public Guid Id { get; init; }

		public string Name { get; set; }

		public decimal Price { get; set; }

		public DateTimeOffset CreatedDate { get; init; }
	}
}
