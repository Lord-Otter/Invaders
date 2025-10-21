using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Invaders;


namespace Invaders;

public class GameOverGUI : GUIBase
{
    private readonly SceneManager sceneManager;

    // Fonts
    private Font font = null!;

    // Texts
    private Text gameOverText = null!;

    // Colors
    private Color titleColor = new Color(10, 10, 10);
    private Color defaultColor = new Color(30, 30, 30);
    private Color hoverColor = new Color(200, 200, 200);

    // Misc.
    private bool confirmQuit = false;


    public GameOverGUI(Scene scene, SceneManager sceneManager, RenderWindow window) : base(scene, window)
    {
        this.sceneManager = sceneManager;
    }

    public override void OnEnter()
    {
        font = scene.AssetManager.LoadFont("prototype");

        CreateTexts();
        window.MouseButtonPressed += OnMousePressed;
    }

    public override void OnExit()
    {
        window.MouseButtonPressed -= OnMousePressed;
    }

    private void CreateTexts()
    {
        
    }

    public override void Update(float deltaTime)
    {
        /*playText.FillColor = IsMouseOver(playText) ? hoverColor : defaultColor;
        highscoresText.FillColor = IsMouseOver(highscoresText) ? hoverColor : defaultColor;
        quitText.FillColor = IsMouseOver(quitText) ? hoverColor : defaultColor;

        if (confirmQuit)
        {
            if (!IsMouseOver(quitText))
            {
                quitText = MakeText("Quit", font, 50, defaultColor, Program.screenW / 2f, Program.screenH - 200);
                confirmQuit = false;
            }
        }*/
    }

    public override void Render(RenderTarget target)
    {
        /*target.Draw(titleText);
        target.Draw(playText);
        target.Draw(highscoresText);
        target.Draw(quitText);*/
    }

    protected override void OnMousePressed(object? sender, MouseButtonEventArgs e)
    {
        /*if (IsMouseOver(playText))
        {
            sceneManager.LoadScene(new GameScene(scene, sceneManager, window));
        }
        else if (IsMouseOver(highscoresText))
        {
            sceneManager.LoadScene(new HighscoresScene(scene, sceneManager, window));
        }
        else if (IsMouseOver(quitText))
        {
            if (!confirmQuit)
            {
                quitText = MakeText("Are you sure?", font, 30, defaultColor,
                                Program.screenW / 2f, Program.screenH - 200);
                confirmQuit = true;
            }
            else
            {
                window.Close();
            }
        }*/
    }
}