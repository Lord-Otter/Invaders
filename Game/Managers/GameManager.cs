using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

public class GameManager
{
    private readonly SceneManager sceneManager;
    private readonly RenderWindow window;
    private readonly Scene scene;
    private readonly GameGUI gui;

    private float elapsedTime;
    private float minSpawnCooldown = 3;
    private float maxSpawnCooldown = 6;
    public int enemyShipCounter;
    public int enemyUFOCounter;

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
        scene.Clear();
        scene.Spawn(new Player("Ally", this), new Vector2f(Program.screenW / 2f, Program.screenH - 100));
    }

    public void Update(float deltaTime)
    {
        elapsedTime += deltaTime;
        Spawner(deltaTime);
    }

    public void HealthUpdate(int currentHealth)
    {
        
    }

    private void Spawner(float deltaTime)
    {
        enemySpawnTimer -= deltaTime;
        if(enemySpawnTimer <= 0f)
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