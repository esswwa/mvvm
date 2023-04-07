using System;
using System.Collections.Generic;

namespace mvvm.Data.Models;

public partial class Orderuser
{
    public int OrderId { get; set; }

    public string OrderStatus { get; set; } = null!;

    public DateOnly OrderDeliveryDate { get; set; }

    public int OrderPickupPoint { get; set; }

    public DateOnly? OrderBeginDate { get; set; }

    public string? OrderFio { get; set; }

    public int OrderCode { get; set; }

    public virtual Pickuppoint OrderPickupPointNavigation { get; set; } = null!;

    public virtual ICollection<Orderproduct> Orderproducts { get; } = new List<Orderproduct>();
}
