using System;

namespace Turnit.GenericStore.Api.Models;

public class ProductModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public AvailabilityModel[] Availability { get; set; }
}
