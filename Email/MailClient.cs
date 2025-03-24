using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Tenant.API.Base.Email
{
    public class MailClient
    {
        #region Variables

        private MailContext MailContext;
        private MailMessage MailMessage;
        public IConfiguration Configuration;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XtraChef.API.Base.Email.MailClient"/> class.
        /// </summary>
        /// <param name="mailContext">Mail context.</param>
        /// /// <param name="configuration">configuration.</param>
        public MailClient(MailContext mailContext, IConfiguration configuration)
        {
            this.MailContext = mailContext;
            this.Configuration = configuration;
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Send the email
        /// </summary>
        /// <param name="alternateView">Override the mail body and to attach you custome html mail</param>
        public void SendMail(AlternateView alternateView = null)
        {
            try
            {
                this.MailMessage = new MailMessage();
                
                //Setting view 
                if (alternateView != null)
                {
                    this.MailMessage.From = new MailAddress(this.MailContext.From);
                    this.MailMessage.Subject = this.MailContext.Subject;
                    this.MailMessage.AlternateViews.Add(alternateView);
                }
                else
                {
                    
                    this.MailMessage.From = new MailAddress(this.MailContext.From);
                    this.MailMessage.Subject = this.MailContext.Subject;
                    this.MailMessage.Body = this.MailContext.Body;
                    this.MailMessage.IsBodyHtml = this.MailContext.IsBodyHtml;
                }

                if(this.Configuration["Environment"].ToLower() != "prod")
                {
                    this.MailMessage.To.Add(this.Configuration["TestEmail"]);
                }
                else
                {
                    //To addresses
                    foreach (string toAddress in this.MailContext.ToAddresses)
                    {
                        this.MailMessage.To.Add(toAddress);
                    }


                    //Cc addresses
                    if (this.MailContext.CcAddresses != null && this.MailContext.CcAddresses.Length > 0)
                    {
                        foreach (string ccAddress in this.MailContext.CcAddresses)
                        {
                            this.MailMessage.CC.Add(ccAddress);
                        }
                    }
                }

                //attachments
                if (this.MailContext.Attachments != null)
                {
                    foreach(KeyValuePair<string, Stream> entry in this.MailContext.Attachments)
                    {
                        this.MailMessage.Attachments.Add(new Attachment(entry.Value, entry.Key));
                    }
                }

                //send email
                SmtpClient client = new SmtpClient(this.MailContext.Host);
                client.EnableSsl = this.MailContext.EnableSsl;
                client.Port = this.MailContext.Port;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = this.MailContext.UseDefaultCredentials;
                client.Credentials = new NetworkCredential(this.MailContext.UserName, this.MailContext.Password);
                client.Send(this.MailMessage);
            } 
            catch (Exception ex) {
                throw ex;
            }
        }

        #endregion
    }
}
