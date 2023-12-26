using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Esdms.Models
{
    /// <summary>
    /// 人員類別
    /// </summary>
    [Table("Category")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [ColumnDef(Visible = false)]
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "人員類別名稱")]
        public string Name { get; set; }
    }

    public class CategorySelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.Models.CategorySelectItems, Esdms";

        protected static IEnumerable<Category> _categorys;
        internal static IEnumerable<Category> Categorys
        {
            get
            {
                if (_categorys == null)
                {
                    using (var db = new EsdmsModelContextExt())
                    {
                        _categorys = db.Category.ToArray();
                    }
                }
                return _categorys;
            }
        }


        public static void Reset()
        {
            _categorys = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return Categorys.Select(s => new KeyValuePair<string, object>(s.Id.ToString(), s.Name));
        }
    }
}