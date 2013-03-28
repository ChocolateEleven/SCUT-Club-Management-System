using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models.View_Models
{
    public class AuthenticationModel
    {
        private List<ClubScopedAuthenticationModel> memberships = new List<ClubScopedAuthenticationModel>();

        public string UserName { get; set; }
        public string Role { get; set; }
        public List<ClubScopedAuthenticationModel> Memberships { get { return memberships; } }
    }
}