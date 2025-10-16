using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Invaders;

public class GameScene : SceneBase
{
    private readonly SceneManager sceneManager;
    private readonly RenderWindow window;
    private bool gameOver = false;

    public GameScene(Scene scene, SceneManager sceneManager, RenderWindow window) : base(scene)
    {
        this.sceneManager = sceneManager;
        this.window = window;
    }

    public override void OnEnter()
    {
        Scene.Clear();
        Scene.Spawn(new Player());
    }

    public override void Update(float deltaTime)
    {
        Scene.UpdateAll(deltaTime);

        if (gameOver)
        {
            sceneManager.LoadScene(new GameOverScene(Scene, sceneManager, window));
        }
    }
    
    public override void Render(RenderTarget target)
    {
        Scene.RenderAll(target);
    }
}