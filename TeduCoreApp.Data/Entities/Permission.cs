using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TeduCoreApp.Infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Permissions")]
    public class Permission : DomainEntity<int>
    {
        [StringLength(450)]
        [Required]
        public string RoleId { get; set; }

        public string FunctionId { get; set; }

        public bool CanCreate { get; set; }

        public bool CanRead { get; set; }

        public bool CanUpdate { get; set; }

        public bool CanDelete { get; set; }

        [ForeignKey("RoleId")]
        public virtual AppRole AppRole { get; set; }

        [ForeignKey("FunctionId")]
        public virtual Function Function { get; set; }
    }
}
