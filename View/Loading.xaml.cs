using System.ComponentModel;
using Testing.DB;

namespace MyPlanogram.View;

public partial class Loading : ContentPage
{
    public Loading()
	{
        
		InitializeComponent();
	}

 
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await MainDB.FetchData(ProgressBar,ProgressDetails);
        App.Current.MainPage = new Tab();
    }


}