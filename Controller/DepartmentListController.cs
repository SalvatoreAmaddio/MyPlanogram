using MyPlanogram.Model;
using MyPlanogram.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Testing.Controller.AbstractControllers;
using Testing.DB;
using Testing.RecordSource;

namespace MyPlanogram.Controller
{
     public class DepartmentListController : AbstractDataListController<Item>
     {
        public RecordSource<Department> Departments { get => MainDB.DBDepartment.RecordSource; }
        Department _selecteddepartment;
        public Department SelectedDepartment { get => _selecteddepartment; set => Set<Department>(ref value, ref _selecteddepartment,nameof(SelectedDepartment)); }
        public ICommand OpenBarcodeListCMD { get; }

        public DepartmentListController() 
        {
            OpenBarcodeListCMD = new Command<Item>(OpenBarcodeList);
            RecordSource.ReplaceRange(MainDB.DBItem.RecordSource);
            SelectedRecord = RecordSource.FirstOrDefault();
            SelectedDepartment = Departments.FirstOrDefault();
            AfterPropChanged += DepartmentListController_AfterPropChanged;
        }

        private void DepartmentListController_AfterPropChanged(object sender, Testing.Model.Abstracts.PropChangedEvtArgs e)
        {
            if (e.PropIs(nameof(SelectedDepartment)))
            {
                var dep = e.GetValue<Department>();
                RecordSource.ReplaceRange(MainDB.DBItem.RecordSource.Where(s => s.Match(Search, dep)));
                return;
            }

            if (e.PropIs(nameof(Search)))
            {
                var search = e.GetValue<string>();
                RecordSource.ReplaceRange(MainDB.DBItem.RecordSource.Where(s => s.Match(search,SelectedDepartment)));
                return;
            }
        }

        private async void OpenBarcodeList(Item item)
        {
            SelectedRecord = item;

            var okay = MainDB.DBBarcode.RecordSource.Any(s => s.Item.ItemID == item.ItemID);
            if (!okay)
            {
                await Shell.Current.DisplayAlert("SORRY", "There are no barcodes for this item.", "OK");
                return;
            }
            await Shell.Current.Navigation.PushAsync(new BarcodeList(item));
        }
        public override void GoBack()
        {
        }

        public override void OpenPage(Item record)
        {
        }
    }
}
