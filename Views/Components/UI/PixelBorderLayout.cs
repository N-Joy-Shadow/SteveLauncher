using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace SteveLauncher.Views.Components.UI;

public class PixelBorderView : ContentView
{
    public Color BorderColor { get; set; } = Colors.Black;
    public int BorderWidth { get; set; } = 4;
    public int PixelSize { get; set; } = 8;

    public PixelBorderView()
    {
        // SKCanvasView를 추가하여 Pixelized Border를 그림
        var canvasView = new SKCanvasView();
        canvasView.PaintSurface += OnPaintSurface;

        // Content를 표시할 수 있도록 Grid 사용
        var layout = new Grid();
        layout.Children.Add(canvasView);         // SKCanvasView를 배경으로 추가
        layout.Children.Add(new ContentPresenter()); // 자식을 포함하도록 ContentPresenter 추가

        Content = layout;
    }

    protected void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.Transparent);

        var rect = new SKRect(0, 0, e.Info.Width, e.Info.Height);
        var paint = new SKPaint
        {
            Color = BorderColor.ToSKColor(),
            Style = SKPaintStyle.Stroke,
            StrokeWidth = BorderWidth
        };

        // Pixelized Border를 그림
        float step = PixelSize;
        for (float x = rect.Left; x < rect.Right; x += step)
        {
            canvas.DrawRect(x, rect.Top, step, BorderWidth, paint); // Top Border
            canvas.DrawRect(x, rect.Bottom - BorderWidth, step, BorderWidth, paint); // Bottom Border
        }
        for (float y = rect.Top; y < rect.Bottom; y += step)
        {
            canvas.DrawRect(rect.Left, y, BorderWidth, step, paint); // Left Border
            canvas.DrawRect(rect.Right - BorderWidth, y, BorderWidth, step, paint); // Right Border
        }
    }
}