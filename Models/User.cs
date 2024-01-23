using Dou.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Esdms.Models
{
    [Table("User")]
    public partial class User : Dou.Models.UserBase
    {
        //取得User所有角色
        public List<RoleUser> GetUserRoles()
        {
            var dbContext = new EsdmsModelContextExt();
            Dou.Models.DB.IModelEntity<RoleUser> roleUser = new Dou.Models.DB.ModelEntity<RoleUser>(dbContext);
            var roles = roleUser.GetAll().Where(a => a.UserId == Dou.Context.CurrentUserBase.Id).ToList();

            return roles;
        }
    }

}