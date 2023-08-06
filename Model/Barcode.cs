using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing.DB;
using Testing.Model.Abstracts;
using ZXing.Net.Maui;

namespace MyPlanogram.Model
{
    public class Barcode : AbstractModel, IDB<Barcode>
    {
        #region backprop
        Int64 _barcodeid;
        string _code;
        Item _item;
        BarcodeFormat _format;
        #endregion

        #region Props
        public Int64 BarcodeID { get=>_barcodeid; set=>Set<Int64>(ref value, ref _barcodeid,nameof(BarcodeID)); }
        public string Code { get => _code; set => Set<string>(ref value, ref _code, nameof(Code)); }
        public Item Item { get => _item; set => Set<Item>(ref value, ref _item, nameof(Item)); }
        public BarcodeFormat Format { get => _format; set => Set<BarcodeFormat>(ref value, ref _format, nameof(Format)); }
        #endregion

        #region Contructors 
        public Barcode()
        {

        }

        public Barcode(MySqlDataReader reader)
        {
            AfterPropChanged += Barcode_AfterPropChanged;
            BarcodeID = reader.GetInt64(0);
            Code=reader.GetString(1);
            Item = new(reader.GetInt64(2));
        }

        private bool IsEven(int num) => num % 2 == 0;
        private bool IsOdd(int num) => num % 2 != 0;

        public bool Check()
        {
            char[] digits = Code.ToCharArray();

            int EvenDigits = 0;
            int OddDigits = 0;
            int number;
            int position = 1;

            foreach (char d in digits)
            {
                number = Int32.Parse(d.ToString());
                if (IsOdd(position))
                {
                    OddDigits = OddDigits + number;
                }
                else
                {
                    EvenDigits = EvenDigits + number;
                }

                position++;
            }

            return Int32.Parse((OddDigits + (EvenDigits * 3)).ToString().LastOrDefault().ToString()) == 0;
        }

        public bool IsValid() => Code.Length <= 13;

        private void Barcode_AfterPropChanged(object sender, PropChangedEvtArgs e)
        {
            if (!e.PropIs(nameof(Code))) return;
                switch (e.GetValue<string>().Length)
                {
                    case 8:
                    Format = BarcodeFormat.Ean8;
                        break;
                    case 13:
                        Format = BarcodeFormat.Ean13;
                        break;
                    case 2:
                        Format = BarcodeFormat.Code128;
                        break;
                default:
                    Format = BarcodeFormat.Codabar;
                        break;
                }
        }
        #endregion

        #region AbstractModel
        public override bool IsNewRecord => throw new NotImplementedException();
        public override void SetForeignKeys()
        {
            Item = MainDB.DBItem.RecordSource.FirstOrDefault(s=>s.ItemID==Item.ItemID);
        }

        #endregion

        #region EqualsString
        public override bool Equals(object obj) => obj is Barcode barcode && BarcodeID == barcode.BarcodeID;
        public override int GetHashCode() => HashCode.Combine(BarcodeID);
        public override string ToString() => Code;
        #endregion

        #region IDB
        public Barcode GetRecord(MySqlDataReader reader) => new(reader);

        public void Params(MySqlParameterCollection param)
        {
        }

        public void SetPrimaryKey(ulong ID)
        {
        }

        public string SQLQuery(QueryType Query)
        {
            switch (Query)
            {
                case QueryType.SELECT:
                    return "SELECT * FROM Barcode;";
                case QueryType.DELETE:
                    return string.Empty;
                case QueryType.UPDATE:
                    return string.Empty;
                case QueryType.INSERT:
                    return string.Empty;
                default: return string.Empty;
            }
        }
        #endregion
    }
}
