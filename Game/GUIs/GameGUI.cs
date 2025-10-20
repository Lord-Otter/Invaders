using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Invaders;

namespace Invaders;

public class GameGUI : GUIBase
{
    private readonly SceneManager sceneManager;

    public GameGUI(Scene scene, SceneManager sceneManager, RenderWindow window) : base(scene, window)
    {
        this.sceneManager = sceneManager;
    }
}