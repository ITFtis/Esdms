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
    /// 專家基本資料 - 個資
    /// </summary>
    [Table("BasicUser_Private")]
    public class BasicUser_Private
    {
        [Key]
        [Display(Name = "身分代碼")]
        [ColumnDef(ColSize = 3)]
        [StringLength(10)]
        public string PId { get; set; }
							
        [Display(Name = "手機號碼")]
        [ColumnDef(ColSize = 3)]
        [StringLength(30)]
        public string PrivatePhone { get; set; }

        [Display(Name = "(住家)縣市")]
        [ColumnDef(Filter = true,
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.CitySelectItems.AssemblyQualifiedName,
            SelectGearingWith = "PZIP,PCityCode,true", ColSize = 3)]
        [StringLength(6)]
        public string PCityCode { get; set; }

        [Display(Name = "(住家)鄉鎮市區")]
        [ColumnDef(Filter = true,
            EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.TownSelectItems.AssemblyQualifiedName,
            ColSize = 3)]
        [StringLength(5)]
        public string PZIP { get; set; }

        [Display(Name = "(住家)地址")]
        [ColumnDef(ColSize = 3)]
        [StringLength(100)]
        public string PAddress { get; set; }

        [Display(Name = "LINE ID")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(100)]
        public string LINE { get; set; }

        [Display(Name = "最高學歷")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(50)]
        public string Education { get; set; }

        [Display(Name = "科系名稱")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(50)]
        public string EducationDepartment { get; set; }

        [Display(Name = "次高學歷")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(50)]
        public string MinorEducation { get; set; }

        [Display(Name = "興趣喜好")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(100)]
        public string Interest { get; set; }

        [Display(Name = "其他")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(200)]
        public string Note { get; set; }

        [Display(Name = "建檔日期")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        public DateTime? BDate { get; set; }

        [Display(Name = "建檔人員編")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
        [StringLength(6)]
        public string BFno { get; set; }

        [Display(Name = "建檔人姓名")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 3)]
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
        public static IEnumerable<BasicUser_Private> GetAllDatas(int cachetimer = 0)
        {
            if (cachetimer == 0) cachetimer = Constant.cacheTime;

            string key = "Esdms.Models.BasicUser_Private";
            var allData = DouHelper.Misc.GetCache<IEnumerable<BasicUser_Private>>(cachetimer, key);
            lock (lockGetAllDatas)
            {
                if (allData == null)
                {
                    Dou.Models.DB.IModelEntity<BasicUser_Private> modle = new Dou.Models.DB.ModelEntity<BasicUser_Private>(new EsdmsModelContextExt());                    
                    allData = modle.GetAll().ToArray();

                    DouHelper.Misc.AddCache(allData, key);
                }
            }

            return allData;
        }

        public static void ResetGetAllDatas()
        {
            string key = "Esdms.Models.BasicUser_Private";
            Misc.ClearCache(key);
        }
    }
}