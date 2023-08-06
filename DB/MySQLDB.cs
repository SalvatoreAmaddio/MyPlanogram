using MySqlConnector;
using Testing.Model.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Testing.RecordSource;
#nullable enable

namespace Testing.DB
{

    public class MySQLDB<M> where M : AbstractModel, IDB<M>, new() {
        private MySqlConnection Connection;
        private MySqlCommand Command = null!;
        public M Record { get; set; } = new();
        public RecordSource<M> RecordSource { get; set; } = new();
        public bool IsConnected { get; private set; }
        private MySqlTransaction? Transaction;

        public MySQLDB()=>
        Connection = new MySqlConnection(MainDB.ConnectionString);

        public void OpenConnection()
        {
            Connection.Open();
            IsConnected = true;
        }

        public void CloseConnection()
        {
            Connection.Close();
            IsConnected = false;
        }


        //   #region SelectStatement
        //public RecordSource<M> Select(string sql)
        //{
        //    RecordSource<M> res = new();
        //    Command = new(sql, Connection);
        //    var result = Command.ExecuteReader();
        //    while (result.Read())
        //    {
        //        res.InsertRecord(Record.GetRecord(result));
        //    }
        //    return res;
        //}

        public void Select()
        {
            RecordSource.Clear();
            Command = new(Record.SQLQuery(QueryType.SELECT), Connection);
            var result = Command.ExecuteReader();
            while (result.Read())
            {
                RecordSource.Add(Record.GetRecord(result));
            }
        }
        //#endregion

        //Command = new("PRAGMA foreign_keys = ON;", Connection);
        //Command.ExecuteNonQuery();

        public bool IsNewRecord() => Record.IsNewRecord;

        public void RunStatement(string sql)
        {
            Command = new(sql, Connection);
            Command.ExecuteNonQuery();
        }

        private UInt64 LastInsertedID()
        {
            Command = new("SELECT LAST_INSERT_ID();", Connection);
            return (UInt64)Command.ExecuteScalar();
        }

        private bool RecordIsSet(M? record) {
            if (record != null) Record = record;
            return (Record != null);
        }
    }

}
