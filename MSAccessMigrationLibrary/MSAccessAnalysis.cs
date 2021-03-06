﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Access = Microsoft.Office.Interop.Access;
using Microsoft.Office.Interop.Access.Dao;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;


namespace MSAccessMigrationLibrary
{
    public class MSAccessAnalysis : IMSAccessAnalysis
    {
        public List<string> GetFormNames(string accessDBfileName)
        {

            var frmNames = new List<string>();
            try
            {
                Access.Application _application = new Access.Application();

                _application.OpenCurrentDatabase(accessDBfileName, true, "");

                Access.AllObjects forms = _application.CurrentProject.AllForms;

                foreach (var frm in forms)
                {
                    try
                    {
                        var typedesc = TypeDescriptor.GetProperties(frm).Find("Name", true);

                        frmNames.Add(typedesc.GetValue(frm).ToString());
                    }
                    catch (Exception ex)
                    {
                        Utility.ExceptionList.Add(ex);

                    }

                }

                _application.CloseCurrentDatabase();
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }
            return frmNames;
        }

        public List<string> GetQueries(string accessDBfileName)
        {
            Database _db = null;
            var queries = new List<string>();
            try
            {
                DBEngine _dbEngine = new DBEngine();
                 _db = _dbEngine.OpenDatabase(accessDBfileName, false, false, "");
                var _queries = _db.QueryDefs;

                for (int index = 0; index < _queries.Count; index++)
                {
                    var _query = _queries[index];
                    if(!_query.Name.StartsWith("~"))
                    queries.Add(_query.Name);
                }

               
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }
            finally
            {
                if (_db != null)
                    _db.Close();
            }
            return queries;
        }

        public List<string> GetReport(string accessDBfileName)
        {
            Access.Application _application = null;
            var reportsName = new List<string>();
            try
            {


                _application = new Access.Application();

                _application.OpenCurrentDatabase(accessDBfileName, true, "");

                Access.AllObjects reports = _application.CurrentProject.AllReports;

                foreach (var rpt in reports)
                {

                    var typedesc = TypeDescriptor.GetProperties(rpt).Find("Name", true);

                    reportsName.Add(typedesc.GetValue(rpt).ToString());
                }

                _application.CloseCurrentDatabase();


            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }
        
            return reportsName;

        }

        public List<string> GetMacros(string accessDBfileName)
        {
            Access.Application _application = null;
            var macrosname = new List<string>();
            try
            {
                _application = new Access.Application();

                _application.OpenCurrentDatabase(accessDBfileName, true, "");

                Access.AllObjects macros = _application.CurrentProject.AllMacros;

                foreach (var rpt in macros)
                {
                    var typedesc = TypeDescriptor.GetProperties(rpt).Find("Name", true);

                    macrosname.Add(typedesc.GetValue(rpt).ToString());
                }

                _application.CloseCurrentDatabase();
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }

            return macrosname;
        }

        public List<TableInfo> GetTablesName(string accessDBfileName)
        {
            var tables = new List<TableInfo>();
            Database _db = null;
            DBEngine _dbEngine = new DBEngine();
            try
            {
                _db = _dbEngine.OpenDatabase(accessDBfileName, false, false, "");

                var debugtables = new List<string>();

                for (int i = 0; i < _db.TableDefs.Count; i++)
                {
                    var tbdl = _db.TableDefs[i];
                    if (tbdl.Attributes != 2 && !tbdl.Name.StartsWith("MSys"))
                    {
                        if (!string.IsNullOrEmpty(tbdl.Connect))
                            tables.Add(new TableInfo(tbdl.Name, "External"));
                        else
                            tables.Add(new TableInfo(tbdl.Name, "Internal"));
                    }
                    //non system table
                    // debugtables.Add(tbdl.Name);
                }


                //     Recordset _rs = _db.ListTables();

                //_rs.MoveFirst();
                ////   string str = "";
                //while (!_rs.EOF)
                //{
                //    for (int i = 0; i < _rs.Fields.Count; i++)
                //        debugtables.Add(_rs.Fields[i].Name + ' ' + _rs.Fields[i].Value);




                //    if (_rs.Fields["TableType"].Value == 1 && _rs.Fields["Attributes"].Value == 0)
                //        tables.Add(new TableInfo(_rs.Fields[0].Value,"Internal"));
                //    if (_rs.Fields["TableType"].Value == 4 && _rs.Fields["Attributes"].Value == 0)
                //        tables.Add(new TableInfo(_rs.Fields[0].Value, "External"));
                //    _rs.MoveNext();
                //}
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }
            finally
            {
                if(_db != null)
                _db.Close();
            }
            return tables;
        }

        public List<string> GetModules(string accessDBfileName)
        {
            Access.Application _application = null;
            var _modules = new List<string>();
            try
            {
                _application = new Access.Application();

                _application.OpenCurrentDatabase(accessDBfileName, true, "");

                Access.AllObjects modules = _application.CurrentProject.AllModules;

                foreach (var mdl in modules)
                {
                    var typedesc = TypeDescriptor.GetProperties(mdl).Find("Name", true);

                    _modules.Add(typedesc.GetValue(mdl).ToString());
                }

                _application.CloseCurrentDatabase();
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }

            return _modules;
        }

        public List<AccessTableRelationInfo> GetRelations(string accessDBfileName)
        {
            var relations = new List<AccessTableRelationInfo>();
            Database _db = null;
            DBEngine _dbEngine = new DBEngine();
            try
            {
                _db = _dbEngine.OpenDatabase(accessDBfileName, false, false, "");

                var debugtables = new List<string>();
              
                for (int i = 0; i < _db.Relations.Count; i++)
                {
                    if (!_db.Relations[i].Name.StartsWith("MSys"))
                    {
                        var tblr = new AccessTableRelationInfo() { RelationName = _db.Relations[i].Name, Table = _db.Relations[i].Table, ForeignTable = _db.Relations[i].ForeignTable };


                        foreach (var field in _db.Relations[i].Fields)
                        {

                            tblr.FieldsInfo.Add(new AccessFieldInfo() { Name = (field as Field).Name, ForeignName = (field as Field).ForeignName });
                        }
                        relations.Add(tblr);
                    }
                }             
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }
            finally
            {
                if (_db != null)
                    _db.Close();
            }
            return relations;
        }

        public List<AccessTableIndexInfo> GetIndexes(string accessDBfileName)
        {
            var indexes = new List<AccessTableIndexInfo>();
            Database _db = null;
            DBEngine _dbEngine = new DBEngine();
            try
            {
                _db = _dbEngine.OpenDatabase(accessDBfileName, false, false, "");

                var debugtables = new List<string>();

                for (int i = 0; i < _db.TableDefs.Count; i++)
                {
                    if (!_db.TableDefs[i].Name.StartsWith("MSys"))
                    {
                        for (int j = 0; j < _db.TableDefs[i].Indexes.Count; j++)
                        {

                            var tblIndex = new AccessTableIndexInfo() { Table = _db.TableDefs[i].Name, Name = _db.TableDefs[i].Indexes[j].Name, IsClustered = _db.TableDefs[i].Indexes[j].Clustered, IsForeignKey = _db.TableDefs[i].Indexes[j].Foreign, IsPrimaryKey = _db.TableDefs[i].Indexes[j].Primary, IsRequired = _db.TableDefs[i].Indexes[j].Required, IsUniqueKey = _db.TableDefs[i].Indexes[j].Unique };
                          //  var fields = _db.TableDefs[i].Indexes[j].Fields as Fields;
                            for (int k = 0; k < _db.TableDefs[i].Indexes[j].Fields.Count; k++)
                            {
                             //   var fld = _db.TableDefs[i].Indexes[j].Fields[k];
                                tblIndex.Fields.Add(new AccessFieldInfo() { Name = _db.TableDefs[i].Indexes[j].Fields[k].Name });
                            }



                            indexes.Add(tblIndex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }
            finally
            {
                if (_db != null)
                    _db.Close();
            }
            return indexes;
        }
    }
}
