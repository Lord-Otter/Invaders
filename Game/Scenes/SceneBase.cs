using System.Runtime.InteropServices.Marshalling;
using Invaders;
using SFML.Graphics;
using SFML.Window;

namespace Invaders;

public abstract class SceneBase
{
    protected readonly Scene Scene;

    protected SceneBase(Scene scene)
    {
        Scene = scene;
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnMousePressed(object? sender, MouseButtonEventArgs e){ }
    public virtual void Update(float deltaTime) { }
    public virtual void Render(RenderTarget target) { }
}