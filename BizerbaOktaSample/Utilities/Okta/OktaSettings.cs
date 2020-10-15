using System;

namespace BizerbaOktaSample.Utilities.Okta
{
    public class OktaSettings
    {
        public String OktaDomain { get; set; }
        public String ClientId { get; set; }
        public String ClientSecret { get; set; }
        public String Domain { get; set; }
        public String PostLogoutRedirectUri { get; set; }
        public String AccessToken { get; set; }

        public String TokenUrl
        {
            get
            {
                return $"{this.OktaDomain}/oauth2/default/v1/token";
            }
        }

        public String UserInfoUrl
        {
            get
            {
                return $"{this.OktaDomain}/api/v1/users";
            }
        }
    }
}