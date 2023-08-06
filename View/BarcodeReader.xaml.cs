using MyPlanogram.Controller;
using ZXing.Net.Maui;

namespace MyPlanogram.View;

public partial class BarcodeReader : ContentPage
{
	public BarcodeListController Controller { get; }

    public BarcodeReader()
    {
        InitializeComponent();
        Controller = (BarcodeListController)BindingContext;
        CodeReader.Options = new()
        {
            AutoRotate = true,
            Multiple = false,
            TryHarder = true,
            TryInverted = true,
            Formats = BarcodeFormats.OneDimensional
        };
    }

    private void CameraBarcodeReaderView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)=>
	Controller.ManageEvent(e);
}