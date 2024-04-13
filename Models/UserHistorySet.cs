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
    /// 組別 (專家參與紀錄會外)
    /// </summary>
    [Table("UserHistorySet")]
    public class UserHistorySet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [ColumnDef(Visible = false)]
        public Int64 Id { get; set; }

        [Required]
        [Display(Name = "對應專家參與紀錄Id")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int FtisUHId { get; set; }        

        [Required]
        [StringLength(40)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "處室組別名稱")]
        public string Name { get; set; }
        
        [Display(Name = "年度")]
        public int? Year { get; set; }

        //組別標案
        [NotMapped]
        public virtual ICollection<UserHistorySetBid> UserHistorySetBids
        {
            get
            {
                return UserHistorySetBid.GetAllDatas().Where(a => a.UHSetId == this.Id).ToList();
            }
        }

        //標案總數量
        [NotMapped]
        [Display(Name = "標案總數量")]
        public virtual int BidCount
        {
            get
            {
                var datas = this.UserHistorySetBids;
                int count = datas.Count();

                return count;
            }
        }

        static object lockGetAllDatas = new object();
        public static IEnumerable<UserHistorySet> GetAllDatas(int cachetimer = 0)
        {
            if (cachetimer == 0) cachetimer = Constant.cacheTime;

            string key = "Esdms.Models.UserHistorySet";
            var allData = DouHelper.Misc.GetCache<IEnumerable<UserHistorySet>>(cachetimer, key);
            lock (lockGetAllDatas)
            {
                if (allData == null)
                {
                    Dou.Models.DB.IModelEntity<UserHistorySet> modle = new Dou.Models.DB.ModelEntity<UserHistorySet>(new EsdmsModelContextExt());
                    allData = modle.GetAll().OrderBy(a => a.Id).ToArray();

                    DouHelper.Misc.AddCache(allData, key);
                }
            }

            return allData;
        }

        public static void ResetGetAllDatas()
        {
            string key = "Esdms.Models.UserHistorySet";
            Misc.ClearCache(key);
        }
    }
}