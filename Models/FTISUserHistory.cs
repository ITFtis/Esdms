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
    /// 專家參與紀錄
    /// </summary>
    [Table("FTISUserHistory")]
    public class FTISUserHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [ColumnDef(Visible = false)]
        public int Id { get; set; }

        [Display(Name = "身分代碼")]
        [ColumnDef(ColSize = 3)]
        [StringLength(13)]
        public string PId { get; set; }        
        
        [Display(Name = "會議類別")]
        [ColumnDef(Filter = true,
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.ActivityCategoryTypeSelectItems.AssemblyQualifiedName,
            SelectGearingWith = "ActivityCategoryId,ActivityCategoryType,true", ColSize = 3)]        
        public int? ActivityCategoryType { get; set; }
        
        [Display(Name = "會議名稱")]
        [ColumnDef(Filter = true,
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.ActivityCategorySelectItems.AssemblyQualifiedName,
            ColSize = 3)]
        public int? ActivityCategoryId { get; set; }

        [Display(Name = "會外年度")]
        [ColumnDef(EditType = EditType.Select,
            Filter = true, SelectItemsClassNamespace = Esdms.GetYaerSelectItems.AssemblyQualifiedName,
            ColSize = 3)]
        public int? OutYear { get; set; }

        [ColumnDef(Display = "部門", EditType = EditType.Select, SelectItemsClassNamespace = DepartmentSelectItems.AssemblyQualifiedName, Filter = true,
            ColSize = 3)]
        [StringLength(2)]
        public string DCode { get; set; }

        [Display(Name = "評選次數")]
        [ColumnDef(ColSize = 3)]
        public int? ActivityCategoryJoinNum { get; set; }

        [Display(Name = "業主")]
        [ColumnDef(ColSize = 3)]
        [StringLength(50)]
        public string Owner { get; set; }

        [Display(Name = "年度")]
        [ColumnDef(EditType = EditType.Select,
            Filter = true, SelectItemsClassNamespace = Esdms.GetYaerSelectItems.AssemblyQualifiedName, 
            ColSize = 3)]
        public int? Year { get; set; }

        [Display(Name = "日期")]
        [ColumnDef(Visible = false, VisibleEdit = false, EditType = EditType.Date, ColSize = 3)]
        public DateTime? Date { get; set; }
       
        [Display(Name = "專案")]
        [ColumnDef(Filter = true,
            EditType = EditType.TextList, SelectItemsClassNamespace = Esdms.Models.ProjectSelectItems.AssemblyQualifiedName,
            ColSize = 3)]        
        public int? ProjectId { get; set; }

        //組別
        [NotMapped]
        public virtual ICollection<UserHistorySet> UserHistorySets
        {
            get
            {                
                return UserHistorySet.GetAllDatas().Where(a => a.FtisUHId == this.Id).ToList();
            }
        }        

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
                    allData = modle.GetAll().OrderByDescending(a => a.Id).ToArray();

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