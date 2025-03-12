using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

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
        
        [Display(Name = "年度")]
        [ColumnDef(EditType = EditType.Select,
            Filter = true, SelectItemsClassNamespace = Esdms.GetYaerSelectItems.AssemblyQualifiedName)]
        public int? Year { get; set; }

        [Display(Name = "專案編號")]
        [StringLength(9)]
        public string PrjId { get; set; }

        [Display(Name = "舊專案編號")]
        [Column(TypeName = "nvarchar")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        [StringLength(50)]
        public string PjNo { get; set; }

        //專案名稱(全名)
        [Required]
        [Column(TypeName = "nvarchar")]
        [ColumnDef(EditType = EditType.TextList,
            SelectItemsClassNamespace = ProjectNameSelectItems.AssemblyQualifiedName,
            Filter = true, FilterAssign = FilterAssignType.Contains)]
        [Display(Name = "專案名稱")]
        public string Name { get; set; }

        [Display(Name = "專案名稱(簡稱)")]
        [StringLength(20)]
        [ColumnDef(Visible = false)]
        public string BriefName { get; set; }

        [Column(TypeName = "nvarchar")]
        [Display(Name = "委辦單位")]
        public string CommissionedUnit { get; set; }
        
        [Display(Name = "建檔日期")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 6)]
        public DateTime? BDate { get; set; }

        [Display(Name = "建檔人員編")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 6)]
        [StringLength(6)]
        public string BFno { get; set; }

        [Display(Name = "建檔人姓名")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 6)]
        [StringLength(50)]
        public string BName { get; set; }

        [Display(Name = "修改日期")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 6)]
        public DateTime? UDate { get; set; }

        [Display(Name = "修改人員編")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 6)]
        [StringLength(6)]
        public string UFno { get; set; }

        [Display(Name = "修改人姓名")]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 6)]
        [StringLength(50)]
        public string UName { get; set; }        

        [Display(Name = "財務專案編號")]
        [StringLength(20)]
        [ColumnDef(Visible = false, ColSize = 6)]
        public string PjNoM { get; set; }

        [Display(Name = "財務專案名稱")]
        [StringLength(100)]
        [ColumnDef(Visible = false, ColSize = 6)]
        public string PjNameM { get; set; }

        [Display(Name = "委辦單位(業主A)")]
        [StringLength(50)]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 6)]
        public string OwnerA { get; set; }

        [Display(Name = "委辦單位(業主B)")]
        [StringLength(50)]
        [ColumnDef(Visible = false, VisibleEdit = false, ColSize = 6)]
        public string OwnerB { get; set; }

        [Display(Name = "專案起始日期")]
        [Column(TypeName = "date")]
        [ColumnDef(EditType = EditType.Date, Sortable = true,
            ColSize = 6)]
        public DateTime? PrjStartDate { get; set; }

        [Display(Name = "專案終止日期")]
        [Column(TypeName = "date")]
        [ColumnDef(EditType = EditType.Date, Sortable = true,
            ColSize = 6)]
        public DateTime? PrjEndDate { get; set; }

        [Display(Name = "經費")]
        [ColumnDef(ColSize = 6)]
        public int? Fee { get; set; }
    }

    //專案自行新增
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
                        _projects = db.Project.OrderByDescending(a => a.Year).ToArray();
                    }
                }
                return _projects;
            }
        }


        public static void Reset()
        {
            _projects = null;
            ////ProjectIntegrateSelectItems.Reset();
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {            
            return Projects.OrderByDescending(a => a.Year)
                    .Select((s, index) => new KeyValuePair<string, object>(s.Id.ToString(), JsonConvert.SerializeObject(new { v = s.Name, s = index, Year = s.Year })));
        }
    }

    /// <summary>
    /// 員工下拉：員編
    /// </summary>
    public class ProjectNameSelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.Models.ProjectNameSelectItems, Esdms";

        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return ProjectSelectItems.Projects.Select(s => new KeyValuePair<string, object>(s.Name, s.Name));
        }
    }

    ////public class ProjectIntegrate
    ////{
    ////    [Display(Name = "順序")]
    ////    public int SerialNo { get; set; }

    ////    [Display(Name = "專案編號")]
    ////    public string PrjID { get; set; }

    ////    [Display(Name = "專案名稱")]
    ////    public string Name { get; set; }                
    ////}

    //////////產基會 + 專案自行新增
    ////////public class ProjectIntegrateSelectItems : Dou.Misc.Attr.SelectItemsClass
    ////////{
    ////////    public const string AssemblyQualifiedName = "Esdms.Models.ProjectIntegrateSelectItems, Esdms";

    ////////    protected static IEnumerable<ProjectIntegrate> _projectIntegrate;
    ////////    internal static IEnumerable<ProjectIntegrate> ProjectIntegrate
    ////////    {
    ////////        get
    ////////        {
    ////////            if (_projectIntegrate == null)
    ////////            {
    ////////                using (var db = new EsdmsModelContextExt())
    ////////                {
    ////////                    var a1 = ProjectSelectItems.Projects.Select(a => new 
    ////////                    {
    ////////                        SType = 1,
    ////////                        Year = a.Id,
    ////////                        PrjID = a.Id.ToString(),
    ////////                        Name = a.Name,
    ////////                    }).ToArray();
    ////////                    var a2 = FtisHelperV2.DB.Helpe.Project.GetAllProject().Select(a => new 
    ////////                    {
    ////////                        SType = 2,
    ////////                        Year = a.PrjYear,
    ////////                        PrjID = a.PrjID,
    ////////                        Name = a.PrjName
    ////////                    }).ToArray();

    ////////                    _projectIntegrate = a1.Concat(a2).OrderBy(a => a.SType).ThenByDescending(a => a.Year).ThenBy(a => a.Name)
    ////////                                        .Select((a, index) => new ProjectIntegrate { 
    ////////                                            SerialNo = index,
    ////////                                            PrjID = a.PrjID,
    ////////                                            Name = a.Name,
    ////////                                        });
    ////////                }
    ////////            }
    ////////            return _projectIntegrate;
    ////////        }
    ////////    }


    ////////    public static void Reset()
    ////////    {
    ////////        _projectIntegrate = null;
    ////////    }
    ////////    public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
    ////////    {
    ////////        return ProjectIntegrate.Select(s => new KeyValuePair<string, object>(s.PrjID, JsonConvert.SerializeObject(new { v = s.Name, s = s.SerialNo })));            
    ////////    }
    ////////}
}