using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Invaders;

public class GameOverScene : SceneBase
{
    private readonly SceneManager sceneManager;
    private readonly RenderWindow window;

    public GameOverScene(Scene scene, SceneManager sceneManager, RenderWindow window) : base(scene)
    {
        this.sceneManager = sceneManager;
        this.window = window;
    }
}