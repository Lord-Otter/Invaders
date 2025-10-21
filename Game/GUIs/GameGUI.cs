using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Invaders;
using System.Security.Cryptography.X509Certificates;
using System.Numerics;

namespace Invaders;

public class GameGUI : GUIBase
{
    private readonly SceneManager sceneManager;
    private readonly GameScene gameScene;
    private readonly Scene scene;
    private readonly RenderWindow window;

    // Fonts
    private Font titleFont = null!;
    private Font font = null!;

    // Texts
    private Text scoreText = null!;
    private Text titleText = null!;
    private Text resumeText = null!;
    private Text mainMenuText = null!;
    private Text quitText = null!;

    // Colors
    private Color titleColor = new Color(220, 220, 220);
    private Color defaultColor = new Color(200, 200, 200);
    private Color hoverColor = new Color(120, 120, 120);

    // Overlay
    private RectangleShape overlay = null!;

    // Health
    private Sprite sprite =  new Sprite();
    private List<Sprite> healthSprites = new List<Sprite>();
    private const float spacing = 10f;
 
    private bool confirmMenu = false;
    private bool confirmQuit = false;
   

    public GameGUI(Scene scene, SceneManager sceneManager, GameScene gameScene, RenderWindow window) : base(scene, window)
    {
        this.sceneManager = sceneManager;
        this.gameScene = gameScene;
        this.scene = scene;
        this.window = window;
    }

    public override void OnEnter()
    {
        font = scene.AssetManager.LoadFont("prototype");
        titleFont = scene.AssetManager.LoadFont("data-control");
        sprite.Texture = scene.AssetManager.LoadTexture("icon");

        CreateUI();

        window.MouseButtonPressed += OnMousePressed;
    }

    public override void OnExit()
    {
        window.MouseButtonPressed -= OnMousePressed;
    }

    private void CreateUI()
    {
        scoreText = MakeText("0", font, 50, new Color(30, 30, 30),
                        Program.screenW - 10, 10, new Vector2f(1, -1));

        overlay = new RectangleShape(new Vector2f(Program.screenW, Program.screenH));
        overlay.FillColor = new Color(0, 0, 0, 150);
        overlay.Position = new Vector2f(0, 0);

        titleText = MakeText("INVADERS", titleFont, 150, titleColor,
                        Program.screenW / 2f, 200);

        resumeText = MakeText("Resume", font, 50, defaultColor,
                        Program.screenW / 2f, Program.screenH - 400);

        mainMenuText = MakeText("Main Menu", font, 50, defaultColor,
                        Program.screenW / 2f, Program.screenH - 350);

        quitText = MakeText("Quit", font, 50, defaultColor,
                        Program.screenW / 2f, Program.screenH - 300);
    }

    public void UpdateHealthBar(int currentHealth)
    {
        healthSprites.Clear();

        for (int i = 0; i < currentHealth; i++)
        {
            Sprite healthIcon = new Sprite(sprite);
            FloatRect bounds = healthIcon.GetLocalBounds();

            float x = 10 + i * (bounds.Width + spacing);
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
        scoreText.FillColor = gameScene.IsPaused() ? titleColor : new Color(30, 30, 30);
        resumeText.FillColor = IsMouseOver(resumeText) ? hoverColor : defaultColor;
        mainMenuText.FillColor = IsMouseOver(mainMenuText) ? hoverColor : defaultColor;
        quitText.FillColor = IsMouseOver(quitText) ? hoverColor : defaultColor;

        if (confirmMenu)
        {
            if (!IsMouseOver(mainMenuText))
            {
                mainMenuText = MakeText("Main Menu", font, 50, defaultColor, Program.screenW / 2f, Program.screenH - 350);
                confirmMenu = false;
            }
        }
        if (confirmQuit)
        {
            if (!IsMouseOver(quitText))
            {
                quitText = MakeText("Quit", font, 50, defaultColor, Program.screenW / 2f, Program.screenH - 300);
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
    
    private void OnMousePressed(object? sender, MouseButtonEventArgs e)
    {
        if (IsMouseOver(resumeText))
        {
            gameScene.SetPaused(false);
        }
        else if (IsMouseOver(mainMenuText))
        {
            if (!confirmMenu)
            {
                mainMenuText = MakeText("Are you sure?", font, 30, defaultColor,
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
                quitText = MakeText("Are you sure?", font, 30, defaultColor,
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