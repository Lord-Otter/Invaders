using SFML.Graphics;


namespace Invaders;

public class HighscoresScene : SceneBase
{
    private readonly HighscoresGUI gui = null!;

    public HighscoresScene(Scene scene, SceneManager sceneManager, RenderWindow window) : base(scene)
    {
        gui = new HighscoresGUI(scene, sceneManager, window);
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