using Esdms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Esdms
{
    public class Department
    {
    }

    public class DepartmentSelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public const string AssemblyQualifiedName = "Esdms.DepartmentSelectItems, Esdms";

        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            //return Deps.Select(s => new KeyValuePair<string, object>(s.Id.ToString(), s.Name));
            return Code.GetDepartment().Select(s => new KeyValuePair<string, object>(s.Key, s.Value));
        }
    }
}