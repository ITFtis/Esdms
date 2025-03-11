using Dou.Misc.Attr;
using DouHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Esdms.Models
{
    /// <summary>
    /// 專案請款
    /// </summary>
    [Table("ProjectInvoice")]
    public class ProjectInvoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "專案編號")]        
        public string PrjID { get; set; }

        [Required]
        [Display(Name = "工項")]
        public string WorkItem { get; set; }

        [Required]
        [Display(Name = "科目")]
        public string CostCode { get; set; }

        [Display(Name = "合約金額")]
        public int? Fee { get; set; }

        [Display(Name = "建檔日")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public DateTime? BDate { get; set; }

        [Display(Name = "建檔人帳號")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        [StringLength(24)]
        public string BFno { get; set; }

        [Display(Name = "建檔人姓名")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        [StringLength(50)]
        public string BName { get; set; }

        [Display(Name = "修改日")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public DateTime? UDate { get; set; }

        [Display(Name = "修改人帳號")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        [StringLength(24)]
        public string UFno { get; set; }

        [Display(Name = "修改人姓名")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        [StringLength(50)]
        public string UName { get; set; }

        //專案請款學者明細
        [NotMapped]
        public virtual ICollection<ProjectInvoiceBasic> ProjectInvoiceBasics
        {
            get
            {
                return ProjectInvoiceBasic.GetAllDatas().Where(a => a.MId == this.Id).ToList();
            }
        }

        static object lockGetAllDatas = new object();
        public static IEnumerable<ProjectInvoice> GetAllDatas(int cachetimer = 0)
        {
            if (cachetimer == 0) cachetimer = Constant.cacheTime;

            string key = "Esdms.Models.ProjectInvoice";
            var allData = DouHelper.Misc.GetCache<IEnumerable<ProjectInvoice>>(cachetimer, key);
            lock (lockGetAllDatas)
            {
                if (allData == null)
                {
                    Dou.Models.DB.IModelEntity<ProjectInvoice> modle = new Dou.Models.DB.ModelEntity<ProjectInvoice>(new EsdmsModelContextExt());
                    allData = modle.GetAll().ToArray();

                    DouHelper.Misc.AddCache(allData, key);
                }
            }

            return allData;
        }

        public static void ResetGetAllDatas()
        {
            string key = "Esdms.Models.ProjectInvoice";
            Misc.ClearCache(key);
        }
    }
}