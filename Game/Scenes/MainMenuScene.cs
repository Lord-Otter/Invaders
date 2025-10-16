using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Collections.Concurrent;

namespace Invaders;

public class MainMenuScene : SceneBase
{
    private readonly SceneManager sceneManager;
    private readonly RenderWindow window;

    private Font font = null!;
    private Font titleFont = null!;

    private Text titleText = null!;
    private Text playText = null!;
    private Text highscoresText = null!;
    private Text quitText = null!;

    private bool confirmQuit = false;

    public MainMenuScene(Scene scene, SceneManager sceneManager, RenderWindow window) : base(scene)
    {
        this.sceneManager = sceneManager;
        this.window = window;
    }

    #region Text
    private void TitleText()
    {
        titleFont = Scene.AssetManager.LoadFont("data-control");
        titleText = new Text("INVADERS", titleFont, 150);
        titleText.FillColor = new Color(10, 10, 10);
        FloatRect bounds = titleText.GetLocalBounds();
        titleText.Origin = new Vector2f(bounds.Left + bounds.Width / 2f, bounds.Top + bounds.Height / 2f);
        titleText.Position = new Vector2f(Program.screenW / 2f, 200);
    }

    private void PlayText()
    {
        playText = new Text("Play", font, 50);
        playText.FillColor = new Color(30, 30, 30);
        FloatRect bounds = playText.GetLocalBounds();
        playText.Origin = new Vector2f(bounds.Left + bounds.Width / 2f, bounds.Top + bounds.Height / 2f);
        playText.Position = new Vector2f(Program.screenW / 2f, Program.screenH - 300);
    }

    private void HighscoresText()
    {
        highscoresText = new Text("Highscores", font, 50);
        highscoresText.FillColor = new Color(30, 30, 30);
        FloatRect bounds = highscoresText.GetLocalBounds();
        highscoresText.Origin = new Vector2f(bounds.Left + bounds.Width / 2f, bounds.Top + bounds.Height / 2f);
        highscoresText.Position = new Vector2f(Program.screenW / 2f, Program.screenH - 250);
    }
    
    private void QuitText()
    {
        quitText = new Text("Quit", font, 50);
        quitText.FillColor = new Color(30, 30, 30);
        FloatRect bounds = quitText.GetLocalBounds();
        quitText.Origin = new Vector2f(bounds.Left + bounds.Width / 2f, bounds.Top + bounds.Height / 2f);
        quitText.Position = new Vector2f(Program.screenW / 2f, Program.screenH - 200);
    }
    #endregion

    public override void OnEnter()
    {
        TitleText();
        font = Scene.AssetManager.LoadFont("prototype");
        PlayText();
        HighscoresText();
        QuitText();

        window.MouseButtonPressed += OnMousePressed;
    }

    public override void OnExit()
    {
        window.MouseButtonPressed -= OnMousePressed;
    }

    public override void Update(float deltaTime)
    {
        playText.FillColor = IsMouseOver(playText) ? new Color(200, 200, 200) : new Color(30, 30, 30);
        highscoresText.FillColor = IsMouseOver(highscoresText) ? new Color(200, 200, 200) : new Color(30, 30, 30);
        quitText.FillColor = IsMouseOver(quitText) ? new Color(200, 200, 200) : new Color(30, 30, 30);

        if (confirmQuit)
        {
            if (!IsMouseOver(quitText))
            {
                quitText.DisplayedString = "Quit";
                quitText.CharacterSize = 50;
                FloatRect bounds = quitText.GetLocalBounds();
                quitText.Origin = new Vector2f(bounds.Left + bounds.Width / 2f, bounds.Top + bounds.Height / 2f);
                quitText.Position = new Vector2f(Program.screenW / 2f, Program.screenH - 200);
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

    private bool IsMouseOver(Text text)
    {
        FloatRect bounds = text.GetGlobalBounds();
        Vector2i mousePos = Mouse.GetPosition(window); // Mouse position in window coordinates
        return bounds.Contains(mousePos.X, mousePos.Y);
    }

    public override void OnMousePressed(object? sender, MouseButtonEventArgs e)
    {
        if (e.Button != Mouse.Button.Left) return;

        if (IsMouseOver(playText))
        {
            sceneManager.LoadScene(new GameScene(Scene, sceneManager, window));
        }
        else if (IsMouseOver(highscoresText))
        {
            sceneManager.LoadScene(new HighscoresScene(Scene, sceneManager, window));
        }
        else if (IsMouseOver(quitText))
        {
            if (!confirmQuit)
            {
                quitText.DisplayedString = "Are you sure?";
                quitText.CharacterSize = 30;
                FloatRect bounds = quitText.GetLocalBounds();
                quitText.Origin = new Vector2f(bounds.Left + bounds.Width / 2f, bounds.Top + bounds.Height / 2f);
                quitText.Position = new Vector2f(Program.screenW / 2f, Program.screenH - 200);
                confirmQuit = true;
            }
            else
            {
                window.Close();
            }
        }
    }
}