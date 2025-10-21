using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

public class GameManager
{
    private readonly SceneManager sceneManager;
    private readonly RenderWindow window;
    private readonly Scene scene;
    public readonly GameGUI gui;

    private float elapsedTime;
    private float minSpawnCooldown;
    private float maxSpawnCooldown;
    public int enemyShipCounter;
    public int enemyUFOCounter;
    public int enemiesKilledCounter;
    private int enemyCap = 20;
    private int currentScore;
    private float scoreTimer;

    private readonly Random rng = new Random();
    private float enemySpawnTimer = 1f;

    public GameManager(Scene scene, SceneManager sceneManager, RenderWindow window, GameGUI gui)
    {
        this.sceneManager = sceneManager;
        this.window = window;
        this.scene = scene;
        this.gui = gui;
    }
    public void Initialize()
    {
        elapsedTime = 0;
        scoreTimer = 0;
        currentScore = 0;
        enemyShipCounter = 0;
        enemyUFOCounter = 0;
        minSpawnCooldown = 2;
        maxSpawnCooldown = 5;
        enemiesKilledCounter = 0;

        scene.Clear();
        scene.Spawn(new Player("Ally", this), new Vector2f(Program.screenW / 2f, Program.screenH - 100));
    }

    public void Update(float deltaTime)
    {
        elapsedTime += deltaTime;
        IncScoreTimer(deltaTime);
        UpdateSpawnRate();
        Spawner(deltaTime);
    }

    private void UpdateSpawnRate()
    {
        float difficultyFactor = enemiesKilledCounter / 5f;

        minSpawnCooldown = MathF.Max(2 - difficultyFactor * 0.3f, 0.5f);
        maxSpawnCooldown = MathF.Max(5 - difficultyFactor * 0.3f, 1f);
    }

    private void IncScoreTimer(float deltaTime)
    {
        scoreTimer += deltaTime;

        if(scoreTimer >= 1f)
        {
            int secondsPassed = (int)scoreTimer;
            currentScore += 10 * secondsPassed;
            gui.UpdateScoreText(currentScore);
            scoreTimer -= secondsPassed;
        }
    }

    public void HealthUpdate(int currentHealth)
    {
        if(currentHealth <= 0)
        {
            sceneManager.LoadScene(new GameOverScene(scene, sceneManager, window, currentScore));
        }
        gui.UpdateHealthBar(currentHealth);
    }

    public void PointUpdate(int pointValue)
    {
        currentScore += pointValue;
        gui.UpdateScoreText(currentScore);
    }

    private void Spawner(float deltaTime)
    {
        enemySpawnTimer -= deltaTime;
        if(enemySpawnTimer <= 0f && (enemyShipCounter + enemyUFOCounter < enemyCap))
        {
            SpawnEnemyShip();
            enemySpawnTimer = (float)(new Random().NextDouble() * (maxSpawnCooldown - minSpawnCooldown) + minSpawnCooldown);
        }
    }
    
    private void SpawnEnemyShip()
    {
        int randomX = new Random().Next(0, Program.screenW);

        scene.Spawn(new EnemyShip("Enemy", this), new Vector2f(randomX, -100));
        enemyShipCounter++;
        Console.WriteLine($"++{enemyShipCounter}");
    }
}