using System;
using System.Collections.Generic;

namespace API.Models;

public partial class RecipeProduct
{
    public int RecipeProductId { get; set; }

    public int RecipeId { get; set; }

    public int ProductId { get; set; }

    public bool Deleted { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
