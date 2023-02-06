using System;
using System.Collections.Generic;

namespace API.Models;

public partial class TableType
{
    public int TableTypeId { get; set; }

    public string TableTypeName { get; set; } = null!;

    public bool Deleted { get; set; }

    public virtual ICollection<TableManagement> TableManagements { get; } = new List<TableManagement>();
}
