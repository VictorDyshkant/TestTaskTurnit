using System;

namespace Turnit.GenericStore.Api.Models;

public class AvailabilityModel
{
    public Guid StoreId { get; set; }

    public int Availability { get; set; }
}