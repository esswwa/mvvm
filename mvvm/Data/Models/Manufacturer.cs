using System;
using System.Collections.Generic;

namespace mvvm.Data.Models;

public partial class Manufacturer
{
    public int IdManufacturer { get; set; }

    public string? ProductManufacture { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
