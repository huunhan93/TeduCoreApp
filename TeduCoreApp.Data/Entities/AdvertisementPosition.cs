using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TeduCoreApp.Infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("AdvertisementPositions")]
    public class AdvertisementPosition : DomainEntity<string>
    {
        [StringLength(20)]
        public string PageId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [ForeignKey("PageId")]
        public virtual AdvertisementPage AdvertisementPage { get; set; }

        public virtual ICollection<Advertisement> Advertisements { get; set; }
    }
}
