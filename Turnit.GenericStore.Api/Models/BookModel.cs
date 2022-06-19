using System;

namespace Turnit.GenericStore.Api.Models;

public class BookModel
{
    public Guid StoreId { get; set; }

    public int Quantity { get; set; }
}