﻿using System.Collections.Generic;

namespace ASM.Util.Models
{
    public class UserInfoModel
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public Dictionary<string, List<string>> UserClaims { get; set; }
    }
}