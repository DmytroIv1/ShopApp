using System;
using System.Collections.Generic;

namespace ShopApp.Models;

public partial class Sale
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateOnly Date { get; set; }

    public virtual Product Product { get; set; } = null!;
}
