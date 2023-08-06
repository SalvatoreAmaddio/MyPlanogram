using MyPlanogram.Controller;
using MyPlanogram.Model;

namespace MyPlanogram.View;

public partial class BarcodeList : ContentPage
{
	public BarcodeListController Controller { get; }

	public BarcodeList()
	{
		InitializeComponent();
		Controller = (BarcodeListController) BindingContext;
	}

    public BarcodeList(Planogram planogram) : this()=>
    Controller.Filter(planogram.Item.ItemID);

    public BarcodeList(Item item) : this() =>
    Controller.Filter(item.ItemID);
}