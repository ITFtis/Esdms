using Dou.Misc.Attr;
using Esdms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esdms
{
    public class CheckYear
    {
    }

    /// <summary>
    /// 年度
    /// </summary>
    public class GetYaerSelectItems : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.GetYaerSelectItems, Esdms";

        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            var v = Code.GetYaer().OrderByDescending(a=>a.Key)
                .Select((a, index) => new
                {
                    SerialNo = index,
                    a.Key,
                    a.Value
                });

            return v.Select(a => new KeyValuePair<string, object>(a.Key, JsonConvert.SerializeObject(new { v = a.Key, s = a.SerialNo })));            
        }
    }
}