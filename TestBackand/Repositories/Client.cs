using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TestBackand.Repositories;

public partial class Client
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public DateOnly Birthday { get; set; }
    [JsonIgnore]

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
