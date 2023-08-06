using MyPlanogram.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing.Controller.AbstractControllers;
using Testing.DB;
using ZXing;
#nullable enable
namespace MyPlanogram.Controller
{
    public class BarcodeListController : AbstractDataListController<Barcode>
    {
        public event EventHandler<BarcodeDetectedEventArgs>? BarcodeDetected;

        bool _isdetecting = true;
        public bool IsDetecting 
        {
            get => _isdetecting;
            set => Set<bool>(ref value, ref _isdetecting,nameof(IsDetecting));
        }

        string? _result;
        public string? Result 
        {
            get => _result;
            set => Set<string?>(ref value, ref _result,nameof(Result));
        }

        public BarcodeListController() { }

        public void ManageEvent(ZXing.Net.Maui.BarcodeDetectionEventArgs e)
        {
            Application.Current.Dispatcher.Dispatch(() =>
            {
                Result = e.Results[e.Results.Length - 1].Value;
                IsDetecting = false;
                BarcodeDetected?.Invoke(this, new(Result));    
            });
        }

        public override void GoBack()
        {
        }
        
        public override void OpenPage(Barcode record)
        {
        }

        public void Filter(Int64 ItemID)
        {
            var range=MainDB.DBBarcode.RecordSource.Where(s=>s.Item.ItemID==ItemID && s.IsValid() && s.Check());
            RecordSource.ReplaceRange(range);
            Record=RecordSource.FirstOrDefault();
        }
    }

    #region BarcodeDetectedEventArgs
    public class BarcodeDetectedEventArgs : EventArgs
    {
        public string? BarcodeString;
        public Barcode? Barcode;
        public bool Found
        {
            get=>Barcode != null;
        }

        public BarcodeDetectedEventArgs(string? barcode)
        {
            BarcodeString = barcode;
            Barcode = MainDB.DBBarcode.RecordSource.FirstOrDefault(s => s.ToString().Equals(BarcodeString));
        }
    }
    #endregion
}
