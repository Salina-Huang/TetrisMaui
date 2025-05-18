using Microsoft.Maui.Graphics;

public class PreviewDrawable : IDrawable
{
    private readonly GameDrawable game;
    public PreviewDrawable(GameDrawable game) => this.game = game;

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        float size = 20;
        canvas.FillColor = Color.FromArgb("#7299FD");

        foreach (var cell in game.NextBlock)
        {
            float dx = (cell.X - 4) * size;
            float dy = cell.Y * size;
            canvas.FillRectangle(dx, dy, size - 2, size - 2);
        }
    }
}
