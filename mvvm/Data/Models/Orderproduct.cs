using System;
using System.Collections.Generic;

namespace mvvm.Data.Models;

public partial class Orderproduct
{
    public int OrderId { get; set; }

    public string ProductArticleNumber { get; set; } = null!;

    public int? ProductCount { get; set; }

    public virtual Orderuser Order { get; set; } = null!;
}
