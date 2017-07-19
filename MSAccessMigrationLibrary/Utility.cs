using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSAccessMigrationLibrary
{
    public static class Utility
    {
      static  List<GVAccessAnalysis> _gvAccessAnalysis = new List<GVAccessAnalysis>();

        public static List<GVAccessAnalysis> GVAccessAnalysisInfo { get { return _gvAccessAnalysis; } }
        static List<Exception> exceptions = new List<Exception>();  
        public static bool IsSqlMigrationSelected { get; set; }

        public static List<Exception> ExceptionList { get { return exceptions; }  }
        public static string FormatAnalysis(DBEngineObject dbEngineObject)
        {
            StringBuilder formattedString = new StringBuilder();
            _gvAccessAnalysis.Clear();
          var propertyInfos =  dbEngineObject.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            formattedString.AppendLine();
            foreach (var property in propertyInfos)
            {
                
               var propertyValue =  property.GetValue(dbEngineObject);
                formattedString.Append(property.Name);
                if (property.Name == "Tables")
                {
                    if (propertyValue != null)
                    {
                        var tables = propertyValue as List<TableInfo>;
                        formattedString.Append("    "+tables.Count);
                        foreach (var table in tables)
                        {
                            var gvAccessAnalysis = new GVAccessAnalysis()
                            {
                                AccessObject = table.TableName,
                                ObjectType = property.Name,
                                Remarks = table.TableType,
                                IsMigrated = false
                            };

                            _gvAccessAnalysis.Add(gvAccessAnalysis);
                            AppLogManager.LogInfo(gvAccessAnalysis.ToString());
                            
                        }

                    }
                }
                else {
                    if (propertyValue != null)
                    {
                        var items = propertyValue as List<string>;
                        formattedString.Append("    " + items.Count);
                        foreach (var item in items)
                        {
                            var gvAccessAnalysis = new GVAccessAnalysis() { AccessObject = item, ObjectType = property.Name, Remarks = "-", IsMigrated = false };

                            _gvAccessAnalysis.Add(gvAccessAnalysis);
                            AppLogManager.LogInfo(gvAccessAnalysis.ToString());
                           
                        }

                    }
                }
                formattedString.AppendLine();
            }
            return formattedString.ToString();
        }

        public static void Refresh()
        {
            _gvAccessAnalysis.Clear();
            exceptions.Clear();
        }
        public static string GetFormattedException()
        {
            string strexpt = string.Empty;
            foreach (var excpt in exceptions)
            {
                strexpt += string.Format("{0} {1} {0} {2}", Environment.NewLine, excpt.Message, excpt.InnerException);
            }
            return strexpt;
        }

        public static object DeepCopy(object obj)
        {
            if (obj == null)
                return null;
            Type type = obj.GetType();

            if (type.IsValueType || type == typeof(string))
            {
                return obj;
            }
            else if (type.IsArray)
            {
                Type elementType = Type.GetType(
                     type.FullName.Replace("[]", string.Empty));
                var array = obj as Array;
                Array copied = Array.CreateInstance(elementType, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    copied.SetValue(DeepCopy(array.GetValue(i)), i);
                }
                return Convert.ChangeType(copied, obj.GetType());
            }
            else if (type.IsClass)
            {

                object toret = Activator.CreateInstance(obj.GetType());
                FieldInfo[] fields = type.GetFields(BindingFlags.Public |
                            BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    object fieldValue = field.GetValue(obj);
                    if (fieldValue == null)
                        continue;
                    field.SetValue(toret, DeepCopy(fieldValue));
                }
                return toret;
            }
            else
                throw new ArgumentException("Unknown type");
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

   
    public class GVAccessAnalysis
    {
        public string AccessObject { get; set; }

        public string ObjectType { get; set; }

        public string Remarks { get; set; }

        public bool IsMigrated { get; set; }

        public override string ToString()
        {
           return string.Format("Name : {0} \t Type: {1} \t Remarks: {2}", AccessObject, ObjectType, Remarks);
        }
    }

   
}
