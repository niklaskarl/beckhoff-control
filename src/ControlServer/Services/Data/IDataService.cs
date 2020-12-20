using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

using ControlServer.Data;

namespace ControlServer.Services.Data
{
    public interface IDataService
    {
        ImmutableArray<Light> Lights { get; }
    }
}
