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
    /// 科目代碼
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
                    //allData = modle.GetAll().OrderBy(a => a.Rank).ToArray();
                    allData = modle.GetAll().ToArray();

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
                        _subjectDetails = db.SubjectDetail.ToArray();
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
            return SubjectDetails.Select(s => new KeyValuePair<string, object>(s.Id.ToString(), "{\"v\":\"" + s.Name + "\",\"SubjectId\":\"" + s.SubjectId + "\"}"));
        }
    }
}