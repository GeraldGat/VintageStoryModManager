using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services.Interfaces
{
    public interface IVintageStoryApiService
    {
        public Task<IEnumerable<VersionInfos>> GetVersionsAsync();
    }
}
