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
        [Display(Name = "名稱(中)")]
        public string Name { get; set; }
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
                        _subjects = db.Subject.ToArray();
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
            return Subjects.Select(s => new KeyValuePair<string, object>(s.Id.ToString(), s.Name));
        }
    }
}