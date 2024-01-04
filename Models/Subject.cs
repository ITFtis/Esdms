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
    /// 科目類別代碼
    /// </summary>
    [Table("Subject")]
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [ColumnDef(Visible = false)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "專長類別")]
        public string Name { get; set; }
        
        [Display(Name = "排序")]
        [ColumnDef(Visible = false)]
        public int Sort { get; set; }

        static object lockGetAllDatas = new object();
        public static IEnumerable<Subject> GetAllDatas(int cachetimer = 0)
        {
            if (cachetimer == 0) cachetimer = Constant.cacheTime;

            string key = "Esdms.Models.Subject";
            var allData = DouHelper.Misc.GetCache<IEnumerable<Subject>>(cachetimer, key);
            lock (lockGetAllDatas)
            {
                if (allData == null)
                {
                    Dou.Models.DB.IModelEntity<Subject> modle = new Dou.Models.DB.ModelEntity<Subject>(new EsdmsModelContextExt());
                    allData = modle.GetAll().OrderBy(a => a.Sort).ToArray();                    

                    DouHelper.Misc.AddCache(allData, key);
                }
            }

            return allData;
        }

        public static void ResetGetAllDatas()
        {
            string key = "Esdms.Models.Subject";
            Misc.ClearCache(key);
        }
    }

    public class SubjectSelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.Models.SubjectSelectItems, Esdms";

        protected static IEnumerable<Subject> _subjects;
        internal static IEnumerable<Subject> Subjects
        {
            get
            {
                if (_subjects == null)
                {
                    using (var db = new EsdmsModelContextExt())
                    {
                        _subjects = db.Subject.OrderBy(a => a.Sort).ToArray();
                    }
                }
                return _subjects;
            }
        }


        public static void Reset()
        {
            _subjects = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return Subjects.Select(s => new KeyValuePair<string, object>(s.Id.ToString(), JsonConvert.SerializeObject(new { v = s.Name, s = s.Sort })));            
        }
    }
}