using Microsoft.Maui.Graphics;
using IImage = Microsoft.Maui.Graphics.IImage;


namespace SteveLauncher.Extension;

public class ImageViewExtension: Image {
    public static readonly BindableProperty SourceFromBase64Property = BindableProperty.Create(
        "ImageSourceFromBase64", typeof(string), typeof(ImageViewExtension),propertyChanged: OnSourceFromBase64Changed);
    
    public ImageSource SourceFromBase64 {
        get => (ImageSource)GetValue(SourceFromBase64Property);
        set => SetValue(SourceFromBase64Property, value);
    }
    
    
    private static void OnSourceFromBase64Changed(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (ImageViewExtension)bindable;
        var base64String = newValue as string;

        if (!string.IsNullOrEmpty(base64String))
            control.Source = ImageSourceExtension.FromBase64(base64String);
        
    }
}