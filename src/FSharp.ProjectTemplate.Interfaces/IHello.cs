using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSharp.ProjectTemplate.Domain;

namespace FSharp.ProjectTemplate.Interfaces
{
    public interface IHello : Orleans.IGrainWithIntegerKey
    {
        Task<string> SayHello(Person person);
    }
}
