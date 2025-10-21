using SFML.Graphics;
using SFML.Window;
using System.Text.Json;
using SFML.System;
using Invaders;
using System.Numerics;
using System.Formats.Asn1;


namespace Invaders;

public class GameOverGUI : GUIBase
{
    private readonly SceneManager sceneManager;

    // Fonts
    private Font font = null!;

    // Texts
    private Text gameOverText = null!;
    private Text yourScoreText = null!;
    private Text submitText = null!;
    private Text warningText = null!;
    private Text retryText = null!;
    private Text mainMenuText = null!;
    private Text quitText = null!;

    // TextBox
    private RectangleShape nameBox = null!;
    private Text boxText = null!;

    // Colors
    private Color textColor = new Color(10, 10, 10);
    private Color defaultColor = new Color(30, 30, 30);
    private Color hoverColor = new Color(200, 200, 200);

    // Misc.
    private bool confirmQuit = false;
    private bool isNameBoxActive = false;
    private int charLimit = 8;
    private string playerName = "";

    private record HighscoreEntry(string Name, int Score);
    private List<(string Name, int Score)> highscores = new List<(string Name, int Score)>();
        
    private int playerScore;

    public GameOverGUI(Scene scene, SceneManager sceneManager, RenderWindow window, int playerScore) : base(scene, window)
    {
        this.sceneManager = sceneManager;
        this.playerScore = playerScore;
    }

    public override void OnEnter()
    {
        font = scene.AssetManager.LoadFont("prototype");

        CreateTexts();
        window.MouseButtonPressed += OnMousePressed;
        window.TextEntered += OnTextEntered;
    }

    public override void OnExit()
    {
        window.MouseButtonPressed -= OnMousePressed;
        window.TextEntered -= OnTextEntered;
    }

    private void CreateTexts()
    {
        gameOverText = MakeText("Game Over", font, 75, textColor,
                        Program.screenW / 2f, 225);

        yourScoreText = MakeText($"Your Score: {playerScore}", font, 50, textColor,
                        Program.screenW / 2f, 300);

        if (playerScore > 0)
        {
            MakeBox(new Color(200, 200, 200), new Color(30, 30, 30));

            boxText = MakeText("Enter Name", font, 30, new Color(50, 50, 50),
                        Program.screenW / 2f, nameBox.Position.Y);

            submitText = MakeText("Submit", font, 50, defaultColor,
                            Program.screenW / 2f, nameBox.Position.Y + 80);
        }
        else
        {
            MakeBox(new Color(70, 70, 70), new Color(40, 40, 40));

            boxText = MakeText("", font, 30, new Color(50, 50, 50),
                        Program.screenW / 2f, nameBox.Position.Y);

            submitText = MakeText("No score to submit", font, 50, textColor,
                            Program.screenW / 2f, nameBox.Position.Y + 80);
        }

        warningText = MakeText("", font, 30, new Color(0, 0, 0, 150),
                        Program.screenW / 2f, Program.screenH / 2f + 20);

        retryText = MakeText("Retry", font, 50, defaultColor,
                        Program.screenW / 2f, Program.screenH - 300);

        mainMenuText = MakeText("Main Menu", font, 50, defaultColor,
                        Program.screenW / 2f, Program.screenH - 250);

        quitText = MakeText("Quit", font, 50, defaultColor,
                        Program.screenW / 2f, Program.screenH - 200);
    }

    private void MakeBox(Color fillColor, Color outlineColor)
    {
        nameBox = new RectangleShape(new Vector2f(400, 70));
        nameBox.FillColor = fillColor;
        nameBox.OutlineColor = outlineColor;
        nameBox.OutlineThickness = -10;
        FloatRect bounds = nameBox.GetLocalBounds();
        nameBox.Origin = new Vector2f(bounds.Left + bounds.Width / 2f, bounds.Top + bounds.Height / 2f);
        nameBox.Position = new Vector2f(Program.screenW / 2f, Program.screenH / 2f - 50);
    }

    private void OnTextEntered(object? sender, TextEventArgs e)
    {
        if (!isNameBoxActive)
        {
            return;
        }

        if (e.Unicode == "\b")
        {
            if (playerName.Length > 0)
            {
                playerName = playerName[..^1];
            }
        }
        else if (e.Unicode.Length == 1 && playerName.Length < charLimit)
        {
            char c = e.Unicode[0];
            if (char.IsLetterOrDigit(c) || c == ' ')
            {
                playerName += c;
            }
        }

        boxText.DisplayedString = string.IsNullOrEmpty(playerName) ? "Enter Name" : playerName;
    }

    private void SaveScore()
    {
        const string FilePath = "assets/highscores.json";

        string name = string.IsNullOrWhiteSpace(playerName) ? "Anonymous" : playerName;
        var newEntry = new HighscoreEntry(name, playerScore);
        string json = JsonSerializer.Serialize(newEntry);

        File.AppendAllText(FilePath, json + Environment.NewLine);
    }

    private void SortList()
    {
        const string FilePath = "assets/highscores.json";
        highscores.Clear();

        if (!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0)
        {
            return;
        }

        try
        {
            string[] lines = File.ReadAllLines(FilePath);

            List<HighscoreEntry> parsedEntries = new List<HighscoreEntry>();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                try
                {
                    HighscoreEntry? entry = JsonSerializer.Deserialize<HighscoreEntry>(line);
                    if (entry != null)
                    {
                        parsedEntries.Add(entry);
                    }
                }
                catch
                {

                }
            }

            List<HighscoreEntry> sorted = parsedEntries.OrderByDescending(e => e.Score).ThenBy(e => e.Name).ToList();

            foreach (HighscoreEntry entry in sorted)
            {
                highscores.Add((entry.Name, entry.Score));
            }

            List<string> jsonLines = sorted.Select(entry => JsonSerializer.Serialize(entry)).ToList();

            File.WriteAllLines(FilePath, jsonLines);
        }
        catch
        {
            
        }
    }

    public override void Update(float deltaTime)
    {
        retryText.FillColor = IsMouseOver(retryText) ? hoverColor : defaultColor;
        mainMenuText.FillColor = IsMouseOver(mainMenuText) ? hoverColor : defaultColor;
        quitText.FillColor = IsMouseOver(quitText) ? hoverColor : defaultColor;

        if (playerScore > 0)
        {
            submitText.FillColor = IsMouseOver(submitText) ? hoverColor : defaultColor;
            nameBox.FillColor = isNameBoxActive ? new Color(200, 200, 200) : new Color(150, 150, 150);
            nameBox.OutlineColor = isNameBoxActive ? new Color(30, 30, 30) : new Color(40, 40, 40);
        }

        if (confirmQuit)
        {
            if (!IsMouseOver(quitText))
            {
                quitText = MakeText("Quit", font, 50, defaultColor, Program.screenW / 2f, Program.screenH - 200);
                confirmQuit = false;
            }
        }
    }

    public override void Render(RenderTarget target)
    {
        target.Draw(gameOverText);
        target.Draw(yourScoreText);

        target.Draw(nameBox);
        target.Draw(boxText);
        target.Draw(warningText);
        target.Draw(submitText);

        target.Draw(retryText);
        target.Draw(mainMenuText);
        target.Draw(quitText);
    }

    protected override void OnMousePressed(object? sender, MouseButtonEventArgs e)
    {
        Vector2f mousePosition = window.MapPixelToCoords(Mouse.GetPosition(window));
        isNameBoxActive = nameBox.GetGlobalBounds().Contains(mousePosition.X, mousePosition.Y);

        if (IsMouseOver(submitText) && playerScore > 0)
        {
            if (!string.IsNullOrWhiteSpace(playerName))
            {
                SaveScore();
                SortList();

                warningText = MakeText("Score Saved!", font, 15, new Color(40, 40, 40),
                                Program.screenW / 2f, nameBox.Position.Y + 45);
            }
            else
            {
                warningText = MakeText("Enter a name to save score!", font, 15, new Color(40, 40, 40),
                                Program.screenW / 2f, nameBox.Position.Y + 45);
            }
        }
        else if (IsMouseOver(retryText))
        {
            sceneManager.LoadScene(new GameScene(scene, sceneManager, window));
        }
        else if (IsMouseOver(mainMenuText))
        {
            sceneManager.LoadScene(new MainMenuScene(scene, sceneManager, window));
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
        }
    }
}