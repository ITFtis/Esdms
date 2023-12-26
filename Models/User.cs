using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Esdms.Models
{
    [Table("User")]
    public partial class User : Dou.Models.UserBase { }

}