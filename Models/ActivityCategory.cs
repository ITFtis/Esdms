using Dou.Misc.Attr;
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
    /// 會議
    /// </summary>
    [Table("ActivityCategory")]
    public class ActivityCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [ColumnDef(Visible = false)]
        public int Id { get; set; }

        [Required]                
        [Display(Name = "會議類別")]
        [ColumnDef(EditType = EditType.Select,
            Filter = true, SelectItemsClassNamespace = Esdms.Models.ActivityCategoryTypeSelectItems.AssemblyQualifiedName,
            ColSize = 3)]
        public int Type { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "會議名稱")]
        public string Name { get; set; }

        [StringLength(40)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "匯入代碼")]
        public string DCode { get; set; }

        [Display(Name = "排序")]
        [ColumnDef(Visible = false)]
        public int Sort { get; set; }
        
        [Display(Name = "不計評選次數")]
        [Required]
        [ColumnDef(EditType = EditType.Radio,
            SelectItemsClassNamespace = Esdms.GetYNSelectItems.AssemblyQualifiedName,
            ColSize = 3)]
        [StringLength(1)]
        public string IsNoCount { get; set; }

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
    }

    public class ActivityCategorySelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.Models.ActivityCategorySelectItems, Esdms";

        protected static IEnumerable<ActivityCategory> _activityCategorys;
        internal static IEnumerable<ActivityCategory> ActivityCategorys
        {
            get
            {
                if (_activityCategorys == null)
                {
                    using (var db = new EsdmsModelContextExt())
                    {
                        _activityCategorys = db.ActivityCategory.ToArray();
                    }
                }
                return _activityCategorys;
            }
        }


        public static void Reset()
        {
            _activityCategorys = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {            
            return ActivityCategorys.Select(s => new KeyValuePair<string, object>(s.Id.ToString(), JsonConvert.SerializeObject(new { v = s.Name, ActivityCategoryType = s.Type })));
        }
    }

    public class ActivityCategoryTypeSelectItems : Dou.Misc.Attr.SelectItemsClass
    {        
        public const string AssemblyQualifiedName = "Esdms.Models.ActivityCategoryTypeSelectItems, Esdms";

        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {            
            return Code.GetActivityCategoryType().Select(s => new KeyValuePair<string, object>(s.Key, s.Value));
        }
    }
}