using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esdms.Models
{
    /// <summary>
    /// 縣市
    /// </summary>
    [Table("City")]
    public class City
    {
        [Key]
        [ColumnDef(Display = "縣市代碼")]
        [StringLength(6)]
        public string CityCode { get; set; }

        [Required]
        [StringLength(30)]
        [Column(TypeName = "nvarchar")]
        [Display(Name = "縣市名稱(中)")]
        public string Name { get; set; }        
    }

    public class CitySelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.Models.CitySelectItems, Esdms";

        protected static IEnumerable<City> _cites;
        internal static IEnumerable<City> CITIES
        {
            get
            {
                if (_cites == null)
                {
                    using (var db = new Esdms.Models.EsdmsModelContextExt())
                    {
                        _cites = db.City.ToArray();
                    }
                }
                return _cites;
            }
        }


        public static void Reset()
        {
            _cites = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return CITIES.Select(s => new KeyValuePair<string, object>(s.CityCode + "", s.Name));
        }
    }
}