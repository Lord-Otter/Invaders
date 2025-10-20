using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Invaders;

public class GameScene : SceneBase
{
    private readonly SceneManager sceneManager;
    private readonly RenderWindow window;
    private GameGUI gui = null!;
    private GameManager gameManager = null!;
    private bool gameOver = false;
    private bool isPaused = false;

    public GameScene(Scene scene, SceneManager sceneManager, RenderWindow window) : base(scene)
    {
        this.sceneManager = sceneManager;
        this.window = window;
    }

    public override void OnEnter()
    {
        gui = new GameGUI(scene, sceneManager, window);
        gui.OnEnter();
        gameManager = new GameManager(scene, sceneManager, window, gui);
        gameManager.Initialize();
    }

    public override void Update(float deltaTime)
    {
        scene.UpdateAll(deltaTime);
        gui.Update(deltaTime);
        gameManager.Update(deltaTime);

        if (gameOver)
        {
            sceneManager.LoadScene(new GameOverScene(scene, sceneManager, window));
        }
    }
    
    public override void Render(RenderTarget target)
    {
        scene.RenderAll(target);
    }
}