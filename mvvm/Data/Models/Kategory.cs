using System;
using System.Collections.Generic;

namespace mvvm.Data.Models;

public partial class Kategory
{
    public int Idkategory { get; set; }

    public string? ProductKategory { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
