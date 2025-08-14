using FluentEmail.Core;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.Ulity
{
    public class EmailSender : IEmailSender
    {
        private readonly IFluentEmail _email;

        public EmailSender(IFluentEmail email)
        {
            _email = email;
        }

        public async Task SendEmailAsync(string to, string subject,string body)
        {
            await _email
          .To(to)
          .Subject(subject)
          .Body(body, isHtml: true)
          .SendAsync();
        }
    }
}
