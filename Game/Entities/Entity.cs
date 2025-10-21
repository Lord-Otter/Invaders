using System.ComponentModel;
using System.Net;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

public abstract class Entity
{
    public string tag { get; set; }
    protected Sprite sprite = new Sprite();
    private string textureName;
    protected int health = 1, maxHealth = 1;
    protected int pointValue = 0;
    protected int damageTakenBuffer;
    protected int damage;
    public bool isDead;

    protected float speed;
    private Vector2f movementDirection = new Vector2f(0f, -1f);
    private Vector2f facingDirection = new Vector2f(0f, -1f);

    public float CollisionRadius { get; protected set; }
    protected Vector2f collisionOffset = new Vector2f(0, 0);

    protected Entity(string textureName, string tag)
    {
        this.textureName = textureName;
        this.tag = tag;
    }

    public virtual Vector2f MovementDirection
    {
        get => movementDirection;
        set
        {
            if (value.X == 0f && value.Y == 0f)
            {
                movementDirection = new Vector2f(0f, 0f);
                return;
            }
        
            float length = MathF.Sqrt(value.X * value.X + value.Y * value.Y);
            movementDirection = new Vector2f(value.X / length, value.Y / length);
        }
    }

    public Vector2f FacingDirection
    {
        get => facingDirection;
        set
        {
            if (value.X == 0f && value.Y == 0f)
            {
                return;
            }
            facingDirection = value;

            float angle = MathF.Atan2(facingDirection.Y, facingDirection.X) * 180f / MathF.PI;
            sprite.Rotation = angle + 90f;
        }
    }
    
    public Vector2f Position
    {
        get => sprite.Position;
        set => sprite.Position = value;
    }

    public Vector2f CollisionCenter => Position + collisionOffset;

    public virtual FloatRect Bounds => sprite.GetGlobalBounds();

    public virtual void Create(Scene scene)
    {
        sprite.Texture = scene.AssetManager.LoadTexture(textureName);
        sprite.Origin = new Vector2f(sprite.TextureRect.Width / 2f, sprite.TextureRect.Height / 2f);
        CollisionRadius = MathF.Max(sprite.TextureRect.Width, sprite.TextureRect.Height) * 0.5f;
    }

    public virtual void OnDestroy(Scene scene) { }

    public virtual void Update(Scene scene, float deltaTime)
    {
        IsAliveCheck();
        CollisionCheck(scene);
        ScreenBoundsCheck(scene);
        Position += MovementDirection * speed * deltaTime;
    }
    
    public virtual void IsAliveCheck()
    {
        if (damageTakenBuffer > 0)
        {
            health -= damageTakenBuffer;
            damageTakenBuffer = 0;
        }

        if (health <= 0)
        {
            isDead = true;
        }
    }

    public virtual void DrawDebugHitbox(RenderTarget target)
    {
        CircleShape shape = new CircleShape(CollisionRadius);
        shape.Origin = new Vector2f(CollisionRadius, CollisionRadius);
        shape.Position = CollisionCenter;
        shape.FillColor = Color.Transparent;
        shape.OutlineColor = Color.Red;
        shape.OutlineThickness = 1f;
        target.Draw(shape);
    }

    public virtual void Render(RenderTarget target)
    {
        target.Draw(sprite);

        //DrawDebugHitbox(target);
    }

    protected virtual void CollisionCheck(Scene scene)
    {
        foreach (Entity other in scene.FindIntersects(this))
        {
            CollideWith(scene, other);
        }
    }

    protected virtual void CollideWith(Scene scene, Entity other)
    {
        health -= other.damage;
    }

    protected virtual void ScreenBoundsCheck(Scene scene, float marginX = 0, float marginY = 0)
    {
        FloatRect bounds = new FloatRect(0, 0, Program.screenW, Program.screenH);
        FloatRect entityBounds = sprite.GetLocalBounds();

        bool left = (Position.X + entityBounds.Width / 2f) - (entityBounds.Width * marginX) < bounds.Left;
        bool right = (Position.X - entityBounds.Width / 2f) + (entityBounds.Width * marginX) > bounds.Left + bounds.Width;
        bool top = (Position.Y + entityBounds.Height / 2f) - (entityBounds.Height * marginY) < bounds.Top;
        bool bottom = (Position.Y - entityBounds.Height / 2f) + (entityBounds.Height * marginY) > bounds.Top + bounds.Height;

        if (left || right || top || bottom)
        {
            OnOutOfBounds(scene, left, right, top, bottom);
        }
    }

    protected virtual void OnOutOfBounds(Scene scene, bool left, bool right, bool top, bool bottom)
    {
        isDead = true;
    }
}
