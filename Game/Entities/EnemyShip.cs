using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

public class EnemyShip : Actor
{
    private GameManager gameManager;

    private static readonly Random rngShip = new Random();
    private static readonly Random rngShoot = new Random();
    private static readonly Random rng = new Random();
    protected Vector2f movementDirection = new Vector2f(0f, -1f);
    private float minShootCooldown = 1f;
    private float maxShootCooldown = 3f;

    public EnemyShip(string tag, GameManager gameManager) : base("invaders", tag)
    {
        this.gameManager = gameManager;

        health = 40;
        maxHealth = health;
        pointValue = 100;
        damage = 1;
        speed = 125f;

        shootCooldown = minShootCooldown;


        int randomValue = rng.Next(2) == 0 ? -1 : 1;
        MovementDirection = new Vector2f(randomValue, 1);
        FacingDirection = MovementDirection;
    }
    
    public override Vector2f MovementDirection
    {
        get => movementDirection;
        set
        {
            movementDirection = value;
        }
    }

    public override void Create(Scene scene)
    {
        sprite.TextureRect = new IntRect(300 + (100 * rngShip.Next(0, 3)), 0, 100, 80);
        base.Create(scene);
    }

    public override void Update(Scene scene, float deltaTime)
    {
        base.Update(scene, deltaTime);
        Shooting(scene, deltaTime);
    }

    public override void OnDestroy(Scene scene)
    {
        scene.Spawn(new Explosion(this));
        gameManager.enemyShipCounter--;
        gameManager.enemiesKilledCounter++;
        gameManager.PointUpdate(pointValue);
        Console.WriteLine($"--{gameManager.enemyShipCounter}");
        Console.WriteLine($"++{gameManager.enemiesKilledCounter}");
    }

    protected override void Shooting(Scene scene, float deltaTime)
    {
        base.Shooting(scene, deltaTime);
        if (shootTimer <= 0)
        {
            base.Shoot(scene, new Vector2f(0, 40));
            shootTimer = (float)(rngShoot.NextDouble() * (maxShootCooldown - minShootCooldown) + minShootCooldown);
        }
    }

    private void ChangeDirection(int flipX)
    {
        MovementDirection = new Vector2f(flipX, 1);
        FacingDirection = MovementDirection;
    }

    private void WrapAround()
    {
        Position = new Vector2f(Position.X, -sprite.TextureRect.Height * 2);
        MovementDirection = new Vector2f(MovementDirection.X, 1);
        FacingDirection = MovementDirection;
    }

    protected override void CollideWith(Scene scene, Entity other)
    {
        base.CollideWith(scene, other);
    }
    
    protected override void ScreenBoundsCheck(Scene scene, float marginX, float marginY)
    {
        marginX = 1;
        marginY = -0.5f;
        base.ScreenBoundsCheck(scene, marginX, marginY);
    }
    protected override void OnOutOfBounds(Scene scene, bool left, bool right, bool top, bool bottom)
    {
        if (left && MovementDirection.X < 0)
        {
            ChangeDirection(1);
        }
        if (right && MovementDirection.X > 0)
        {
            ChangeDirection(-1);
        }
        if (bottom && MovementDirection.Y > 0)
        {
            WrapAround();
        }
    }
}