using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class IdentityForTPC
    {
        public const string APPLICATION = "Application";
        public const string ASSET_BASE = "AssetBase";
        public const string ROLE_BASE = "RoleBase";

        [Key]
        [MaxLength(30)]
        public string BaseName { get; set; }

        [Required]
        public int Identity { get; set; }
    }
}