﻿using Domain.Common;

namespace Domain.BaseDomain.User
{
    public class IdentityUser : BaseEntity<Guid>
    {
        public IdentityUser()
        {
        }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Temp { get; set; }
        public bool IsActive { get; set; }
    }
}