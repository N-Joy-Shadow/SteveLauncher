namespace SteveLauncher.Extension;

public static class ImageSourceExtension {
    public static ImageSource FromBase64(string base64) {
        if(!base64.StartsWith("data:image"))
            throw new ArgumentException("Invalid base64 string");
        
        string pureBase64 = base64.Split(",")[1];

        return ImageSource.FromStream(() =>
            new MemoryStream(Convert.FromBase64String(pureBase64)));
    }
}