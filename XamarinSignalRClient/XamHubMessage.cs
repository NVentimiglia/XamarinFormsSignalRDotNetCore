using System;

namespace XamarinSignalRClient
{
    /// <summary>
    /// Message Signature
    /// </summary>
    [Serializable]
    public class XamHubMessage
    {
        public string SenderId { get; set; }
        public string Payload { get; set; }
    }
}
