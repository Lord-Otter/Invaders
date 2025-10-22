using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static SFML.Window.Keyboard.Key;


namespace Invaders;

public class Player : Actor
{
    private readonly GameManager gameManger;
    private Vector2f playerDirection = new Vector2f(0, -1);
    private float iFramesTimer = 0f;
    private float iFramesDuration = 3f;

    public Player(string tag, GameManager gameManager) : base("invaders", tag)
    {
        this.gameManger = gameManager;

        health = 3;
        damage = 3;
        speed = 400f;

        shootCooldown = 0.175f;
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
        collisionRadius = MathF.Max(sprite.TextureRect.Width, sprite.TextureRect.Height) * 0.4f;
        gameManger.HealthUpdate(health);
    }

    public override void Update(Scene scene, float deltaTime)
    {
        base.Update(scene, deltaTime);

        Movement();
        Shooting(scene, deltaTime);
        IFrames(deltaTime);
    }

    private void IFrames(float deltaTime)
    {
        if(iFramesTimer > 0f)
        {
            iFramesTimer -= deltaTime;
            if (iFramesTimer > 0f)
            {
                sprite.Color = new Color(255, 255, 255, 100);
            }
            else
            {
                sprite.Color = new Color(255, 255, 255, 255);
            }
        }
    }

    protected override void CollideWith(Scene scene, Entity other)
    {
        if(iFramesTimer > 0)
        {
            return;
        }
        base.CollideWith(scene, other);
        gameManger.HealthUpdate(health);
        
        iFramesTimer = iFramesDuration;
    }

    protected override void CollisionCheck(Scene scene)
    {
        base.CollisionCheck(scene);
    }

    protected override void Shooting(Scene scene, float deltaTime)
    {
        if (shootTimer > 0)
        {
            shootTimer -= deltaTime;
        }
        
        if(Keyboard.IsKeyPressed(Space) && shootTimer <= 0)
        {
            base.Shoot(scene, new Vector2f(-25, 20));
            base.Shoot(scene, new Vector2f(25, 20));
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