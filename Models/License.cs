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
    /// 證照代碼
    /// </summary>
    [Table("License")]
    public class License
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [ColumnDef(Visible = false)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "證照名稱")]
        public string Name { get; set; }

        [StringLength(40)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "證照等級")]
        public string LevelName { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "機關")]
        public string Administration { get; set; }
    }

    public class LicenseSelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.Models.LicenseSelectItems, Esdms";

        protected static IEnumerable<License> _licenses;
        internal static IEnumerable<License> Licenses
        {
            get
            {
                if (_licenses == null)
                {
                    using (var db = new EsdmsModelContextExt())
                    {
                        _licenses = db.License.ToArray();
                    }
                }
                return _licenses;
            }
        }


        public static void Reset()
        {
            _licenses = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return Licenses.Select(s => new KeyValuePair<string, object>(s.Id.ToString(), s.Name));
        }
    }
}