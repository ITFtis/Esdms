using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DouHelper;

namespace Esdms.Models
{
    /// <summary>
    /// 經歷
    /// </summary>
    [Table("Resume")]
    public class Resume
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

        [Display(Name = "服務機關(構)名稱")]
        [ColumnDef(ColSize = 3)]
        [StringLength(100)]
        public string UnitName { get; set; }

        [Display(Name = "職稱")]
        [ColumnDef(ColSize = 3)]
        [StringLength(50)]
        public string Position { get; set; }

        [Display(Name = "所任工作")]
        [ColumnDef(ColSize = 3)]
        [StringLength(100)]
        public string Description { get; set; }

        [Display(Name = "任職起")]
        [ColumnDef(ColSize = 3, EditType = EditType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "任職迄")]
        [ColumnDef(ColSize = 3, EditType = EditType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "備註")]
        [ColumnDef(ColSize = 3)]
        [StringLength(1000)]
        public string Note { get; set; }

        static object lockGetAllDatas = new object();
        public static IEnumerable<Resume> GetAllDatas(int cachetimer = 0)
        {
            if (cachetimer == 0) cachetimer = Constant.cacheTime;

            string key = "Esdms.Models.Resume";
            var allData = DouHelper.Misc.GetCache<IEnumerable<Resume>>(cachetimer, key);
            lock (lockGetAllDatas)
            {
                if (allData == null)
                {
                    Dou.Models.DB.IModelEntity<Resume> modle = new Dou.Models.DB.ModelEntity<Resume>(new EsdmsModelContextExt());
                    allData = modle.GetAll().ToArray();

                    DouHelper.Misc.AddCache(allData, key);
                }
            }

            return allData;
        }

        public static void ResetGetAllDatas()
        {
            string key = "Esdms.Models.Resume";
            Misc.ClearCache(key);
        }
    }
}