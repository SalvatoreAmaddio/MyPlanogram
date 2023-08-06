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
    public class Item : AbstractModel, IDB<Item>
    {
        #region backprops
        Int64 _itemid;
        string _sku;
        string _itemname;
        double _price;
        string _pictureurl;
        Offer _offer;
        Department _department;
        bool _isbs;
        bool _asra;
        bool _stop;
        bool _scheduleforchange;
        #endregion

        #region Props
        public Int64 ItemID { get => _itemid; set => Set<Int64>(ref value, ref _itemid,nameof(ItemID)); }
        public string SKU { get => _sku; set => Set<string>(ref value, ref _sku, nameof(SKU)); }
        public string ItemName { get => _itemname; set => Set<string>(ref value, ref _itemname, nameof(ItemName)); }
        public double Price { get => _price; set => Set<double>(ref value, ref _price, nameof(Price)); }
        public string PictureURL { get => _pictureurl; set => Set<string>(ref value, ref _pictureurl, nameof(PictureURL)); }
        public Offer Offer { get => _offer; set => Set<Offer>(ref value, ref _offer, nameof(Offer)); }
        public Department Department { get => _department; set => Set<Department>(ref value, ref _department, nameof(Department)); }
        public bool IsBs { get => _isbs; set => Set<bool>(ref value, ref _isbs,nameof(IsBs)); }
        public bool ASRA { get => _asra; set => Set<bool>(ref value, ref _asra, nameof(ASRA)); }
        public bool ScheduleForChange { get => _scheduleforchange; set => Set<bool>(ref value, ref _scheduleforchange, nameof(ScheduleForChange)); }
        public bool Stop { get => _stop; set => Set<bool>(ref value, ref _stop, nameof(Stop)); }
        public int ShowOffer { get => (Offer.OfferID == 1) ? 0 : 30; }
        
        public int GridHeight
        {
            get => (Offer.OfferID == 1) ? 265 : 295;
        }
        
        public int ShowAdditionalInfo 
        { 
            get
            {

                var val = (ASRA) || (IsBs) || (ScheduleForChange) || (Stop);
                return (val) ? 35 : 0;
            }
        }
        #endregion

        #region constructors
        public Item()
        {

        }

        public Item(Int64 ItemID)=>this.ItemID=ItemID;

        public Item(MySqlDataReader reader)
        {
            ItemID = reader.GetInt64(0);
            SKU=reader.GetInt32(1).ToString();
            ItemName = reader.GetString(2);
            Price = reader.GetDouble(3);
            PictureURL = reader.GetString(4);
            Offer = new(reader.GetInt64(5));
            IsBs = reader.GetBoolean(6);

            ASRA = reader.GetBoolean(7);
            ScheduleForChange = reader.GetBoolean(8);
            Stop = reader.GetBoolean(9);
            Department = new(reader.GetInt32(10));
        }
        #endregion

        #region AbstractModel
        public override bool IsNewRecord => ItemID==0;

        public override void SetForeignKeys()
        {
            Offer = MainDB.DBOffer.RecordSource.FirstOrDefault(s => s.OfferID==Offer.OfferID);
            Department = MainDB.DBDepartment.RecordSource.FirstOrDefault(s=>s.DepartmentID==Department.DepartmentID);
        }
        #endregion

        #region EqualsToString
        public override bool Equals(object obj)=>obj is Item item && SKU == item.SKU;
        public override int GetHashCode()=>HashCode.Combine(SKU);
        public override string ToString() =>$"{SKU} - {ItemName}".ToLower();
        #endregion

        #region OwnMethods
        public bool Match(string value)=>
        SKU.Equals(value) || ItemName.ToLower().Contains(value);

        public bool Match(string value, Department department) =>
        Department?.DepartmentID==department?.DepartmentID && (SKU.Equals(value) || ItemName.ToLower().Contains(value));

        public string SQLQuery(QueryType Query)
        {
            switch (Query) 
            {
                case QueryType.SELECT:
                    return "SELECT * FROM Item;";
                default: return string.Empty;
            
            }

        }

        public Item GetRecord(MySqlDataReader reader) => new(reader);

        public void Params(MySqlParameterCollection param)
        {
        }

        public void SetPrimaryKey(ulong ID)
        {
        }

        #endregion
    }
}
