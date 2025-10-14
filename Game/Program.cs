using Invaders;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

//Idk göra olika menyer eller scener med sceneloader eller kanske GameManager.
class Program
{
    static public uint screenW = 900;
    static public uint screenH = 900;
    static void Main(string[] args)
    {
        using var window = new RenderWindow(new VideoMode(screenW, screenH), "Invaders");
        window.Closed += (o, e) => window.Close();

        Clock clock = new Clock();
        Scene scene = new Scene();

        while (window.IsOpen)
        {
            window.DispatchEvents();

            float deltaTime = MathF.Min(clock.Restart().AsSeconds(), 0.01f);

            scene.UpdateAll(deltaTime);

            window.Clear(new Color(94, 63, 107));

            scene.RenderAll(window);
            window.Display();
        }
    }
}