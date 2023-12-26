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
    /// 專家證照 public int Id { get; set; }
    /// </summary>
    [Table("BasicUser_License")]
    public class BasicUser_License
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

        [Required]
        [ColumnDef(Display = "證照", EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.LicenseSelectItems.AssemblyQualifiedName, Filter = true,
            ColSize = 3)]        
        public int? LicenseId { get; set; }

        [Display(Name = "建檔日期")]
        [ColumnDef(VisibleEdit = false, ColSize = 3)]
        public DateTime? BDate { get; set; }

        [Display(Name = "建檔人員編")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(6)]
        public string BFno { get; set; }

        [Display(Name = "建檔人姓名")]
        [ColumnDef(VisibleEdit = false, ColSize = 3)]
        [StringLength(50)]
        public string BName { get; set; }

        [Display(Name = "修改日期")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        public DateTime? UDate { get; set; }

        [Display(Name = "修改人員編")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(6)]
        public string UFno { get; set; }

        [Display(Name = "修改人姓名")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(50)]
        public string UName { get; set; }

        static object lockGetAllDatas = new object();
        public static IEnumerable<BasicUser_License> GetAllDatas(int cachetimer = 0)
        {
            if (cachetimer == 0) cachetimer = Constant.cacheTime;

            string key = "Esdms.Models.BasicUser_License";
            var allData = DouHelper.Misc.GetCache<IEnumerable<BasicUser_License>>(cachetimer, key);
            lock (lockGetAllDatas)
            {
                if (allData == null)
                {
                    Dou.Models.DB.IModelEntity<BasicUser_License> modle = new Dou.Models.DB.ModelEntity<BasicUser_License>(new EsdmsModelContextExt());
                    allData = modle.GetAll().ToArray();

                    DouHelper.Misc.AddCache(allData, key);
                }
            }

            return allData;
        }

        public static void ResetGetAllDatas()
        {
            string key = "Esdms.Models.BasicUser_License";
            Misc.ClearCache(key);
        }
    }
}