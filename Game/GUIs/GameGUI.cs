using SFML.Graphics;
using SFML.Window;
using SFML.System;


namespace Invaders;

public class GameGUI : GUIBase
{
    private readonly GameScene gameScene;

    // Texts
    private Text scoreText = null!;
    private Text titleText = null!;
    private Text resumeText = null!;
    private Text mainMenuText = null!;
    private Text quitText = null!;

    // Overlay
    private RectangleShape overlay = null!;

    // Health
    private List<Sprite> healthSprites = new List<Sprite>();
 
    private bool confirmMenu = false;
    private bool confirmQuit = false;
   

    public GameGUI(Scene scene, SceneManager sceneManager, RenderWindow window, GameScene gameScene) : base(scene, sceneManager, window)
    {
        this.gameScene = gameScene;
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
        scoreText = MakeText("0", protoFont, 50, new Color(30, 30, 30),
                        Program.screenW - 10, 10, new Vector2f(1, -1));

        overlay = new RectangleShape(new Vector2f(Program.screenW, Program.screenH));
        overlay.FillColor = new Color(0, 0, 0, 150);
        overlay.Position = new Vector2f(0, 0);

        titleText = MakeText("INVADERS", dataFont, 150, white,
                        Program.screenW / 2f, 200);

        resumeText = MakeText("Resume", protoFont, 50, nearWhite,
                        Program.screenW / 2f, Program.screenH - 400);

        mainMenuText = MakeText("Main Menu", protoFont, 50, nearWhite,
                        Program.screenW / 2f, Program.screenH - 350);

        quitText = MakeText("Quit", protoFont, 50, nearWhite,
                        Program.screenW / 2f, Program.screenH - 300);
    }

    public void UpdateHealthBar(int currentHealth)
    {
        healthSprites.Clear();

        for (int i = 0; i < currentHealth; i++)
        {
            Sprite healthIcon = new Sprite(sprite);
            FloatRect bounds = healthIcon.GetLocalBounds();

            float x = 10 + i * (bounds.Width + 10);
            float y = 15;

            healthIcon.Position = new Vector2f(x, y);

            healthSprites.Add(healthIcon);
        }
    }

    public void UpdateScoreText(int currentScore)
    {
        scoreText.DisplayedString = currentScore.ToString();
        FloatRect textBounds = scoreText.GetLocalBounds();
        scoreText.Origin = new Vector2f(textBounds.Width, textBounds.Top);
        scoreText.Position = new Vector2f(Program.screenW - 10, 10);
    }

    public override void Update(float deltaTime)
    {
        scoreText.FillColor = gameScene.IsPaused() ? white : new Color(30, 30, 30);
        resumeText.FillColor = IsMouseOver(resumeText) ? lightGrey : nearWhite;
        mainMenuText.FillColor = IsMouseOver(mainMenuText) ? lightGrey : nearWhite;
        quitText.FillColor = IsMouseOver(quitText) ? lightGrey : nearWhite;

        if (confirmMenu)
        {
            if (!IsMouseOver(mainMenuText))
            {
                mainMenuText = MakeText("Main Menu", protoFont, 50, nearWhite, Program.screenW / 2f, Program.screenH - 350);
                confirmMenu = false;
            }
        }
        if (confirmQuit)
        {
            if (!IsMouseOver(quitText))
            {
                quitText = MakeText("Quit", protoFont, 50, nearWhite, Program.screenW / 2f, Program.screenH - 300);
                confirmQuit = false;
            }
        }
    }

    public override void Render(RenderTarget target)
    {
        if (gameScene.IsPaused())
        {
            target.Draw(overlay);
            target.Draw(titleText);
            target.Draw(resumeText);
            target.Draw(mainMenuText);
            target.Draw(quitText);
        }

        foreach (Sprite healthIcon in healthSprites)
        {
            target.Draw(healthIcon);
        }

        target.Draw(scoreText);


    }
    
    protected override void OnMousePressed(object? sender, MouseButtonEventArgs e)
    {
        if (IsMouseOver(resumeText))
        {
            gameScene.SetPaused(false);
        }
        else if (IsMouseOver(mainMenuText))
        {
            if (!confirmMenu)
            {
                mainMenuText = MakeText("Are you sure?", protoFont, 30, nearWhite,
                                Program.screenW / 2f, Program.screenH - 350);
                confirmMenu = true;
            }
            else
            {
                sceneManager.LoadScene(new MainMenuScene(scene, sceneManager, window));
            }
        }
        else if (IsMouseOver(quitText))
        {
            if (!confirmQuit)
            {
                quitText = MakeText("Are you sure?", protoFont, 30, nearWhite,
                                Program.screenW / 2f, Program.screenH - 300);
                confirmQuit = true;
            }
            else
            {
                window.Close();
            }
        }
        
    }
}