using EGameCafe.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.Common.Interfaces
{
    public interface IEmailSender
    {
        Task<Result> SendEmailAsync(string email, string subject, string message);
    }
}
