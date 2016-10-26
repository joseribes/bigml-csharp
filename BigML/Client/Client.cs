using System;

namespace BigML
{
    /// <summary>
    /// BigML client.
    /// </summary>
    public partial class Client
    {
        internal string _apiKey;
        internal string _username;
        internal string _dev;
        internal string _protocol;
        internal string _VpcDomain;
        internal bool _useContextInAwaits;

        const string DefaultDomain = "bigml.io";
        const string DefaultProtocol = "https";

        public Client(string userName, string apiKey, bool devMode=false,
                      string vpcDomain="", bool useContextInAwaits = true)
        {
            _apiKey = apiKey;
            _username = userName;
            _dev = (devMode) ? "dev/" : "";
            _protocol = DefaultProtocol;
            _VpcDomain = (vpcDomain.Trim().Length > 0) ? vpcDomain : DefaultDomain;
            _useContextInAwaits = useContextInAwaits;
        }
    }
}