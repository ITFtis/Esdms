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
    /// 組別標案 (專家參與紀錄會外)
    /// </summary>
    [Table("UserHistorySetBid")]
    public class UserHistorySetBid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [ColumnDef(Visible = false)]
        public Int64 Id { get; set; }
                
        [Display(Name = "UHSetId")]
        [ColumnDef(Visible = false)]
        public Int64? UHSetId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "標案名稱")]
        public string Name { get; set; }

        static object lockGetAllDatas = new object();
        public static IEnumerable<UserHistorySetBid> GetAllDatas(int cachetimer = 0)
        {
            if (cachetimer == 0) cachetimer = Constant.cacheTime;

            string key = "Esdms.Models.UserHistorySetBid";
            var allData = DouHelper.Misc.GetCache<IEnumerable<UserHistorySetBid>>(cachetimer, key);
            lock (lockGetAllDatas)
            {
                if (allData == null)
                {
                    Dou.Models.DB.IModelEntity<UserHistorySetBid> modle = new Dou.Models.DB.ModelEntity<UserHistorySetBid>(new EsdmsModelContextExt());
                    allData = modle.GetAll().OrderBy(a => a.Id).ToArray();

                    DouHelper.Misc.AddCache(allData, key);
                }
            }

            return allData;
        }

        public static void ResetGetAllDatas()
        {
            string key = "Esdms.Models.UserHistorySetBid";
            Misc.ClearCache(key);
        }
    }
}