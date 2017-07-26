//using Microsoft.Office.Interop.Access.Dao;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Access = Microsoft.Office.Interop.Access
//namespace TestingMSAccessLibrary
//{
//   public class TableInformation
//    {
//        Access.Application _application = null;
//        List<TableInformation> _tableInformation = null;
//        public TableInformation()
//        {
//             _application = new Access.Application();
//            _tableInformation = new List<TableInformation>();
//        }
//        public string Table { get; set; }
//        public string Fields { get; set; }
//        public string FieldType { get; set; }

//        public int FieldSize { get; set; }
       
//        void GetFieldInformation()
//        {
         
//            Database db;
//            TableDef td;
//            Recordset rs;
//            Recordset rs2;
//            string Test;
//            string NameHold;
//            string typehold;
//            string SizeHold;
//            string fielddescription;
//            string tName;
//            long n;
//            long i;
//            Field fld;
//            string strSQL;
//            n = 0;
//            db = _application.CurrentDb();
//            //  Trap for any errors.
//            // TODO: On Error Resume Next Warning!!!: The statement is not translatable 
           
       
//            db.Execute("CREATE TABLE tblFields(Object TEXT (55), FieldName TEXT (55), FieldType TEXT (20), FieldSize Long, Fi" +
//            "eldAttributes Long, FldDescription TEXT (20));";
//            strSQL = "SELECT MSysObjects.Name, MSysObjects.Type From MsysObjects WHERE";
//            strSQL = (strSQL + "((MSysObjects.Type)=1)");
//            strSQL = (strSQL + "ORDER BY MSysObjects.Name;");
//            rs = db.OpenRecordset(strSQL);
//            if (!rs.BOF)
//            {
//                //  Get number of records in recordset
//                rs.MoveLast;
//                n = rs.RecordCount;
//                rs.MoveFirst;
//            }

//            rs2 = db.OpenRecordset("tblFields");
//            for (i = 0; (i <= (n - 1)); i++)
//            {
//                fielddescription = " ";
//                td = db.TableDefs(i);
//                // Skip over any MSys objects
//                if (((Left(rs, Name, 4) != "MSys")
//    && (Left(rs, Name, 1) != "~")))
//                {
//                    NameHold = rs;
//                    Name;
//                    // TODO: On Error Resume Next Warning!!!: The statement is not translatable 
//                    foreach (fldin td.Fields)
//                    {
//                        fielddescription = fld.Name;
//                        typehold = FieldType(fld.Type);
//                        SizeHold = fld.Size;
//                        rs2.AddNew;
//                        rs2;
//                        NameHold;
//                        rs2;
//                        FieldName = fielddescription;
//                        rs2;
//                        FieldType = typehold;
//                        rs2;
//                        FieldSize = SizeHold;
//                        rs2;
//                        FieldAttributes = fld.Attributes;
//                        rs2;
//                        FldDescription = fld.Properties["description"];
//                        rs2.Update;
//                    }

//                }

//                rs.MoveNext;
//            }

//            rs.Close;
//            rs2.Close;
//            db.Close;
//        }

//        string FieldType(int intType)
//        {
//            switch (intType)
//            {
//                case dbBoolean:
//                    FieldType = "dbBoolean";
//                    break;
//                case dbByte:
//                    FieldType = "dbByte";
//                    break;
//                case dbInteger:
//                    FieldType = "dbInteger";
//                    break;
//                case dbLong:
//                    FieldType = "dbLong";
//                    break;
//                case dbCurrency:
//                    FieldType = "dbCurrency";
//                    break;
//                case dbSingle:
//                    FieldType = "dbSingle";
//                    break;
//                case dbDouble:
//                    FieldType = "dbDouble";
//                    break;
//                case dbDate:
//                    FieldType = "dbDate";
//                    break;
//                case dbBinary:
//                    FieldType = "dbBinary";
//                    break;
//                case dbText:
//                    FieldType = "dbText";
//                    break;
//                case dbLongBinary:
//                    FieldType = "dbLongBinary";
//                    break;
//                case dbMemo:
//                    FieldType = "dbMemo";
//                    break;
//                case dbGUID:
//                    FieldType = "dbGUID";
//                    break;
//            }
//        }
//    }
//}
