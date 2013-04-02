using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace SCUTClubManager.BusinessLogic
{
    public static class PasswordProcessor
    {
        public static string ProcessWithMD5(string password)
        {
            if (String.IsNullOrWhiteSpace(password))
            {
                return "";
            }

            ASCIIEncoding encoding = new ASCIIEncoding();

            var bytes = encoding.GetBytes(password.Trim());
            var hashed_bytes = MD5.Create().ComputeHash(bytes);
            var processed_password = Encoding.UTF8.GetString(hashed_bytes);

            return processed_password;
        }
    }
}