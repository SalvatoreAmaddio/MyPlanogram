using MyPlanogram.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Testing.Model.Abstracts;
using Testing.RecordSource;

namespace Testing.DB
{
    public static class MainDB 
    {
        public const string ConnectionString = "Server=sql4.freemysqlhosting.net;Database=sql4497931;Uid=sql4497931;Pwd=2Tf1qm9dEG;Allow User Variables=true; AllowZeroDateTime=True; ConvertZeroDateTime=True;";
        public static MySQLDB<Planogram> DBPlanogram { get; } = new();
        public static MySQLDB<Shelf> DBShelf { get; } = new();
        public static MySQLDB<Bay> DBBay { get; } = new();
        public static MySQLDB<Section> DBSection { get; } = new();
        public static MySQLDB<Offer> DBOffer { get; } = new();
        public static MySQLDB<Barcode> DBBarcode { get; } = new();
        public static MySQLDB<Department> DBDepartment { get; } = new();
        public static MySQLDB<Item> DBItem { get; } = new();

        private static Label ProgressDetails;

        public static void SetCascadeEvents()
        {
        }

        private static Task Update(string text)
        {
            ProgressDetails.Dispatcher.Dispatch(() =>
            {
                ProgressDetails.Text = text;

            });
            return Task.CompletedTask;
        }

        public static async Task FetchData(ProgressBar progressBar,Label ProgressDetails)
        {
            MainDB.ProgressDetails = ProgressDetails;

            await Task.WhenAll(
                Task.Run(() =>
                {
                    Update("Connecting...");
                    DBPlanogram.OpenConnection();
                    DBShelf.OpenConnection();
                    DBBay.OpenConnection();
                    DBSection.OpenConnection();
                    DBOffer.OpenConnection();
                    DBBarcode.OpenConnection();
                    DBDepartment.OpenConnection();
                    DBItem.OpenConnection();
                    progressBar.ProgressTo(0.10, 500, Easing.Linear);
                }));


            await Task.WhenAll(
                Task.Run(() =>
                {
                    Update("Downloading Departments...");
                    DBDepartment.Select();
                    progressBar.ProgressTo(0.10, 500, Easing.Linear);

                    Update("Downloading Offers...");
                    DBOffer.Select();
                    progressBar.ProgressTo(0.20, 500, Easing.Linear);

                    Update("Downloading Items...");
                    DBItem.Select();
                    progressBar.ProgressTo(0.30, 500, Easing.Linear);

                    Update("Downloading Planograms...");
                    DBPlanogram.Select();
                    progressBar.ProgressTo(0.40, 500, Easing.Linear);

                    Update("Downloading Bays...");
                    DBBay.Select();
                    progressBar.ProgressTo(0.50, 500, Easing.Linear);

                    Update("Downloading Shelves...");
                    DBShelf.Select();
                    progressBar.ProgressTo(0.60, 500, Easing.Linear);

                    Update("Downloading Sections...");
                    DBSection.Select();
                    progressBar.ProgressTo(0.70, 500, Easing.Linear);

                    Update("Downloading Barcodes...");
                    DBBarcode.Select();
                    progressBar.ProgressTo(0.80, 500, Easing.Linear);

                }));

            await Task.WhenAll(
            Task.Run(() =>
            {
            Update("Disconnecting...");
            DBDepartment.CloseConnection();
            DBPlanogram.CloseConnection();
            DBShelf.CloseConnection();
            DBBay.CloseConnection();
            DBSection.CloseConnection();
            DBOffer.CloseConnection();
            DBBarcode.CloseConnection();
            DBItem.CloseConnection();
            Parallel.ForEach<Item>(DBItem.RecordSource, record => record.SetForeignKeys());

            progressBar.ProgressTo(0.80, 500, Easing.Linear);
            Update("Setting Items on Shelfs in Planogram...");
            Parallel.ForEach<Planogram>(DBPlanogram.RecordSource, record => record.SetForeignKeys());

            progressBar.ProgressTo(0.90, 500, Easing.Linear);
            Update("Matching Items and Barcodes...");
            Parallel.ForEach<Barcode>(DBBarcode.RecordSource, record => record.SetForeignKeys());

            progressBar.ProgressTo(0.95, 500, Easing.Linear);
            Update("Inserting not ranged items...");
            RecordSource<Planogram> notinit = new();
            var range = DBItem.RecordSource.Where(s => NotIn(s));
            Parallel.ForEach<Item>(range, record => notinit.Add(new(record)));
            
            progressBar.ProgressTo(0.99, 500, Easing.Linear);
            DBPlanogram.RecordSource.AddRange(notinit);
            })
            );
        }
        

        public static bool NotIn(Item item) =>
        !DBPlanogram.RecordSource.Any(s => s.Item.ItemID == item.ItemID);
    }
}
