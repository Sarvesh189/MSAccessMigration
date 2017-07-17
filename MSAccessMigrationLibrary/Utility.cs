using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAccessMigrationLibrary
{
    public static class Utility
    {
        static List<Exception> exceptions = new List<Exception>();  
        public static bool IsSqlMigrationSelected { get; set; }

        public static List<Exception> ExceptionList { get { return exceptions; }  }
        public static string FormatAnalysis(DBEngineObject dbEngineObject)
        {
            StringBuilder formattedString = new StringBuilder();
          var propertyInfos =  dbEngineObject.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var property in propertyInfos)
            {
              var propertyValue =  property.GetValue(dbEngineObject);
                if(propertyValue !=null)
                    formattedString.Append(propertyValue.ToString());
            }
            return formattedString.ToString();
        }
    }

    public class TableInfo
    {
        public TableInfo(string tableName, string tableType)
        {
            TableName = tableName;
            TableType = tableType;
        }
        public string TableName { get; set; }
        public string TableType { get; set; }
    }

   
}
