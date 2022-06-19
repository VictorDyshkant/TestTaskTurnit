using System;

namespace Turnit.GenericStore.Api.Models;

public class RestockModel
{
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
}