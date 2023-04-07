using System;
using System.Collections.Generic;

namespace mvvm.Data.Models;

public partial class Pickuppoint
{
    public int IdPickupPoint { get; set; }

    public int? Index { get; set; }

    public string? Country { get; set; }

    public string? Street { get; set; }

    public int? House { get; set; }

    public virtual ICollection<Orderuser> Orderusers { get; } = new List<Orderuser>();
}
