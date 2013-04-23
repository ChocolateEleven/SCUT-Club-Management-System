using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCUTClubManager.Models;
using SCUTClubManager.Models.View_Models;
using System.Web.Security;
using SCUTClubManager.DAL;

namespace SCUTClubManager.BusinessLogic
{
    /**
     *  用于处理已验证用户的访问权限。
     */
    public class ScmRoleProvider : RoleProvider
    {
        private static UnitOfWork context = new UnitOfWork();

        // 以下为社团内部权限管理用。

        /**
         *  查询当前用户是否属于某个社团的某个部门以及其职位。
         *  @param club_id 要查询的社团。
         *  @param branch_id 要查询的部门，为空则不查询该项。
         *  @param possible_roles 要查询的职位。为空则不查询该项，为多个值则意味着满足任意一个值皆可。
         *  @returns 查询结果。
         */
        public static bool HasMembershipIn(int club_id, int? branch_id = null, string[] possible_roles = null)
        {
            var user_identity = HttpContext.Current.User.Identity;

            if (user_identity.IsAuthenticated)
            {
                User user = context.Users.Find(user_identity.Name);

                // 只有学生才能成为社团成员。
                if (user is Student)
                {
                    Student student = user as Student;

                    if (student.MemberShips.Any(t => t.ClubId == club_id))
                    {
                        ClubMember membership = student.MemberShips.Single(t => t.ClubId == club_id);

                        if (possible_roles != null)
                        {
                            var all_roles = context.ClubRoles.ToList();

                            if (!possible_roles.All(t => all_roles.Any(p => p.Name.Equals(t))))
                            {
                                throw new ArgumentException("One or more undefined club roles are given.");
                            }
                        }

                        if ((branch_id == null || membership.BranchId == branch_id) && (possible_roles == null ||
                            possible_roles.Contains(membership.ClubRole.Name)))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /**
         *  查询当前用户在给定社团中的角色。
         *  @param club_id 要查询的社团。
         *  @returns 查询结果，若该用户不是给定社团的成员则为空。
         */
        public static ClubMember GetRoleInClub(int club_id)
        {
            var user_identity = HttpContext.Current.User.Identity;

            if (user_identity.IsAuthenticated)
            {
                User user = context.Users.Find(user_identity.Name);

                // 只有学生才能成为社团成员。
                if (user is Student)
                {
                    Student student = user as Student;
                    foreach (var membership in student.MemberShips)
                    {
                        if (membership.ClubId == club_id)
                        {
                            return membership;
                        }
                    }
                }
            }

            return null;
        }

        /**
         *  获取给定社团角色名称对应的Id。
         *  @param role_name 要查询的社团角色名称。
         *  @returns 查询结果，若不存在该角色则返回0。
         */
        public static int GetRoleIdByName(string role_name)
        {
            RoleBase role = context.RoleBases.ToList().SingleOrDefault(t => t.Name == role_name);

            if (role != null)
            {
                return role.Id;
            }
            else
            {
                return 0;
            }
        }

        // 以下为对用户全局权限的管理用。用于区分系统管理员和一般用户。

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string user_name)
        {
            User user = context.Users.Find(user_name);
            List<string> roles = new List<string>();

            if (user != null)
            {
                UserRole role = user.Role;

                if (role != null)
                {
                    roles.Add(role.Name);
                }
            }

            return roles.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        /**
         *  查询给定用户是否为某个角色。建议使用Attribute语法而不是直接调用该方法。
         *  @param user_name 要查询的用户的用户名。
         *  @param role_name 要查询的角色名。
         *  @returns 查询结果。
         */
        public override bool IsUserInRole(string user_name, string role_name)
        {
            if (!context.UserRoles.ToList().Any(t => t.Name.Equals(role_name)))
            {
                throw new ArgumentException("One or more undefined user roles are given.");
            }

            User user = context.Users.Find(user_name);

            if (user != null && user.Role.Name.Equals(role_name))
            {
                return true;
            }

            return false;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}