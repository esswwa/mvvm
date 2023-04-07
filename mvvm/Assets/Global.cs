using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm.Assets
{
    public static class Global
    {
        public static List<Card>? CurrentCart { get; set; } = new List<Card>();
    }
}
