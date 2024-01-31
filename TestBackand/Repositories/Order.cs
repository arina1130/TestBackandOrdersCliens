using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TestBackand.Repositories;

public partial class Order
{
    public int Id { get; set; }

    public DateTime DateTime { get; set; }

    public int IdStatus { get; set; }

    public int IdClient { get; set; }

    public float Sum { get; set; }
    [JsonIgnore]

    public virtual Client? IdClientNavigation { get; set; }
    [JsonIgnore]
    public virtual Status? IdStatusNavigation { get; set; }
}
