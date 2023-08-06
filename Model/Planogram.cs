using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Testing.DB;
using Testing.Model.Abstracts;

#nullable enable
namespace MyPlanogram.Model
{
    public class Planogram : AbstractModel, IDB<Planogram>
    {
        #region backprop
        Int64? _planoid;
        Item _item=null!;
        Shelf? _shelf=new();
        int? _faces;
        bool? _substitute;
        int? _orderlist;
        #endregion

        #region Props

        public string GetFaces
        {
            get=>(PlanoID == null) ? "Faces: N/A" :$"Faces: {Faces}";
        }

        public string GetSubstitute
        {
            get 
               => (PlanoID == null) ? string.Empty
                : ((bool)Substitute) ? "(Substitute)" : "";
        }

        public int ShowSubstitute
        {
            get
               => (PlanoID == null) ? 0
                : ((bool)Substitute) ? 30 : 0;
        }

        public Int64? PlanoID
        {
            get => _planoid;
            set => Set<Int64?>(ref value, ref _planoid, nameof(PlanoID));
        }

        public Item Item 
        {
            get => _item;
            set => Set<Item>(ref value, ref _item,nameof(Item));
        }

        public Shelf? Shelf
        {
            get => _shelf;
            set => Set<Shelf?>(ref value, ref _shelf, nameof(Shelf));
        }

        public int? Faces
        {
            get => _faces;
            set => Set<int?>(ref value, ref _faces, nameof(Faces));
        }

        public bool? Substitute
        {
            get=> _substitute;
            set => Set<bool?>(ref value, ref _substitute,nameof(Substitute));
        }

        public int? Orderlist
        {
            get => _orderlist;
            set => Set<int?>(ref value, ref _orderlist,nameof(Orderlist));
        }

        public int ShowButton
        {
            get => (PlanoID) == null ? 0 : 55;
        }

        public int ShowLocationInfo
        {
            get => (PlanoID) == null ? 0 : 30;
        }
        #endregion

        #region Constructors
        public Planogram() { }

        public Planogram(Item item) 
        {
            Item = item;
            Shelf = new();
        }

        public Planogram(MySqlDataReader reader) 
        {
            PlanoID = reader.GetInt64(0);
            Item = new(reader.GetInt64(1));
            Shelf = new(reader.GetInt64(2));
            Faces = reader.GetInt32(3);
            Substitute = reader.GetBoolean(4);
            Orderlist = reader.GetInt32(6);

            //Item = new(reader);

            //if (!reader.IsDBNull(11))
            //{
            //    PlanoID = reader.GetInt64(11);
            //}

            //if (!reader.IsDBNull(12))
            //{
            //    Shelf = new(reader.GetInt64(12));
            //}

            //if (!reader.IsDBNull(13))
            //{
            //    Faces = reader.GetInt32(13);
            //}

            //if (!reader.IsDBNull(14))
            //{
            //    Substitute = reader.GetBoolean(14);
            //}

            //if (!reader.IsDBNull(15))
            //{
            //    Orderlist = reader.GetInt32(15);
            //}
        }
        #endregion

        #region AbstractModel
        public override bool IsNewRecord => PlanoID==0;

        public override void SetForeignKeys()
        {
            Item = MainDB.DBItem?.RecordSource?.FirstOrDefault(s=>s?.ItemID==Item?.ItemID);
            Shelf = MainDB.DBShelf.RecordSource.FirstOrDefault(s=>s.ShelfID==Shelf.ShelfID);
            Shelf.Bay = MainDB.DBBay.RecordSource.FirstOrDefault(s => s.BayID==Shelf.Bay.BayID);
            Shelf.Bay.Section = MainDB.DBSection.RecordSource.FirstOrDefault(s=>s.SectionID==Shelf.Bay.Section.SectionID);            
        }
        #endregion

        #region EqualsString
        public override bool Equals(object? obj)=>
        obj is Planogram planogram &&
        PlanoID == planogram.PlanoID;
        public override int GetHashCode()=>HashCode.Combine(PlanoID);
        public override string ToString() => $"{PlanoID}";
        #endregion

        #region IDB
        public void SetPrimaryKey(ulong ID)
        {
        }

        public string SQLQuery(QueryType Query)
        {
            return Query switch
            {
                QueryType.SELECT => "SELECT * FROM Planogram;",
                QueryType.DELETE => string.Empty,
                QueryType.UPDATE => string.Empty,
                QueryType.INSERT => string.Empty,
                _ => string.Empty,
            };
        }

        public Planogram GetRecord(MySqlDataReader reader) => new(reader);

        public void Params(MySqlParameterCollection param)
        {

        }

        #endregion
    }
}
