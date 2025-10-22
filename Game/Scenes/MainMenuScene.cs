using SFML.Graphics;


namespace Invaders;

public class MainMenuScene : SceneBase
{
    private readonly MainMenuGUI gui = null!;

    public MainMenuScene(Scene scene, SceneManager sceneManager, RenderWindow window) : base(scene)
    {
        gui = new MainMenuGUI(scene, sceneManager, window);
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