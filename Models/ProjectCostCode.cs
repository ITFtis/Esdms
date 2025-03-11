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
    /// 專案費用科目代碼
    /// </summary>
    [Table("ProjectCostCode")]
    public class ProjectCostCode
    {
        [Key]
        [StringLength(20)]
        [Display(Name = "代碼")]
        public string Code { get; set; }

        [StringLength(50)]
        [Display(Name = "費用名稱")]
        public string Name { get; set; }

        [Display(Name = "排序")]
        public int Sort { get; set; }

        [Display(Name = "建檔日期")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        public DateTime? BDate { get; set; }

        [Display(Name = "建檔人員編")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(6)]
        public string BFno { get; set; }

        [Display(Name = "修改日期")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        public DateTime? UDate { get; set; }

        [Display(Name = "修改人員編")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(6)]
        public string UFno { get; set; }

        static object lockGetAllDatas = new object();
        public static IEnumerable<ProjectCostCode> GetAllDatas(int cachetimer = 0)
        {
            if (cachetimer == 0) cachetimer = Constant.cacheTime;

            string key = "Esdms.Models.ProjectCostCode";
            var allData = DouHelper.Misc.GetCache<IEnumerable<ProjectCostCode>>(cachetimer, key);
            lock (lockGetAllDatas)
            {
                if (allData == null)
                {
                    Dou.Models.DB.IModelEntity<ProjectCostCode> modle = new Dou.Models.DB.ModelEntity<ProjectCostCode>(new EsdmsModelContextExt());
                    allData = modle.GetAll().ToArray();

                    DouHelper.Misc.AddCache(allData, key);
                }
            }

            return allData;
        }

        public static void ResetGetAllDatas()
        {
            string key = "Esdms.Models.ProjectCostCode";
            Misc.ClearCache(key);
        }
    }
}