using System;
using System.Collections.Generic;

namespace NorthwindApiDemo.EFModels
{
    public partial class OrderSubtotals
    {
        public int OrderId { get; set; }
        public decimal? Subtotal { get; set; }
    }
}
