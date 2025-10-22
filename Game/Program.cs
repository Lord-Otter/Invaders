// Explosion sprite was downloaded from https://www.vecteezy.com
// Prototype Font by Justin Callaghan https://www.1001freefonts.com/prototype.font
// Data Control Font by Vic Fieger https://www.1001freefonts.com/data-control.font

using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

public class Program
{
    public const int screenW = 800;
    public const int screenH = 1000;
    public static float timeScale = 1f;

    public static void Main(string[] args)
    {
        using RenderWindow window = new RenderWindow(new VideoMode(screenW, screenH), "Invaders", Styles.Default);
        View gameView = new View(new FloatRect(0, 0, screenW, screenH));
        Letterboxing(window, gameView);
        window.Resized += (sender, e) => { Letterboxing(window, gameView); };
        
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

            window.Clear(Color.Black);

            window.SetView(gameView);
            
            RectangleShape background = new RectangleShape(new Vector2f(screenW, screenH));
            background.FillColor = new Color(94, 63, 107);
            background.Position = new Vector2f(0, 0);
            window.Draw(background);

            sceneManager.Render(window);
            window.Display();
        }
    }
    
    private static void Letterboxing(RenderWindow window, View view)
    {
        float windowRatio = (float)window.Size.X / (float)window.Size.Y;
        float viewRatio = (float)screenW / (float)screenH;

        float sizeX = 1f;
        float sizeY = 1f;
        float posX = 0f;
        float posY = 0f;

        if (windowRatio > viewRatio)
        {
            sizeX = viewRatio / windowRatio;
            posX = (1f - sizeX) / 2f;
        }
        else
        {
            sizeY = windowRatio / viewRatio;
            posY = (1f - sizeY) / 2f;
        }

        view.Viewport = new FloatRect(posX, posY, sizeX, sizeY);
        window.SetView(view);
    }
}