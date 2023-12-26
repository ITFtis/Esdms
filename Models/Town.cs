using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Esdms.Models
{
    /// <summary>
    /// 鄉鎮
    /// </summary>
    [Table("Town")]
    public class Town
    {
        [Key]
        [Display(Name = "鄉鎮代碼")]
        [ColumnDef(Index = 4)]
        [StringLength(5)]
        public string ZIP { get; set; }  //郵遞區號

        [Required]
        [Display(Name = "縣市")]
        [ColumnDef(Index = 1, Filter = true, Sortable = true, EditType = EditType.Select, SelectItemsClassNamespace = Esdms.Models.CitySelectItems.AssemblyQualifiedName)]
        [StringLength(6)]
        public string CityCode { get; set; }

        [Required]        
        [Display(Name = "鄉鎮名稱(中)")]
        [Column(TypeName = "nvarchar")]        
        [ColumnDef(Index = 2)]
        [StringLength(20)]
        public string Name { get; set; }             
    }

    public class TownSelectItems : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.Models.TownSelectItems, Esdms";

        protected static IEnumerable<Town> _towns;
        internal static IEnumerable<Town> Towns
        {
            get
            {
                if (_towns == null || _towns.Count() == 0)
                {
                    using (var db = new EsdmsModelContextExt())
                    {
                        _towns = db.Town.ToArray();
                    }
                }
                return _towns;
            }
        }


        public static void Reset()
        {
            _towns = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return Towns.Select(s => new KeyValuePair<string, object>(s.ZIP, "{\"v\":\"" + s.Name + "\",\"CityCode\":\"" + s.CityCode + "\",\"PCityCode\":\"" + s.CityCode + "\"}"));
        }
    }
}