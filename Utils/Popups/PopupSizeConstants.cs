namespace SteveLauncher.Utils.Popups;

public class PopupSizeConstants {
    public PopupSizeConstants(IDeviceDisplay deviceDisplay)
    {
        Tiny = new(100, 100);
        Small = new(300, 300);
        Medium = new Size(800, 600); 
        MediumRatio = new(0.5 * (deviceDisplay.MainDisplayInfo.Width / deviceDisplay.MainDisplayInfo.Density), 0.5 * (deviceDisplay.MainDisplayInfo.Height / deviceDisplay.MainDisplayInfo.Density));
        LargeRaito= new(0.7 * (deviceDisplay.MainDisplayInfo.Width / deviceDisplay.MainDisplayInfo.Density), 0.6 * (deviceDisplay.MainDisplayInfo.Height / deviceDisplay.MainDisplayInfo.Density));
    }

    // examples for fixed sizes
    public Size Tiny { get; }

    public Size Small { get; }

    public Size Medium { get; }
    
    // examples for relative to screen sizes
    public Size MediumRatio { get; }

    public Size LargeRaito { get; }

}