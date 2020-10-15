using System;

namespace BizerbaOktaSample.Utilities.Okta
{
    public class OktaUserGroup : IUserGroup
    {
        public String Id { get; set; }
        public String FriendlyName { get; set; }
        public String Description { get; set; }
    }
}