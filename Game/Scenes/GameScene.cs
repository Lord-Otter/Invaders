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
    private bool isPaused = false;

    public GameScene(Scene scene, SceneManager sceneManager, RenderWindow window) : base(scene)
    {
        this.sceneManager = sceneManager;
        this.window = window;
    }

    public override void OnEnter()
    {
        gui = new GameGUI(scene, sceneManager, window, this);
        gui.OnEnter();
        gameManager = new GameManager(scene, sceneManager, window, gui);
        gameManager.Initialize();

        window.KeyPressed += OnKeyPressed;
    }

    public override void OnExit()
    {
        gui.OnExit();
        window.KeyPressed -= OnKeyPressed;
    }

    public bool IsPaused()
    {
        return isPaused;
    } 

    public void SetPaused(bool paused)
    {
        isPaused = paused;
    }

    public override void Update(float deltaTime)
    {
        scene.UpdateAll(deltaTime);
        gui.Update(deltaTime);
        gameManager.Update(deltaTime);

        if (isPaused)
        {
            Program.timeScale = 0;
        }
        else
        {
            Program.timeScale = 1;
        }
    }

    private void OnKeyPressed(object? sender, KeyEventArgs e)
    {
        if (e.Code == Keyboard.Key.Escape)
        {
            isPaused = !isPaused;
        }
    }
    
    public override void Render(RenderTarget target)
    {
        scene.RenderAll(target);
        gui.Render(target);
    }
}