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
        [Display(Name = "科目類別")]
        [ColumnDef(Filter = true, EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.SubjectSelectItems.AssemblyQualifiedName)]        
        public int MId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "名稱(中)")]
        public string Name { get; set; }
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
            return SubjectDetails.Select(s => new KeyValuePair<string, object>(s.Id.ToString(), "{\"v\":\"" + s.Name + "\",\"MId\":\"" + s.MId + "\"}"));
        }
    }
}