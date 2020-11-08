using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.Common.Interfaces
{
    public interface IIdGenerator
    {
        Task<string> BasicIdGenerator(IDateTime dateTime, string configureValue = "TestApp");
    }
}
