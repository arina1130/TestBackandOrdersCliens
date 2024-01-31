using System;
using System.Collections.Generic;

namespace TestBackand.Repositories;

public partial class Status
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
