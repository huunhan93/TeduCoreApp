using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;
using TeduCoreApp.Infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Bills")]
    public class Bill : DomainEntity<int>, ISwitchable, IDateTracking
    {
        public Bill() { }
        
        [Required]
        [MaxLength(256)]
        public string CustomerName { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerAddress { get; set; }

        [Required]
        [MaxLength(50)]
        public string CustomerMobile { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerMessage { get; set; }

        public PaymentMethod PaymentMethod { set; get; }

        public BillStatus BillStatus { set; get; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        [DefaultValue(Status.Active)]
        public Status Status { get; set; }

        public Guid? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual AppUser User { get; set; }

        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}
