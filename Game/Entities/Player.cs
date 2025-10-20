using System.Numerics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static SFML.Window.Keyboard.Key;


namespace Invaders;

public class Player : Actor
{
    private GameManager gameManger;
    private Vector2f playerDirection = new Vector2f(0, -1);


    public Player(string tag, GameManager gameManager) : base("invaders", tag)
    {
        this.gameManger = gameManager;

        health = 3;
        maxHealth = health;
        damage = 3;
        speed = 400f;

        shootCooldown = 0.1f;
    }

    public override Vector2f MovementDirection
    {
        get => playerDirection;
        set
        {
            playerDirection = value;
        }
    }

    public override void Create(Scene scene)
    {
        sprite.TextureRect = new IntRect(0, 0, 100, 76);
        base.Create(scene);
        CollisionRadius = MathF.Max(sprite.TextureRect.Width, sprite.TextureRect.Height) * 0.4f;
    }

    public override void Update(Scene scene, float deltaTime)
    {
        base.Update(scene, deltaTime);

        Movement();
        Shooting(scene, deltaTime);
    }

    protected override void CollideWith(Scene scene, Entity other)
    {
        base.CollideWith(scene, other);
        gameManger.HealthUpdate(health);
    }

    public override void OnDestroy(Scene scene)
    {
        
    }

    protected override void Shooting(Scene scene, float deltaTime)
    {
        if (shootTimer > 0)
        {
            shootTimer -= deltaTime;
        }
        
        if(Keyboard.IsKeyPressed(Space) && shootTimer <= 0)
        {
            base.Shoot(scene, new Vector2f(-25, 10));
            base.Shoot(scene, new Vector2f(25, 10));
            //Play shooting sounds effect
            shootTimer = shootCooldown;
        }
    }

    private void Movement()
    {
        Vector2f inputDirection = new Vector2f(0, 0);

        // Horizontal Movement
        if (Keyboard.IsKeyPressed(Keyboard.Key.Left) && !Keyboard.IsKeyPressed(Keyboard.Key.Right))
        {
            inputDirection.X = -1;
        }
        else if (Keyboard.IsKeyPressed(Keyboard.Key.Right) && !Keyboard.IsKeyPressed(Keyboard.Key.Left))
        {
            inputDirection.X = 1;
        }

        // Vertical Movement
        if (Keyboard.IsKeyPressed(Keyboard.Key.Up) && !Keyboard.IsKeyPressed(Keyboard.Key.Down))
        {
            inputDirection.Y = -0.75f;
        }
        else if (Keyboard.IsKeyPressed(Keyboard.Key.Down) && !Keyboard.IsKeyPressed(Keyboard.Key.Up))
        {
            inputDirection.Y = 0.75f;
        }
        MovementDirection = inputDirection;

        if (Program.timeScale == 0)
        {
            return;
        }
        int frame = inputDirection.X < 0 ? 1 : inputDirection.X > 0 ? 2 : 0;
        sprite.TextureRect = new IntRect(100 * frame, 0, 100, 76);
    }

    protected override void ScreenBoundsCheck(Scene scene, float marginX, float marginY)
    {
        base.ScreenBoundsCheck(scene, 0.75f, 0.75f);
    }
    protected override void OnOutOfBounds(Scene scene, bool left, bool right, bool top, bool bottom)
    {
        if (left)
        {
            Position = new Vector2f(sprite.TextureRect.Width * 0.25f, Position.Y);
        }
        if (right)
        {
            Position = new Vector2f(Program.screenW - sprite.TextureRect.Width * 0.25f, Position.Y);
        }
        if(top)
        {
            Position = new Vector2f(Position.X, sprite.TextureRect.Height * 0.25f);
        }
        if(bottom)
        {
            Position = new Vector2f(Position.X, Program.screenH - sprite.TextureRect.Height * 0.25f);
        }
    }
}