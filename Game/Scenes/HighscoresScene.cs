using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Text.Json;

namespace Invaders;

public class HighscoresScene : SceneBase
{
    private readonly SceneManager sceneManager;
    private readonly RenderWindow window;
    private HighscoresGUI gui = null!;

    public HighscoresScene(Scene scene, SceneManager sceneManager, RenderWindow window) : base(scene)
    {
        this.sceneManager = sceneManager;
        this.window = window;
    }

    public override void OnEnter()
    {
        gui = new HighscoresGUI(scene, sceneManager, window);
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