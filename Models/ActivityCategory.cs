using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Esdms.Models
{
    /// <summary>
    /// 專案類型
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
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "類別名稱")]
        public string Name { get; set; }
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
            return ActivityCategorys.Select(s => new KeyValuePair<string, object>(s.Id.ToString(), s.Name));
        }
    }
}