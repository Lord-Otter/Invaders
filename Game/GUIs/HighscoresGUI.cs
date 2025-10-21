using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Text.Json;
using Invaders;
using System.Runtime.InteropServices;

namespace Invaders;

public class HighscoresGUI : GUIBase
{
    private readonly SceneManager sceneManager;

    // Fonts
    private Font font = null!;
    private Font arrowFont = null!;

    // Texts
    private Text titleText = null!;
    private Text backText = null!;
    private Text rightArrow = null!;
    private Text leftArrow = null!;
    private Text pageNumberText = null!;
    private Text noScoresText = null!;

    // Colors
    private Color textColor = new Color(10, 10, 10);
    private Color buttonColor = new Color(30, 30, 30);
    private Color hoverColor = new Color(200, 200, 200);
    private Color greyedOutColor = new Color(68, 50, 76);

    // Misc.
    private List<Text> scoreTexts = new List<Text>();
    private record HighscoreEntry(string Name, int Score);
    private int currentPage = 1;
    private int maxPages = 0;

    public HighscoresGUI(Scene scene, SceneManager sceneManager, RenderWindow window) : base(scene, window)
    {
        this.sceneManager = sceneManager;
    }

    public override void OnEnter()
    {
        font = scene.AssetManager.LoadFont("prototype");
        arrowFont = scene.AssetManager.LoadFont("data-control");
        CreateTexts();
        window.MouseButtonPressed += OnMousePressed;
    }

    public override void OnExit()
    {
        window.MouseButtonPressed -= OnMousePressed;
    }

    private void CreateTexts()
    {
        ScoreList(currentPage, out maxPages);
        string pageText = $"{currentPage}/{maxPages}";

        titleText = MakeText("Highscores!", font, 75, textColor,
                        Program.screenW / 2f, 200);

        pageNumberText = MakeText(pageText, font, 50, textColor,
                        Program.screenW / 2f, Program.screenH - 200);

        rightArrow = MakeText(">", arrowFont, 75, buttonColor,
                        Program.screenW / 2f + 100, pageNumberText.Position.Y, new Vector2f(-1, 0));

        leftArrow = MakeText("<", arrowFont, 75, buttonColor,
                        Program.screenW / 2f - 100, pageNumberText.Position.Y, new Vector2f(1, 0));

        backText = MakeText("Back", font, 50, buttonColor,
                        Program.screenW / 2f, Program.screenH - 100);     
    }

    private void ScoreList(int pageNumber, out int maxPages)
    {
        scoreTexts.Clear();

        const string FilePath = "assets/highscores.json";

        if (!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0)
        {
            noScoresText = MakeText("List is empty!", font, 50, hoverColor,
                    Program.screenW / 2f, Program.screenH / 2f);

            scoreTexts.Add(noScoresText);
            maxPages = 1;
            return;
        }

        string[] lines = File.ReadAllLines(FilePath);

        maxPages = (int)Math.Ceiling((double)lines.Length / 10);

        for (int i = 1 + (10 * (pageNumber - 1)); i <= lines.Length && i <= 10 * pageNumber; i++)
        {
            try
            {
                HighscoreEntry? entry = JsonSerializer.Deserialize<HighscoreEntry>(lines[i - 1]);
                if (entry == null)
                {
                    continue;
                }

                Text nameText = MakeText(entry.Name, font, 50, textColor,
                        Program.screenW / 2f - 250f, 300 + (i - (10 * (pageNumber - 1)) - 1) * 50, new Vector2f(-1, 0));
                scoreTexts.Add(nameText);

                Text rankText = MakeText($"{i}.", font, 50, textColor,
                        nameText.Position.X - 15, nameText.Position.Y, new Vector2f(1, 0));
                scoreTexts.Add(rankText);

                Text scoreText = MakeText(entry.Score.ToString(), font, 50, textColor,
                        Program.screenW / 2f + 250, nameText.Position.Y, new Vector2f(1, 0));
                scoreTexts.Add(scoreText);
            }
            catch
            {

            }
            string pageText = $"{currentPage}/{maxPages}";
            pageNumberText = MakeText(pageText, font, 50, textColor,
                    Program.screenW / 2f, Program.screenH - 200);
        }
    }

    public override void Update(float deltaTime)
    {
        backText.FillColor = IsMouseOver(backText) ? hoverColor : buttonColor;

        if (currentPage == maxPages)
        {
            rightArrow.FillColor = greyedOutColor;
        }
        else
        {
            rightArrow.FillColor = IsMouseOver(rightArrow) ? hoverColor : buttonColor;
        }

        if (currentPage == 1)
        {
            leftArrow.FillColor = greyedOutColor;
        }
        else
        {
            leftArrow.FillColor = IsMouseOver(leftArrow) ? hoverColor : buttonColor;
        }
    }

    public override void Render(RenderTarget target)
    {
        target.Draw(titleText);
        foreach (Text scoreText in scoreTexts)
        {
            target.Draw(scoreText);
        }
        target.Draw(pageNumberText);
        target.Draw(rightArrow);
        target.Draw(leftArrow);
        target.Draw(backText);
    }

    protected override void OnMousePressed(object? sender, MouseButtonEventArgs e)
    {
        if (IsMouseOver(backText))
        {
            sceneManager.LoadScene(new MainMenuScene(scene, sceneManager, window));
        }
        else if (IsMouseOver(rightArrow) && currentPage < maxPages)
        {
            currentPage++;
            ScoreList(currentPage, out maxPages);
        }
        else if (IsMouseOver(leftArrow) && currentPage > 1)
        {
            currentPage--;
            ScoreList(currentPage, out maxPages);
        }

    }
}