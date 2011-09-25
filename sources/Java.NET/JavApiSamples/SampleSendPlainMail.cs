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
 *  Copyright © 2011 Sebastian Ritter
 */
using System;

using java = biz.ritter.javapi;
using javax = biz.ritter.javapix;

namespace javapix.sample.mail
{
    public class SampleSendPlainMail
    {
        static void Main ()
        {
            try
            {

                String recipient = "Hello.Developer@example.com";
                String subject = "Hello";
                String content = "Hello JavApi user!";
                String from1 = "JavApi.Mail@example.com";

                java.util.Properties props = new java.util.Properties();
                props.put("mail.smtp.host", "post.example.com");
                props.put("mail.smtp.port", "25");
                props.put("mail.smtp.auth", "true");
                props.put("mail.smtp.user", "sebastian.ritter@example.com");
                props.put("mail.smtp.password", "secret");
                javax.mail.Session session = javax.mail.Session.getDefaultInstance(props);
                javax.mail.Message msg = new javax.mail.internet.MimeMessage(session);
                javax.mail.internet.InternetAddress addressFrom = new javax.mail.internet.InternetAddress(from1);
                msg.setFrom(addressFrom);
                javax.mail.internet.InternetAddress addressTo = new javax.mail.internet.InternetAddress(recipient);
                msg.setRecipient(javax.mail.Message.RecipientType.TO, addressTo);
                msg.setSubject(subject);
                msg.setContent(content, "text/plain");
                javax.mail.Transport.send(msg);
            }
            catch (javax.mail.SendFailedException/*sfe*/)
            {
                
            }
            catch (javax.mail.MessagingException/*me*/)
            {
            }
            
        }
    }
}

