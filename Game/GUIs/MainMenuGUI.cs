using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Invaders;


namespace Invaders;

public class MainMenuGUI : GUIBase
{
    private readonly SceneManager sceneManager;

    // Fonts
    private Font titleFont = null!;
    private Font font = null!;

    // Texts
    private Text titleText = null!;
    private Text playText = null!;
    private Text highscoresText = null!;
    private Text quitText = null!;

    // Colors
    private Color titleColor = new Color(10, 10, 10);
    private Color defualtColor = new Color(30, 30, 30);
    private Color hoverColor = new Color(200, 200, 200);

    // Misc.
    private bool confirmQuit = false;


    public MainMenuGUI(Scene scene, SceneManager sceneManager, RenderWindow window) : base(scene, window)
    {
        this.sceneManager = sceneManager;
    }

    public override void OnEnter()
    {
        titleFont = scene.AssetManager.LoadFont("data-control");
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
        titleText = MakeText("INVADERS", titleFont, 150, titleColor, Program.screenW / 2f, 200);
        playText = MakeText("Play", font, 50, defualtColor, Program.screenW / 2f, Program.screenH - 300);
        highscoresText = MakeText("Highscores", font, 50, defualtColor, Program.screenW / 2f, Program.screenH - 250);
        quitText = MakeText("Quit", font, 50, defualtColor, Program.screenW / 2f, Program.screenH - 200);
    }

    public override void Update(float deltaTime)
    {
        playText.FillColor = IsMouseOver(playText) ? hoverColor : defualtColor;
        highscoresText.FillColor = IsMouseOver(highscoresText) ? hoverColor : defualtColor;
        quitText.FillColor = IsMouseOver(quitText) ? hoverColor : defualtColor;

        if (confirmQuit)
        {
            if (!IsMouseOver(quitText))
            {
                quitText = MakeText("Quit", font, 50, defualtColor, Program.screenW / 2f, Program.screenH - 200);
                confirmQuit = false;
            }
        }
    }

    public override void Render(RenderTarget target)
    {
        target.Draw(titleText);
        target.Draw(playText);
        target.Draw(highscoresText);
        target.Draw(quitText);
    }

    protected override void OnMousePressed(object? sender, MouseButtonEventArgs e)
    {
        if (IsMouseOver(playText))
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
                quitText = MakeText("Are you sure?", font, 30, defualtColor, Program.screenW / 2f, Program.screenH - 200);
                confirmQuit = true;
            }
            else
            {
                window.Close();
            }
        }
    }
}