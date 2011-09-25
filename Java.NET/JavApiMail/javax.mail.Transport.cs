/*
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at 
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *  
 *  Copyright ?2011 Sebastian Ritter
 */
using System;
using System.Net.Mail;
using System.Net;

using javax = biz.ritter.javapix;

namespace biz.ritter.javapix.mail
{
    public class Transport
    {
        public Transport ()
        {
        }
        
        public static void send (Message msg)
        {
            try
            {
                NetworkCredential credential = null;
                if (null != msg.session.getProperty("mail.smtp.user"))
                {
                    credential = new NetworkCredential(msg.session.getProperty("mail.smtp.user"), msg.session.getProperty("mail.smtp.password"));
                }
                String port = msg.session.getProperty("mail.smtp.port") != null ? msg.session.getProperty("mail.smtp.port") : "25";
                SmtpClient smtp = new SmtpClient(msg.session.getProperty("mail.smtp.host"), int.Parse(port));
                smtp.Credentials = credential;
                MailMessage mail = new MailMessage();
                MailAddress mailFrom = new MailAddress(msg.getFrom()[0].toString());
                mail.From = mailFrom;
                for (int i = 0; i < msg.getRecipients(Message.RecipientType.TO).Length; i++)
                {
                    MailAddress mailAddress = new MailAddress(msg.getRecipients(Message.RecipientType.TO)[i].ToString());
                    mail.To.Insert(i, mailAddress);
                }
                mail.Subject = msg.getSubject();
                if ("text/plain".Equals(msg.getContentType()))
                {
                    mail.Body = msg.getContent().ToString();
                }
                else
                {
                    throw new NotImplementedException();
                }
                mail.IsBodyHtml = ("text/html".Equals(msg.getContentType()));
                smtp.Send(mail);
            }
            catch (SmtpException se)
            {
                switch (se.StatusCode) {
                    case SmtpStatusCode.GeneralFailure | SmtpStatusCode.MustIssueStartTlsFirst :
                        throw new javax.mail.SendFailedException (se.Message);
                    default :
                        throw se;
                }
            }
        }
        
    }
}

