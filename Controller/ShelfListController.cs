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
#nullable enable

namespace MyPlanogram.Controller
{
    public class ShelfListController : AbstractDataListController<Shelf>
    {
        Bay? _bay;
        public Bay? Bay { get=>_bay; set=>Set<Bay?>(ref value, ref _bay,nameof(Bay)); }
        public ICommand ViewPlanogramCMD { get; }

        public ShelfListController() 
        {
            ViewPlanogramCMD = new Command(ViewPlanogram);            
        }

        public override void GoBack()
        {
        }

        public void Filter(Bay bay)
        {
            Bay= bay;
            RecordSource.ReplaceRange(MainDB.DBShelf.RecordSource.Where(s=>s.Bay.Equals(Bay)).OrderByDescending(s=>s.Notch));
            SelectedRecord=RecordSource.FirstOrDefault();
            AfterPropChanged += ShelfListController_AfterPropChanged;
        }

        private void ShelfListController_AfterPropChanged(object? sender, Testing.Model.Abstracts.PropChangedEvtArgs e)
        {
              if (e.PropIs(nameof(SelectedRecord)))
                {
                    OpenPage(e.GetValue<Shelf>());
                }
        }

        private async void ViewPlanogram()
        {
            var shelf = RecordSource.FirstOrDefault();
            Planogram planogram = MainDB.DBPlanogram.RecordSource.Where(s=>s.Shelf.Equals(shelf)).OrderBy(s=>s.Orderlist).FirstOrDefault();
            await Shell.Current.Navigation.PushAsync(new BayPlanogramPage(planogram));
        }

        public override async void OpenPage(Shelf record)
        {
            if (record == null) return;
            Planogram planogram = new();
            planogram.PlanoID = -1;
            record.Bay = Bay;
            planogram.Shelf = record;
            await Shell.Current.Navigation.PushAsync(new BayPlanogramPage(planogram));
            SelectedRecord = null;
        }

    }
}
