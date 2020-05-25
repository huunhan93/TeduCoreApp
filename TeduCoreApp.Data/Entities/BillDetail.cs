using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TeduCoreApp.Infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("BillDetails")]
    public class BillDetail : DomainEntity<int>
    {
        public BillDetail() { }

        public int BillId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public Decimal Price { get; set; }

        public int ColorId { get; set; }

        public int SizeId { get; set; }

        [ForeignKey("BillId")]
        public virtual Bill Bill { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("ColorId")]
        public virtual Color Color { get; set; }

        [ForeignKey("SizeId")]
        public virtual Size Size { get; set; }
    }
}
