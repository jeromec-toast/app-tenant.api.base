using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace Tenant.API.Base.Email
{
    public class MailContext
    {
        public bool IsBodyHtml;
        public bool EnableSsl;
        public bool UseDefaultCredentials;
        public int Port;
        public string From;
        public string UserName;
        public string Password;
        public string Subject;
        public string Body;
        public string Host;
        public string[] ToAddresses;
        public string[] CcAddresses;
        public Dictionary<string, Stream> Attachments;
    }
}
