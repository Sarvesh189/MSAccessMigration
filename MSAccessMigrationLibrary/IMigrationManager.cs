using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAccessMigrationLibrary
{
  
        public interface IMigrationManager
        {
            DBEngineObject AnalyseAccessDB(string accessDBfileName);
            bool TransferAccessDB(string sourceDBFile, string destinationDBFile, List<string> tables, IProgress<int> progress);

        }
    }

