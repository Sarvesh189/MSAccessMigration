using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAccessMigration
{
  public interface IMSAccessAnalysis
    {

        List<string> GetFormNames(string accessDBfileName);
        List<TableInfo> GetTablesName(string accessDBfileName);
        List<string> GetReport(string accessDBfileName);
        List<string> GetQueries(string accessDBfileName);

        List<string> GetMacros(string accessDBfileName);
    }
}
 