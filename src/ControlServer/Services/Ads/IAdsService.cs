using System;
using System.Threading.Tasks;

namespace ControlServer.Services.Ads
{
    public interface IAdsService
    {
        Task<bool> ReadBoolAsync(int indexGroup, int indexOffset);

        Task WriteBoolAsync(int indexGroup, int indexOffset, bool value);

        Task<bool> ReadWriteBoolAsync(int indexGroup, int indexOffset, bool value);
    }
}
