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
    /// 組別 (專家參與紀錄會外)
    /// </summary>
    [Table("UserHistorySet")]
    public class UserHistorySet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [ColumnDef(Visible = false)]
        public Int64 Id { get; set; }

        [Required]
        [Display(Name = "對應專家參與紀錄Id")]
        public int FtisUHId { get; set; }
        
        [StringLength(40)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "委辦單位")]
        public string CommUnit { get; set; }

        [Required]
        [StringLength(40)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "組別名稱")]
        public string Name { get; set; }
        
        [Display(Name = "年度")]
        public int? Year { get; set; }
    }
}