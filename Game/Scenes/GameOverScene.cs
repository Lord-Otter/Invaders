using SFML.Graphics;

namespace Invaders;

public class GameOverScene : SceneBase
{
    private readonly GameOverGUI gui = null!;

    public GameOverScene(Scene scene, SceneManager sceneManager, RenderWindow window, int currentScore) : base(scene)
    {
        gui = new GameOverGUI(scene, sceneManager, window, currentScore);
    }

    public override void OnEnter() 
    {
        gui.OnEnter();
    }

    public override void OnExit()
    {
        gui.OnExit();
    }

    public override void Update(float deltaTime)
    {
        gui.Update(deltaTime);
    }

    public override void Render(RenderTarget target)
    {
        gui.Render(target);
    }
}