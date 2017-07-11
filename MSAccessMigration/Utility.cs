using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAccessMigration
{
  public static  class Utility
    {
        public static bool  IsSqlMigrationSelected{ get; set; }
    }

    public class TableInfo
    {
        public TableInfo(string tableName,string tableType) {
            TableName = tableName;
            TableType = tableType;
        }
        public string TableName { get; set; }
        public string TableType { get; set; }
    }
}
