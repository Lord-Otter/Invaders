using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Text.Json;

namespace Invaders;

public class HighscoresScene : SceneBase
{
    private readonly SceneManager sceneManager;
    private readonly RenderWindow window;

    private Font font = null!;

    private Text titleText = null!;
    private Text backText = null!;

    private List<Text> scoreTexts = new List<Text>();

    private record HighscoreEntry(string Name, int Score);

    public HighscoresScene(Scene scene, SceneManager sceneManager, RenderWindow window) : base(scene)
    {
        this.sceneManager = sceneManager;
        this.window = window;
    }

    #region Text
    private void TitleText()
    {
        titleText = new Text("Highscores!", font, 75);
        titleText.FillColor = new Color(10, 10, 10);
        FloatRect bounds = titleText.GetLocalBounds();
        titleText.Origin = new Vector2f(bounds.Left + bounds.Width / 2f, bounds.Top + bounds.Height / 2f);
        titleText.Position = new Vector2f(Program.screenW / 2f, 200);
    }

    private void BackText()
    {
        backText = new Text("Back", font, 50);
        backText.FillColor = new Color(30, 30, 30);
        FloatRect bounds = backText.GetLocalBounds();
        backText.Origin = new Vector2f(bounds.Left + bounds.Width / 2f, bounds.Top + bounds.Height / 2f);
        backText.Position = new Vector2f(Program.screenW / 2f, Program.screenH - 200);
    }

    private void ScoreList()
    {
        scoreTexts.Clear();

        const string FilePath = "assets/highscores.json";

        if (!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0)
        {
            Text noScoresText = new Text("List is empty!", font, 50);
            noScoresText.FillColor = new Color(200, 200, 200);
            FloatRect bounds = noScoresText.GetLocalBounds();
            noScoresText.Origin = new Vector2f(bounds.Left + bounds.Width / 2f, bounds.Top + bounds.Height / 2f);
            noScoresText.Position = new Vector2f(Program.screenW / 2f, Program.screenH / 2f);

            scoreTexts.Add(noScoresText);
            return;
        }

        string[] lines = File.ReadAllLines(FilePath);

        float lineSpacing = 50f;

        for (int i = 0; i < lines.Length && i < 10; i++)
        {
            try
            {
                HighscoreEntry? entry = JsonSerializer.Deserialize<HighscoreEntry>(lines[i]);
                if (entry == null)
                {
                    continue;
                }



                float y = 300 + i * lineSpacing;

                Text nameText = new Text(entry.Name, font, 50);
                nameText.FillColor = new Color(10, 10, 10);
                FloatRect nameBounds = nameText.GetLocalBounds();
                nameText.Origin = new Vector2f(nameBounds.Left, nameBounds.Top + nameBounds.Height / 2f);
                nameText.Position = new Vector2f(250f, 300 + i * 50);
                scoreTexts.Add(nameText);

                Text rankText = new Text($"{i + 1}.", font, 50);
                rankText.FillColor = new Color(10, 10, 10);
                FloatRect rankBounds = rankText.GetLocalBounds();
                rankText.Origin = new Vector2f(rankBounds.Left + rankBounds.Width, rankBounds.Top + rankBounds.Height / 2f);
                rankText.Position = new Vector2f(nameText.Position.X - 15, nameText.Position.Y);
                scoreTexts.Add(rankText);

                Text scoreText = new Text(entry.Score.ToString(), font, 50);
                scoreText.FillColor = new Color(10, 10, 10);
                FloatRect scoreBounds = scoreText.GetLocalBounds();
                scoreText.Origin = new Vector2f(scoreBounds.Left + scoreBounds.Width, scoreBounds.Top + scoreBounds.Height / 2f);
                scoreText.Position = new Vector2f(Program.screenW - 225f, nameText.Position.Y);
                scoreTexts.Add(scoreText);
            }
            catch
            {
                
            }

        }
    }
    #endregion

    public override void OnEnter()
    {
        font = Scene.AssetManager.LoadFont("prototype");
        TitleText();
        ScoreList();
        BackText();

        window.MouseButtonPressed += OnMousePressed;
    }

    public override void OnExit()
    {
        window.MouseButtonPressed -= OnMousePressed;
    }

    public override void Update(float deltaTime)
    {
        backText.FillColor = IsMouseOver(backText) ? new Color(200, 200, 200) : new Color(30, 30, 30);
    }

    public override void Render(RenderTarget target)
    {
        target.Draw(titleText);
        foreach(Text scoreText in scoreTexts)
        {
            target.Draw(scoreText);
        }
        target.Draw(backText);
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

        if (IsMouseOver(backText))
        {
            sceneManager.LoadScene(new MainMenuScene(Scene, sceneManager, window));
        }
    }
}