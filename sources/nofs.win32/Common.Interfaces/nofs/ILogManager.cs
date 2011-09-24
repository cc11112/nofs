using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nofs.Net.Common.Interfaces.nofs
{
    interface ILogManager
    {
        public void LogDebug(String msg);

        public void LogError(String msg);

        public void LogInfo(String msg);
    }
}
