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
using System.Collections.Generic;

using javax = biz.ritter.javapix;

namespace biz.ritter.javapix.mail.internet
{
	public class MimeMessage : javax.mail.Message, javax.mail.MimePart
	{

		private Address [] to;
		private String subject;
		private String textContent;
		private readonly String textContentType = "text/plain";
		
		public MimeMessage (javax.mail.Session session) : base(session)
		{
		}
		
		public override void setRecipients (RecipientType type, Address[] addresses)
		{
			if (type.Equals (Message.RecipientType.TO)) {
				this.to = addresses;
			} 
			else {
				throw new NotImplementedException ();
			}
		}
		public override Address[] getRecipients (RecipientType type)
		{
			if (type.Equals (Message.RecipientType.TO)) {
				return this.to;
			}
			else {
				throw new NotImplementedException ();
			}
		}
		public override void setSubject (string subject)
		{
			this.subject = subject;
		}
		public override string getSubject ()
		{
			return this.subject;
		}
		public override void setContent (Object obj, String type)
		{
			if (!(obj is String) || obj == null) throw new NotImplementedException ();
			this.textContent = obj.ToString();
		}
		public override object getContent ()
		{
			return this.textContent;
		}
		public override string getContentType ()
		{
			return this.textContentType;
		}
		public override Address[] getFrom ()
		{
			return new javax.mail.Address[] { this.from_ };
		}
	}
}

