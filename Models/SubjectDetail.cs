using Dou.Misc.Attr;
using DouHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Esdms.Models
{
    /// <summary>
    /// 專長領域(D)
    /// </summary>
    [Table("SubjectDetail")]
    public class SubjectDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [ColumnDef(Visible = false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "專長類別")]
        [ColumnDef(Filter = true, EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.SubjectSelectItems.AssemblyQualifiedName)]        
        public int SubjectId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "專長領域")]
        public string Name { get; set; }
        
        [StringLength(40)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "匯入代碼")]
        public string DCode { get; set; }

        [Display(Name = "排序")]
        [ColumnDef(Visible = false)]
        public int Sort { get; set; }

        static object lockGetAllDatas = new object();
        public static IEnumerable<SubjectDetail> GetAllDatas(int cachetimer = 0)
        {
            if (cachetimer == 0) cachetimer = Constant.cacheTime;

            string key = "Esdms.Models.SubjectDetail";
            var allData = DouHelper.Misc.GetCache<IEnumerable<SubjectDetail>>(cachetimer, key);
            lock (lockGetAllDatas)
            {
                if (allData == null)
                {
                    Dou.Models.DB.IModelEntity<SubjectDetail> modle = new Dou.Models.DB.ModelEntity<SubjectDetail>(new EsdmsModelContextExt());
                    allData = modle.GetAll().OrderBy(a => a.Sort).ToArray();                    

                    DouHelper.Misc.AddCache(allData, key);
                }
            }

            return allData;
        }

        public static void ResetGetAllDatas()
        {
            string key = "Esdms.Models.SubjectDetail";
            Misc.ClearCache(key);
        }
    }

    public class SubjectDetailSelectItems : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.Models.SubjectDetailSelectItems, Esdms";

        protected static IEnumerable<SubjectDetail> _subjectDetails;
        internal static IEnumerable<SubjectDetail> SubjectDetails
        {
            get
            {
                if (_subjectDetails == null || _subjectDetails.Count() == 0)
                {
                    using (var db = new EsdmsModelContextExt())
                    {
                        _subjectDetails = db.SubjectDetail.OrderBy(a => a.Sort).ToArray();
                    }
                }
                return _subjectDetails;
            }
        }


        public static void Reset()
        {
            _subjectDetails = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            //return SubjectDetails.Select(s => new KeyValuePair<string, object>(s.Id.ToString(), "{\"v\":\"" + s.Name + "\",\"SubjectId\":\"" + s.SubjectId + "\"}"));
            return SubjectDetails.Select(s => new KeyValuePair<string, object>(s.Id.ToString(), JsonConvert.SerializeObject(new { v = s.Name, s = s.Sort, SubjectId = s.SubjectId })));
        }
    }
}