using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCUTClubManager.Models;
using SCUTClubManager.Models.View_Models;
using System.Web.Security;

namespace SCUTClubManager.BusinessLogic
{
    /**
     *  用于处理用户身份验证信息本地存取的类，验证后的用户信息将以Cookies的形式存储在本地以便记录用户的身份以及登录状态。
     */
    public static class AuthenticationProcessor
    {
        /**
         *  序列化一个AuthenticationModel。该方法本该类内部调用，该类的使用者无需关注该方法。
         *  @param model 要被序列化的AutheticationModel。
         *  @returns 序列化后的字符串。
         */
        private static string SerializeAuthenticationModel(AuthenticationModel model)
        {
            string result;
            const string SEPARATOR = "-";

            result = model.UserName + SEPARATOR + model.Role;

            foreach (var membership in model.Memberships)
            {
                result += SEPARATOR + membership.ClubId + SEPARATOR + membership.BranchId + SEPARATOR + membership.ClubRole;
            }

            return result;
        }

        /**
         *  反序列化一个AuthenticationModel。该方法本该类内部调用，该类的使用者无需关注该方法。
         *  @param serialized_string 序列化后的字符串。
         *  @returns 反序列化后得到的AutheticationModel。
         */
        private static AuthenticationModel DeserializeAuthenticationModel(string serialized_string)
        {
            AuthenticationModel model = new AuthenticationModel();
            const string SEPARATOR = "-";
            int separator_index = 0;

            if (serialized_string.Contains(SEPARATOR))
            {
                separator_index = serialized_string.IndexOf(SEPARATOR);
                model.UserName = serialized_string.Substring(0, separator_index);
                serialized_string = serialized_string.Substring(separator_index + 1);

                separator_index = serialized_string.IndexOf(SEPARATOR);
                model.Role = serialized_string.Substring(0, separator_index);
                serialized_string = serialized_string.Substring(separator_index + 1);

                while (serialized_string.Contains(SEPARATOR))
                {
                    ClubScopedAuthenticationModel membership = new ClubScopedAuthenticationModel();

                    separator_index = serialized_string.IndexOf(SEPARATOR);
                    membership.ClubId = Int32.Parse(serialized_string.Substring(0, separator_index));
                    serialized_string = serialized_string.Substring(separator_index + 1);

                    separator_index = serialized_string.IndexOf(SEPARATOR);
                    membership.BranchId = Int32.Parse(serialized_string.Substring(0, separator_index));
                    serialized_string = serialized_string.Substring(separator_index + 1);

                    separator_index = serialized_string.IndexOf(SEPARATOR);
                    membership.ClubRole = serialized_string.Substring(0, separator_index);
                    serialized_string = serialized_string.Substring(separator_index + 1);
                }

                return model;
            }

            return null;
        }

        /**
         *  将用户验证信息记录在本地，在调用MembershipProcessor.ValidateUser之后调用这个方法来记录用户验证信息。
         *  @param user 要记录的用户。
         *  @param rememberMe 是否永远保留该验证信息，即一段时间后无需用户再次登录。
         */
        public static void Authenticate(User user, bool rememberMe)
        {
            HttpCookie cookie = FormsAuthentication.GetAuthCookie(user.UserName, rememberMe);
            var orig_ticket = FormsAuthentication.Decrypt(cookie.Value);
            AuthenticationModel model = new AuthenticationModel();

            model.UserName = user.UserName;
            model.Role = user.Role;

            if (user is Student)
            {
                Student student = user as Student;

                foreach (var membership in student.MemberShips)
                {
                    model.Memberships.Add(new ClubScopedAuthenticationModel
                    {
                        ClubId = membership.ClubId,
                        BranchId = membership.BranchId,
                        ClubRole = membership.ClubRole.Name
                    });
                }
            }

            string serialized_string = SerializeAuthenticationModel(model);
            var new_ticket = new FormsAuthenticationTicket(orig_ticket.Version, orig_ticket.Name, 
                orig_ticket.IssueDate, orig_ticket.Expiration, orig_ticket.IsPersistent, serialized_string);
            cookie.Value = FormsAuthentication.Encrypt(new_ticket);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /**
         *  当前用户是否已被验证（已登录）。只读。
         */
        public static bool HasAuthenticated
        {
            get
            {
                return HttpContext.Current.Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName);
            }
        }

        /**
         *  当前用户是否属于某个角色。例如：学生或者社联。
         *  @param role 要查询的角色。
         *  @returns 当前用户是否属于该角色。
         */
        public static bool MatchRole(string role)
        {
            if (HasAuthenticated)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

                if (cookie != null)
                {
                    var decrypted_ticket = FormsAuthentication.Decrypt(cookie.Value);
                    AuthenticationModel model = DeserializeAuthenticationModel(decrypted_ticket.UserData);

                    if (model != null && role.Equals(model.Role))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /**
         *  当前用户是否属于某个社团的某个部门的某个角色。该角色指社团内角色，不同于MatchRole所查询的角色。如：社团部长。
         *  @param club_id 要查询的社团。
         *  @param branch_id 要查询的部门，为空则不查询该项。
         *  @param possible_roles 要查询的角色。为空则不查询该项，为多个值则意味着满足任意一个值皆可。
         *  @returns 查询结果。
         */
        public static bool HasMembershipIn(int club_id, int? branch_id = null, string[] possible_roles = null)
        {
            if (HasAuthenticated)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

                if (cookie != null)
                {
                    var decrypted_ticket = FormsAuthentication.Decrypt(cookie.Value);
                    AuthenticationModel model = DeserializeAuthenticationModel(decrypted_ticket.UserData);

                    if (model != null)
                    {
                        foreach (var membership in model.Memberships)
                        {
                            if (membership.ClubId == club_id)
                            {
                                if ((branch_id == null || branch_id.Value == membership.BranchId) && (possible_roles == null || possible_roles.Contains(membership.ClubRole)))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        /**
         *  登出，在本地上消除当前用户的验证记录。
         */
        public static void LogOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}