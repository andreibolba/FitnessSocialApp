using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public int PersonId { get; set; }

    public string RecipeName { get; set; } = null!;

    public string? RecipeDescription { get; set; }

    public bool Deleted { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<RecipeProduct> RecipeProducts { get; } = new List<RecipeProduct>();
}
