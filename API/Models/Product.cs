using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int PersonId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal ProductPrice { get; set; }

    public int ProductKalories { get; set; }

    public bool Deleted { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<RecipeProduct> RecipeProducts { get; } = new List<RecipeProduct>();
}
