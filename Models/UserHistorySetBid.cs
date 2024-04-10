using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Esdms.Models
{
    /// <summary>
    /// 組別標案 (專家參與紀錄會外)
    /// </summary>
    [Table("UserHistorySetBid")]
    public class UserHistorySetBid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [ColumnDef(Visible = false)]
        public Int64 Id { get; set; }
                
        [Display(Name = "UHSetId")]
        [ColumnDef(Visible = false)]
        public Int64? UHSetId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "標案名稱")]
        public string Name { get; set; }
    }
}