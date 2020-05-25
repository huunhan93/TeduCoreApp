using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TeduCoreApp.Infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("AdvertisementPages")]
    public class AdvertisementPage : DomainEntity<string>
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public virtual ICollection<AdvertisementPosition> AdvertismentPositions { get; set; }
    }
}
