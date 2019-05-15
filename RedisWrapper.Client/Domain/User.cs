using System;

namespace RedisWrapper.Service.Domain
{
    public class User
    {
        public Guid IdUser { get; set; }
        public string Logon { get; set; }
        public string Email { get; set; }
    }
}
