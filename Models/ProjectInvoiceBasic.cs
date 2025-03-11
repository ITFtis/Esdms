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
    /// 專案請款學者明細
    /// </summary>
    [Table("ProjectInvoiceBasic")]
    public class ProjectInvoiceBasic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "對應專案請款編號")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int MId { get; set; }

        [Required]
        [Display(Name = "委員姓名")]
        public string BasicName { get; set; }

        [Display(Name = "公司名稱")]
        [StringLength(100)]
        public string CopName { get; set; }

        [Required]
        [Display(Name = "金額")]
        public int Amount { get; set; }

        [Required]
        [Display(Name = "請款日")]
        [ColumnDef(EditType = EditType.Date)]
        [Column(TypeName = "date")]
        public DateTime ApplyDate { get; set; }

        [Display(Name = "建檔日")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public DateTime? BDate { get; set; }

        [Display(Name = "建檔人帳號")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        [StringLength(24)]
        public string BId { get; set; }

        [Display(Name = "建檔人姓名")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        [StringLength(50)]
        public string BName { get; set; }
    }
}