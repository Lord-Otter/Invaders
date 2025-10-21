using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Invaders;

public class GameOverScene : SceneBase
{
    private readonly SceneManager sceneManager;
    private readonly RenderWindow window;
    private GameOverGUI gui = null!;

    private int playerScore;

    public GameOverScene(Scene scene, SceneManager sceneManager, RenderWindow window, int currentScore) : base(scene)
    {
        this.sceneManager = sceneManager;
        this.window = window;

        playerScore = currentScore;
    }

    public override void OnEnter()
    {
        gui = new GameOverGUI(scene, sceneManager, window, playerScore);
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