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
    /// 專家專長
    /// </summary>
    [Table("Expertise")]
    public class Expertise
    {
        [Key]
        [Display(Name = "身分證字號")]
        [ColumnDef(ColSize = 3)]
        [Column(Order = 1)]
        [StringLength(10)]
        public string PId { get; set; }

        [Key]
        [Display(Name = "科別代碼")]
        [ColumnDef(Filter = true,
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.SubjectSelectItems.AssemblyQualifiedName,
            ColSize = 3)]
        [Column(Order = 2)]
        [StringLength(20)]
        public string SubjectCode { get; set; }

        [Display(Name = "類別名稱")]
        [StringLength(50)]
        [ColumnDef(ColSize = 3)]
        public string TypeName { get; set; }

        [Display(Name = "科別名稱")]
        [StringLength(50)]
        [ColumnDef(ColSize = 3)]
        public string SName { get; set; }

        [Display(Name = "專長內容")]
        [ColumnDef(ColSize = 3)]
        [StringLength(500)]
        public string Note { get; set; }

        static object lockGetAllDatas = new object();
        public static IEnumerable<Expertise> GetAllDatas(int cachetimer = 0)
        {
            if (cachetimer == 0) cachetimer = Constant.cacheTime;

            string key = "Esdms.Models.Expertise";
            var allData = DouHelper.Misc.GetCache<IEnumerable<Expertise>>(cachetimer, key);
            lock (lockGetAllDatas)
            {
                if (allData == null)
                {
                    Dou.Models.DB.IModelEntity<Expertise> modle = new Dou.Models.DB.ModelEntity<Expertise>(new EsdmsModelContextExt());
                    //allData = modle.GetAll().OrderBy(a => a.Rank).ToArray();
                    allData = modle.GetAll().ToArray();

                    DouHelper.Misc.AddCache(allData, key);
                }
            }

            return allData;
        }

        public static void ResetGetAllDatas()
        {
            string key = "Esdms.Models.Expertise";
            Misc.ClearCache(key);
        }
    }
}