using System.Diagnostics;
using System.Reflection;
using Invaders;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

public class Bullet : Entity
{
    private Random rng = new Random();

    public Bullet(Entity source, int damage) : base("invaders", source.tag)
    {
        speed = 750f;
        this.damage = damage;
        FacingDirection = source.FacingDirection;
        MovementDirection = source.FacingDirection;
        Position = source.Position;
    }

    public override void Create(Scene scene)
    {
        int offset = tag == "Ally" ? 0 : 1;
        if (tag != "Ally")
        {
            speed = 600;
        }
        sprite.TextureRect = new IntRect(690 + (9 * offset), 0, 9, 36);
        base.Create(scene);
        sprite.Origin = new Vector2f(sprite.TextureRect.Width / 2f, sprite.TextureRect.Height * 0.1f);
        CollisionRadius = MathF.Max(sprite.TextureRect.Width, sprite.TextureRect.Height / 5) * 0.5f;
        if (tag == "Ally")
        {
            collisionOffset = new Vector2f(0, sprite.TextureRect.Height * rng.Next(1, 10) / 10f);
        }
    }

    public override void OnDestroy(Scene scene)
    {
        scene.Spawn(new HitEffect(this));
    }

    protected override void CollideWith(Scene scene, Entity other)
    {
        if (other is Bullet)
        {
            return;
        }
        base.CollideWith(scene, other);
    }

    protected override void ScreenBoundsCheck(Scene scene, float marginX = 0, float marginY = 0)
    {
        base.ScreenBoundsCheck(scene, -1.75f, -1.75f);
    }
}