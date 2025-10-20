using System.Text;
using Invaders;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Invaders;

public abstract class GUIBase
{
    protected readonly RenderWindow window;
    protected readonly Scene scene;

    protected GUIBase(Scene scene, RenderWindow window)
    {
        this.scene = scene;
        this.window = window;
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void Update(float deltaTime) { }
    public virtual void Render(RenderTarget target) { }

    protected Text MakeText(string content, Font font, uint size, Color textColor, float x, float y, Vector2f? originAlign = null)
    {
        Text text = new Text(content, font, size);
        text.FillColor = new Color(textColor);
        
        FloatRect bounds = text.GetLocalBounds();
        Vector2f align = originAlign ?? new Vector2f(0, 0);
        float originX = bounds.Left + bounds.Width * (0.5f + align.X * 0.5f);
        float originY = bounds.Top + bounds.Height * (0.5f + align.Y * 0.5f);
        text.Origin = new Vector2f(originX, originY);

        text.Position = new Vector2f(x, y);
        return text;
    }
    
    protected virtual void OnMousePressed(object? sender, MouseButtonEventArgs e)
    {
        if (e.Button != Mouse.Button.Left) return;
    }

    protected bool IsMouseOver(Text text)
    {
        FloatRect bounds = text.GetGlobalBounds();
        Vector2f mousePosition = window.MapPixelToCoords(Mouse.GetPosition(window));
        return bounds.Contains(mousePosition.X, mousePosition.Y);
    }
}