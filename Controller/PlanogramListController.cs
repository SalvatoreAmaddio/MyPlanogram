using MvvmHelpers;
using MyPlanogram.Model;
using MyPlanogram.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Testing.Controller.AbstractControllers;
using Testing.DB;
using Testing.RecordSource;
using Tab = MyPlanogram.View.Tab;
#nullable enable

namespace MyPlanogram.Controller
{
    public class PlanogramListController : AbstractDataListController<Planogram>
    {
        public ObservableRangeCollection<ShelfGroup> Groups { get; private set; } = new();

        ShelfGroup? _selectedgroup;
        public ShelfGroup? SelectedGroup { get => _selectedgroup; set => Set<ShelfGroup?>(ref value, ref _selectedgroup,nameof(SelectedGroup)); }
        
        public ICommand OpenBarcodeReaderCMD { get; }
        public ICommand OpenBarcodeListCMD { get; }

        public PlanogramListController() 
        {
            OpenBarcodeListCMD = new Command<Planogram>(OpenBarcodeList);
            OpenBarcodeReaderCMD = new Command(OpenBarcodeReader);
            SelectedRecord = RecordSource.FirstOrDefault();
            AfterPropChanged += PlanogramListController_AfterPropChanged;
        }

        private async void OpenBarcodeReader()
        {
            BarcodeReader barcodeReader = new();
            barcodeReader.Controller.BarcodeDetected += Controller_BarcodeDetected;
            await Shell.Current.Navigation.PushAsync(barcodeReader);
        }

        private async void OpenBarcodeList(Planogram planogram)
        {
            SelectedGroup = Groups.FirstOrDefault(s => s.NotchNumber == planogram?.Shelf?.Notch);
            SelectedRecord = planogram;

            var okay =MainDB.DBBarcode.RecordSource.Any(s => s.Item.ItemID == planogram.Item.ItemID);
            if (!okay)
            {
                await Shell.Current.DisplayAlert("SORRY","There are no barcodes for this item.","OK");
                return;
            }
            await Shell.Current.Navigation.PushAsync(new BarcodeList(planogram));
        }

        private async void Controller_BarcodeDetected(object? sender, BarcodeDetectedEventArgs e)
        {

            if (e.Found)
            {
                Search = e?.Barcode?.Item?.SKU;
            }
            await Shell.Current.Navigation.PopAsync();

            if (!e.Found)
            {
                await Shell.Current.DisplayAlert($"{e.BarcodeString} NOT FOUND", $"Could not find any item with this Barcode.\nTry again or search either by SKU or Item's Name.", "OK");
            }
        }

        public override void GoBack()
        {
        }
        
        public void Filter(Planogram planogram)
        {
            Shelf shelf = planogram.Shelf;
            if (planogram.PlanoID < 0)
            {
                var group = MainDB.DBPlanogram.RecordSource.Where(s => s.Shelf.Equals(planogram.Shelf)).OrderBy(s => s.Orderlist);
                Groups.Add(new(group));
            }
            else
            {
                var GroupByBay = MainDB.DBPlanogram.RecordSource.Where(s => s.Shelf.Bay.Equals(planogram.Shelf.Bay)).OrderByDescending(s => s.Shelf.ShelfNum).GroupBy(s => s.Shelf.ShelfNum);
                foreach (var group in GroupByBay)
                {
                    Groups.Add(new(group.OrderBy(s => s.Orderlist)));
                }
            }
            SelectedGroup = Groups.FirstOrDefault(s => s.NotchNumber == planogram?.Shelf?.Notch);
            SelectedRecord = (planogram.PlanoID < 0) ? SelectedGroup?.FirstOrDefault() : SelectedGroup?.FirstOrDefault(s => s.PlanoID == planogram.PlanoID);
        }

        public override async void OpenPage(Planogram record)=>
        await Shell.Current.Navigation.PushAsync(new PlanogramViewer(record));

        private void PlanogramListController_AfterPropChanged(object sender, Testing.Model.Abstracts.PropChangedEvtArgs e)
        {
            if (!e.PropIs(nameof(Search))) return;
                RecordSource.Clear();
                string value = e.GetValue<string>().ToLower();
                if (string.IsNullOrEmpty(value))
                return;
                
            RecordSource.ReplaceRange(MainDB.DBPlanogram.RecordSource.Where(
            s => s.Item.Match(value)));
        }

    }

    #region Group
    public class ShelfGroup : ObservableRangeCollection<Planogram>
    {
        public string? Shelf { get; private set; }
        public string? Notch { get; private set; }
        public int? NotchNumber;
        public bool? OnHook { get; private set; }
        private int? ShelfNum { get; set; }
        public Planogram? planogram;

        public ShelfGroup(IEnumerable<Planogram> planograms) : base(planograms)
        {
            planogram = this.FirstOrDefault();
            NotchNumber = planogram?.Shelf?.Notch;
            OnHook= planogram?.Shelf?.OnHook;
            ShelfNum = planogram?.Shelf?.ShelfNum;

            if ((bool)OnHook)
            {
                Shelf = $"ON HOOKS";
                Notch = string.Empty;
                return;
            }
            Shelf = $"SHELF: {ShelfNum}";
            Notch = $"NOTCH: {NotchNumber}";
        }

        public override bool Equals(object? obj)
        {
            return obj is ShelfGroup group &&
                   NotchNumber == group.NotchNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NotchNumber);
        }
    }
    #endregion
}
