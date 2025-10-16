// Explosion sprite was downloaded from www.vecteezy.com

using Invaders;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

class Program
{
    public const int screenW = 900;
    public const int screenH = 1000;
    public static float timeScale = 1f;
    
    static void Main(string[] args)
    {
        using RenderWindow window = new RenderWindow(new VideoMode(screenW, screenH), "Invaders");
        Image icon = new Image("assets/icon.png");
        window.SetIcon(35, 27, icon.Pixels);
        window.Closed += (_, _) => window.Close();

        Clock clock = new Clock();
        SceneManager sceneManager = new SceneManager();
        Scene scene = new Scene();

        sceneManager.LoadScene(new MainMenuScene(scene, sceneManager, window));

        while (window.IsOpen)
        {
            window.DispatchEvents();

            float deltaTime = MathF.Min(clock.Restart().AsSeconds() * timeScale, 0.01f);

            sceneManager.Update(deltaTime);

            window.Clear(new Color(94, 63, 107));

            sceneManager.Render(window);
            window.Display();
        }
    }
}