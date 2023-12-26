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
    /// Ftis活動計畫參與
    /// </summary>
    [Table("FTISUserHistory")]
    public class FTISUserHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [ColumnDef(Visible = false)]
        public int Id { get; set; }

        [Display(Name = "身分證字號")]
        [ColumnDef(ColSize = 3)]
        [StringLength(10)]
        public string PId { get; set; }

        [ColumnDef(Display = "部門", EditType = EditType.Select, SelectItemsClassNamespace = "FtisHelperV2.DB.DepartmentSelectItemsClassImp, FtisHelperV2", Filter = true, 
            ColSize = 3)]
        [StringLength(2)]
        public string DCode { get; set; }

        [Display(Name = "日期")]
        [ColumnDef(EditType = EditType.Date, ColSize = 3)]
        public DateTime Date { get; set; }

        [Display(Name = "專案")]
        [ColumnDef(Filter = true,
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.ProjectSelectItems.AssemblyQualifiedName,
            ColSize = 3)]        
        public int ProjectId { get; set; }

        [Display(Name = "專案類型")]
        [ColumnDef(Filter = true,
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.ActivityCategorySelectItems.AssemblyQualifiedName,
            ColSize = 3)]
        public int ActivityCategoryId { get; set; }

        static object lockGetAllDatas = new object();
        public static IEnumerable<FTISUserHistory> GetAllDatas(int cachetimer = 0)
        {
            if (cachetimer == 0) cachetimer = Constant.cacheTime;

            string key = "Esdms.Models.FTISUserHistory";
            var allData = DouHelper.Misc.GetCache<IEnumerable<FTISUserHistory>>(cachetimer, key);
            lock (lockGetAllDatas)
            {
                if (allData == null)
                {
                    Dou.Models.DB.IModelEntity<FTISUserHistory> modle = new Dou.Models.DB.ModelEntity<FTISUserHistory>(new EsdmsModelContextExt());
                    allData = modle.GetAll().ToArray();

                    DouHelper.Misc.AddCache(allData, key);
                }
            }

            return allData;
        }

        public static void ResetGetAllDatas()
        {
            string key = "Esdms.Models.FTISUserHistory";
            Misc.ClearCache(key);
        }
    }
}