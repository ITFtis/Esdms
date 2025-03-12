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
        [ColumnDef(ColSize = 6)]
        public string PrjId { get; set; }

        //虛欄位
        [Display(Name = "財務專案編號")]
        [ColumnDef(ColSize = 3)]
        public string PrjPjNoM 
        {
            get
            {
                string str = "";
                var v = ProjectSelectItems.Projects.Where(a => a.PrjId == this.PrjId).FirstOrDefault();
                return v == null ? str : v.PjNoM;
            }
        }

        //虛欄位
        [Display(Name = "專案名稱")]
        [ColumnDef(ColSize = 3)]
        public string PrjName
        {
            get
            {
                string str = "";
                var v = ProjectSelectItems.Projects.Where(a => a.PrjId == this.PrjId).FirstOrDefault();
                return v == null ? str : v.Name;
            }
        }

        //虛欄位
        [Display(Name = "專案起始日期")]
        [ColumnDef(EditType = EditType.Date, ColSize = 3)]
        public DateTime? PrjStartDate
        {
            get
            {
                var v = ProjectSelectItems.Projects.Where(a => a.PrjId == this.PrjId).FirstOrDefault();
                return v == null ? (DateTime?)null : v.PrjStartDate;
            }
        }

        //虛欄位
        [Display(Name = "專案終止日期")]
        [ColumnDef(EditType = EditType.Date, ColSize = 3)]
        public DateTime? PrjEndDate
        {
            get
            {
                var v = ProjectSelectItems.Projects.Where(a => a.PrjId == this.PrjId).FirstOrDefault();
                return v == null ? (DateTime?)null : v.PrjEndDate;
            }
        }

        //虛欄位
        [Display(Name = "委辦單位")]
        [ColumnDef(ColSize = 6)]
        public string PrjCommissionedUnit
        {
            get
            {
                string str = "";
                var v = ProjectSelectItems.Projects.Where(a => a.PrjId == this.PrjId).FirstOrDefault();
                return v == null ? str : v.CommissionedUnit;
            }
        }

        [Required]
        [Display(Name = "工項")]
        [ColumnDef(ColSize = 3)]
        public string WorkItem { get; set; }

        [Required]
        [Display(Name = "科目")]
        [ColumnDef(EditType = EditType.Select,
            SelectItemsClassNamespace = Esdms.Models.ProjectCostCodeSelectItems.AssemblyQualifiedName,
            Filter = true, FilterAssign = FilterAssignType.Contains,
            ColSize = 3)]
        public string CostCode { get; set; }

        [Display(Name = "合約金額")]
        [ColumnDef(ColSize = 3)]
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