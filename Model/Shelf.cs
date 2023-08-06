using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing.DB;
using Testing.Model.Abstracts;

#nullable enable
namespace MyPlanogram.Model
{
    public class Shelf : AbstractModel, IDB<Shelf>
    {
        #region backprops
        Int64? _shelfid;
        int? _notch;
        int? _shelfnum;
        bool? _onhook;
        Bay? _bay;
        #endregion

        #region Props
        public Int64? ShelfID 
        {
            get => _shelfid;
            set => Set<Int64?>(ref value, ref _shelfid,nameof(ShelfID));
        }

        public int? Notch 
        { 
            get=>_notch;
            set => Set<int?>(ref value, ref _notch,nameof(Notch));
        }

        public int? ShelfNum 
        {
            get => _shelfnum;
            set => Set<int?>(ref value, ref _shelfnum,nameof(ShelfNum));
        }

        public bool? OnHook 
        {
            get => _onhook;
            set => Set<bool?>(ref value, ref _onhook,nameof(OnHook));
        }

        public Bay? Bay 
        {
            get => _bay;
            set => Set<Bay?>(ref value, ref _bay,nameof(Bay));
        }
        #endregion

        #region Constructors
        public Shelf()
        {
            Bay = new();
        }
        
        public string? ShelfAndNotch=>
        (bool)(OnHook) ? "ON HOOKS" : $"SHELF: {ShelfNum} - NOTCH: {Notch}";

        public string? NotchOrHook => (bool)OnHook ? string.Empty : $"NOTCH: {Notch}"; 

        public Shelf(Int64 ShelfID)=>this.ShelfID= ShelfID;

        public Shelf(MySqlDataReader reader) 
        {
            ShelfID = reader.GetInt64(0);
            Notch = reader.GetInt32(1);
            ShelfNum = reader.GetInt32(2);
            OnHook = reader.GetBoolean(3);
            Bay = new(reader.GetInt64(4));
        } 
        #endregion

        #region AbstractModel
        public override bool IsNewRecord => ShelfID==0;
        public override void SetForeignKeys()
        {
        }
        #endregion

        #region EqualsToString
        public override bool Equals(object? obj)=>
        obj is Shelf shelf &&
        ShelfID == shelf.ShelfID;

        public override int GetHashCode()=>
        HashCode.Combine(ShelfID);

        public override string? ToString() 
        {

            if (ShelfID == null) return $"SHELF: N/A";

            if ((bool)OnHook) return "ON HOOKS";
            return $"SHELF: {ShelfNum}";
        } 
        #endregion

        #region IDB
        public string SQLQuery(QueryType Query)
        {
            switch (Query)
            {
                case QueryType.SELECT:
                    return "SELECT * FROM Shelf;";
                case QueryType.DELETE:
                    return string.Empty;
                case QueryType.UPDATE:
                    return string.Empty;
                case QueryType.INSERT:
                    return string.Empty;
                default: return string.Empty;
            }
        }

        public Shelf GetRecord(MySqlDataReader reader) => new(reader);

        public void Params(MySqlParameterCollection param)
        {
        }

        public void SetPrimaryKey(ulong ID)
        {
        }
        #endregion
    }
}
