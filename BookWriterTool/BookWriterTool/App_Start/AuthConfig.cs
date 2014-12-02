namespace BookWriterTool
{
    using Microsoft.Web.WebPages.OAuth;

    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            /*Local konfiguration*/
            OAuthWebSecurity.RegisterFacebookClient(
                appId: "242345135934107",
                appSecret: "8d27dac9193ae9b81e7140e839137fe9");
            /*Release*/
            //OAuthWebSecurity.RegisterFacebookClient(
            // appId: "1435067763389188",
            // appSecret: "625f5ac112a4146a6a503d3ad70d9af6");

            OAuthWebSecurity.RegisterClient(client: new BookWriterTool.Helpers.GoogleCustomClient(), displayName: "Google Id", extraData: null);
        }
    }
}
