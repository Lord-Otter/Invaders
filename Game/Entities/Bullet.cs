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
        speed = 1000f;
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
            speed = 500;     
        }
        sprite.TextureRect = new IntRect(690 + (9 * offset), 0, 9, 36);
        base.Create(scene);
        sprite.Origin = new Vector2f(sprite.TextureRect.Width / 2f, sprite.TextureRect.Height * 0.5f);
        CollisionRadius = MathF.Max(sprite.TextureRect.Width, sprite.TextureRect.Height / 5) * 0.5f;
        if(tag == "Ally")
        {
            collisionOffset = new Vector2f(0, sprite.TextureRect.Height * rng.Next(1, 10) / 10f);
        }
    }

    protected override void CollideWith(Scene scene, Entity other)
    {
        if (other is Bullet)
        {
            return;
        }
        base.CollideWith(scene, other);
    }
}