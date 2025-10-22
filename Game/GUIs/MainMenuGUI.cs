using SFML.Graphics;
using SFML.Window;


namespace Invaders;

public class MainMenuGUI : GUIBase
{
    // Texts
    private Text titleText = null!;
    private Text playText = null!;
    private Text highscoresText = null!;
    private Text quitText = null!;

    // Misc.
    private bool confirmQuit = false;

    public MainMenuGUI(Scene scene, SceneManager sceneManager, RenderWindow window) : base(scene, sceneManager, window)
    {
        
    }

    public override void OnEnter()
    {
        CreateTexts();
        window.MouseButtonPressed += OnMousePressed;
    }

    public override void OnExit()
    {
        window.MouseButtonPressed -= OnMousePressed;
    }

    protected override void CreateTexts()
    {
        titleText = MakeText("INVADERS", dataFont, 150, black,
                        Program.screenW / 2f, 200);

        playText = MakeText("Play", protoFont, 50, darkGrey,
                        Program.screenW / 2f, Program.screenH - 300);

        highscoresText = MakeText("Highscores", protoFont, 50, darkGrey,
                        Program.screenW / 2f, Program.screenH - 250);

        quitText = MakeText("Quit", protoFont, 50, darkGrey,
                        Program.screenW / 2f, Program.screenH - 200);
    }

    public override void Update(float deltaTime)
    {
        playText.FillColor = IsMouseOver(playText) ? nearWhite : darkGrey;
        highscoresText.FillColor = IsMouseOver(highscoresText) ? nearWhite : darkGrey;
        quitText.FillColor = IsMouseOver(quitText) ? nearWhite : darkGrey;

        if (confirmQuit)
        {
            if (!IsMouseOver(quitText))
            {
                quitText = MakeText("Quit", protoFont, 50, darkGrey, Program.screenW / 2f, Program.screenH - 200);
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
                quitText = MakeText("Are you sure?", protoFont, 30, darkGrey,
                                Program.screenW / 2f, Program.screenH - 200);
                confirmQuit = true;
            }
            else
            {
                window.Close();
            }
        }
    }
}