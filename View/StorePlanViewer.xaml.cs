using MyPlanogram.Model;

namespace MyPlanogram.View;

public partial class StorePlanViewer : ContentPage, ITitle
{
    public string TopTitle { get; set; } = "Store Plan";

    public StorePlanViewer()
	{
		InitializeComponent();
	}
  
}