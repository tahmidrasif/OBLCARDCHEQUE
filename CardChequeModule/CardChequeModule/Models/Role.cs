using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CardChequeModule.Models;

namespace TestTemplate.Models
{
    public class Role:RoleProvider
    {
        OBLCARDCHEQUEEntities db=new OBLCARDCHEQUEEntities();
        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string userid)
        {
            long id = Convert.ToInt32(userid);
            var roleId = db.OCCUSER.Where(x => x.ID == id).Select(x => x.TYPE).FirstOrDefault();
    
            string role = RoleCheck(roleId);

            string[] roles = { role };
            return roles;
       
       
        }

        private string RoleCheck(long? roleId)
        {
            string rolename=db.OCCENUMERATION.Where(x => x.ID == roleId).Select(x => x.Name).FirstOrDefault();
            return rolename;
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}