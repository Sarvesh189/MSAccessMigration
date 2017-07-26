using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAccessMigrationLibrary
{
    public interface IMSAccessTransfer
    {
        DBEngineObject DBEngineObject { get; set; }
        List<string> TransferInternalTables(string sourceDBFile, string destinationDBFile, IProgress<int> progress);

        void TransferForm(string sourceDBFile, string destinationDBFile, IProgress<int> progress);

        void TransferReport(string sourceDBFile, string destinationDBFile, IProgress<int> progress);

        List<string> TransferObjectToSQL(List<string> tables, string sourceAccessfile);
        void TransferQueries(string sourceDBFile, string destinationDBFile, IProgress<int> progress);
        void TransferModules(string sourceDBFile, string destinationDBFile, IProgress<int> progress);

        void TransferMacros(string sourceDBFile, string destinationDBFile, IProgress<int> progress);
        void EstablishRelations(string sourceDBFile, string destinationDBFile, IProgress<int> progress);

        void EstablishRelationsInSql(string sourceDBFile, string destinationDBFile, IProgress<int> progress);

        void CreateIndexes(string destinationDBFile, IProgress<int> progress);

        void CreateSqlIndexes(string destinationDBFile);
         event EventHandler<ItemTransferEventArg> ItemTransferred;
    }
}
