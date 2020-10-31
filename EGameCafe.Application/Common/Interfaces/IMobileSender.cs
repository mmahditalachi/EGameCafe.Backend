using EGameCafe.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.Common.Interfaces
{
    public interface IMobileSenders
    {
        Task<Result> SendOTP(string phoneNumber,string email, string token);
    }
}
