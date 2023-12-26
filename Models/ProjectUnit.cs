using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Esdms.Models
{
    /// <summary>
    /// 專案單位
    /// </summary>
    [Table("ProjectUnit")]
    public class ProjectUnit
    {
        [Key]
        [StringLength(50)]
        [Column(TypeName = "nvarchar", Order = 1)]
        [Display(Name = "工務單位(二三階)Id")]
        public string AgencyId { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "工務單位(二三階)名稱")]
        public string AgencyName { get; set; }

        [Key]
        [StringLength(50)]
        [Column(TypeName = "nvarchar", Order = 2)]
        [Display(Name = "工務單位，(部級)Id")]
        public string DeparementId { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "工務單位，部級(Name)")]
        public string DeparementName { get; set; }
    }

    public class ProjectUnitSelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.Models.ProjectUnitSelectItems, Esdms";

        protected static IEnumerable<ProjectUnit> _projectUnits;
        internal static IEnumerable<ProjectUnit> ProjectUnits
        {
            get
            {
                if (_projectUnits == null)
                {
                    using (var db = new EsdmsModelContextExt())
                    {
                        _projectUnits = db.ProjectUnit.ToArray();
                    }
                }
                return _projectUnits;
            }
        }


        public static void Reset()
        {
            _projectUnits = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return ProjectUnits.Select(s => new KeyValuePair<string, object>(s.AgencyId.ToString(), s.AgencyName));
        }
    }
}