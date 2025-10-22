using SFML.Graphics;
using SFML.System;


namespace Invaders;

public class HitEffect : Entity
{
    private readonly Entity source;
    private float lifeTime = 0.1f;
    public HitEffect(Entity source) : base("invaders", "Background")
    {
        speed = 0;
        health = 1;
        MovementDirection = new Vector2f(0, 0);
        Position = source.Position;
        this.source = source;
    }

    public override void Create(Scene scene)
    {
        if (source.tag == "Ally")
        {
            sprite.TextureRect = new IntRect(708, 0, 36, 36);
        }
        else
        {
            sprite.TextureRect = new IntRect(744, 0, 36, 36);
        }

        base.Create(scene);
    }

    public override void Update(Scene scene, float deltaTime)
    {
        lifeTime -= deltaTime;
        if(lifeTime <= 0)
        {
            isDead = true;
        }
    }
}