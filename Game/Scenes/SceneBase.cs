using SFML.Graphics;
using SFML.Window;

namespace Invaders;

public abstract class SceneBase
{
    protected readonly Scene scene;

    protected SceneBase(Scene scene)
    {
        this.scene = scene;
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnMousePressed(object? sender, MouseButtonEventArgs e){ }
    public virtual void Update(float deltaTime) { }
    public virtual void Render(RenderTarget target) { }
}