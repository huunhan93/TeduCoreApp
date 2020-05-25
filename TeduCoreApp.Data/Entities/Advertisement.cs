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
    public class Advertisement : DomainEntity<int>,
        ISwitchable, ISortable, IDateTracking
    {
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        [StringLength(250)]
        public string Url { get; set; }

        [StringLength(20)]
        public string PositionId { get; set; }

        [DefaultValue(Status.Active)]
        public Status Status { set; get; }

        public int SortOrder { set; get; }

        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }

        [ForeignKey("PositionId")]
        public virtual AdvertisementPosition AdvertisementPosition { get; set; }
    }
}
