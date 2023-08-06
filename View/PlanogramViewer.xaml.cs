using MyPlanogram.Model;

namespace MyPlanogram.View;

public partial class PlanogramViewer : ContentPage, ITitle
{
    public Planogram Planogram { get; set; }
    public string TopTitle { get; set; } = string.Empty;

    public PlanogramViewer()
	{
		InitializeComponent();
	}

    public PlanogramViewer(Planogram record)
    {
        InitializeComponent();
        BindingContext = this;
        Planogram = record;
        TopTitle = Planogram.Shelf.Bay.Section.SectionName;
        MyImg.Source = Planogram.Shelf.Bay.PictureURL;
    }

    private async void Button_Clicked(object sender, EventArgs e)=>
    await Shell.Current.Navigation.PushAsync(new BayPlanogramPage(Planogram));

  
}