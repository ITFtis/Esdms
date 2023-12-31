﻿using Dou.Misc.Attr;
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
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "類別名稱")]
        public string Name { get; set; }

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
            return ActivityCategorys.Select(s => new KeyValuePair<string, object>(s.Id.ToString(), s.Name));
        }
    }
}