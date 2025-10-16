using System.Text;
using Invaders;
using SFML.Graphics;
using SFML.System;

namespace Invaders;

public class SceneManager
{
    private SceneBase? currentScene;

    public void LoadScene(SceneBase newScene)
    {
        currentScene?.OnExit();
        currentScene = newScene;
        currentScene.OnEnter();
    }

    public void Update(float deltaTime)
    {
        currentScene?.Update(deltaTime);
    }

    public void Render(RenderTarget target)
    {
        currentScene?.Render(target);
    }
}