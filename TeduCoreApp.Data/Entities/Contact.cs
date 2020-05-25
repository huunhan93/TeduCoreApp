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
    [Table("Caontacts")]
    public class Contact : DomainEntity<int>,
        ISwitchable
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(250)]
        public string Website { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        public string Other { get; set; }

        public double? Lng { get; set; }

        public double? Lat { get; set; }

        [DefaultValue(Status.Active)]
        public Status Status { get; set; }
    }
}
