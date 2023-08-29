using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve.Core.Features.MailService;

public interface IEmailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}
