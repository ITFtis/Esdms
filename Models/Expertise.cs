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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [ColumnDef(Visible = false)]
        public int Id { get; set; }

        [Display(Name = "身分代碼")]
        [ColumnDef(ColSize = 3)]        
        [StringLength(13)]
        public string PId { get; set; }

        [Display(Name = "專長類別")]
        [ColumnDef(Filter = true,
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.SubjectSelectItems.AssemblyQualifiedName,
            SelectGearingWith = "SubjectDetailId,SubjectId,true", ColSize = 3)]
        public int? SubjectId { get; set; }

        [Display(Name = "專長領域")]
        [ColumnDef(Filter = true,
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.SubjectDetailSelectItems.AssemblyQualifiedName,
            ColSize = 3)]
        public int? SubjectDetailId { get; set; }

        [Display(Name = "專長內容")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
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
                    allData = modle.GetAll().OrderBy(a => a.SubjectId).ToArray();

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