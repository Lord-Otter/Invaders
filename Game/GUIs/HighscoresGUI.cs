using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Text.Json;

namespace Invaders;

public class HighscoresGUI : GUIBase
{
    // Texts
    private Text titleText = null!;
    private Text backText = null!;
    private Text rightArrow = null!;
    private Text leftArrow = null!;
    private Text pageNumberText = null!;
    private Text noScoresText = null!;

    // Misc.
    private readonly List<Text> scoreTexts = new List<Text>();
    private record HighscoreEntry(string Name, int Score);
    private int currentPage = 1;
    private int maxPages = 0;

    public HighscoresGUI(Scene scene, SceneManager sceneManager, RenderWindow window) : base(scene, sceneManager, window)
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
        ScoreList(currentPage, out maxPages);
        string pageText = $"{currentPage}/{maxPages}";

        titleText = MakeText("Highscores!", protoFont, 75, black,
                        Program.screenW / 2f, 200);

        pageNumberText = MakeText(pageText, protoFont, 50, black,
                        Program.screenW / 2f, Program.screenH - 200);

        rightArrow = MakeText(">", dataFont, 75, darkGrey,
                        Program.screenW / 2f + 100, pageNumberText.Position.Y, new Vector2f(-1, 0));

        leftArrow = MakeText("<", dataFont, 75, darkGrey,
                        Program.screenW / 2f - 100, pageNumberText.Position.Y, new Vector2f(1, 0));

        backText = MakeText("Back", protoFont, 50, darkGrey,
                        Program.screenW / 2f, Program.screenH - 100);     
    }

    private void ScoreList(int pageNumber, out int maxPages)
    {
        scoreTexts.Clear();

        const string FilePath = "assets/highscores.json";

        if (!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0)
        {
            noScoresText = MakeText("List is empty!", protoFont, 50, nearWhite,
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

                Text nameText = MakeText(entry.Name, protoFont, 50, black,
                        Program.screenW / 2f - 250f, 300 + (i - (10 * (pageNumber - 1)) - 1) * 50, new Vector2f(-1, 0));
                scoreTexts.Add(nameText);

                Text rankText = MakeText($"{i}.", protoFont, 50, black,
                        nameText.Position.X - 15, nameText.Position.Y, new Vector2f(1, 0));
                scoreTexts.Add(rankText);

                Text scoreText = MakeText(entry.Score.ToString(), protoFont, 50, black,
                        Program.screenW / 2f + 250, nameText.Position.Y, new Vector2f(1, 0));
                scoreTexts.Add(scoreText);
            }
            catch
            {

            }
            string pageText = $"{currentPage}/{maxPages}";
            pageNumberText = MakeText(pageText, protoFont, 50, black,
                    Program.screenW / 2f, Program.screenH - 200);
        }
    }

    public override void Update(float deltaTime)
    {
        backText.FillColor = IsMouseOver(backText) ? nearWhite : darkGrey;

        if (currentPage == maxPages)
        {
            rightArrow.FillColor = purpleish;
        }
        else
        {
            rightArrow.FillColor = IsMouseOver(rightArrow) ? nearWhite : darkGrey;
        }

        if (currentPage == 1)
        {
            leftArrow.FillColor = purpleish;
        }
        else
        {
            leftArrow.FillColor = IsMouseOver(leftArrow) ? nearWhite : darkGrey;
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