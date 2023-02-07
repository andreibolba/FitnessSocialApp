using System;
using System.Collections.Generic;

namespace API.Models;

public partial class TableManagement
{
    public int TableManagementId { get; set; }

    public int TableTypeId { get; set; }

    public int PersonId { get; set; }

    public DateTime Quantity { get; set; }

    public bool Deleted { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual TableType TableType { get; set; } = null!;
}
