
namespace MyPlanogram.View;

public partial class Tab : Shell
{

    public Tab()
    {
        InitializeComponent();
    }

    string _title2;
    public string Title2 
    { 
        get 
            {
            return _title2;
            }
        set
        {
            _title2 = value;
            OnPropertyChanged("Title2");
        }
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);
        SetTitle();
    }

    private void SetTitle()
    {
        if (Shell.Current == null) return;
        Page page = Shell.Current.CurrentPage;

        switch(page) 
        {
            case PlanogramPage:
                Title2 = "MyPlanogram";
                break;
            case BarcodeReader:
                Title2 = "Barcode Reader";
                break;
            case BarcodeList:
                Title2 = "Barcodes";
                break;
            default:
                Title2=((ITitle)page).TopTitle;
                break;
        }
    }
}