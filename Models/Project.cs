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
    /// 專案
    /// </summary>
    [Table("Project")]
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [ColumnDef(Visible = false)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "專案名稱")]
        public string Name { get; set; }
        
        [Display(Name = "工務二三階單位")]
        [ColumnDef(Filter = true, Sortable = true, EditType = EditType.Select, 
            SelectItemsClassNamespace = Esdms.Models.ProjectUnitSelectItems.AssemblyQualifiedName)]        
        public string AgencyId { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "執行單位")]
        public string ExecuteUnit { get; set; }
    }

    public class ProjectSelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.Models.ProjectSelectItems, Esdms";

        protected static IEnumerable<Project> _projects;
        internal static IEnumerable<Project> Projects
        {
            get
            {
                if (_projects == null)
                {
                    using (var db = new EsdmsModelContextExt())
                    {
                        _projects = db.Project.ToArray();
                    }
                }
                return _projects;
            }
        }


        public static void Reset()
        {
            _projects = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return Projects.Select(s => new KeyValuePair<string, object>(s.Id.ToString(), s.Name));
        }
    }
}