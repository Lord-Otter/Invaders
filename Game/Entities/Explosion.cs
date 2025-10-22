using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

public class Explosion : Entity
{
    private float frameTimer;
    private int frame;

    public Explosion(Entity source) : base("invaders", "Background")
    {
        speed = 0;
        health = 1;
        MovementDirection = new Vector2f(0, 0);
        Position = source.Position;
    }

    public override void Create(Scene scene)
    {
        sprite.TextureRect = new IntRect(0, 90, 125, 125);
        base.Create(scene);
        scene.AssetManager.PlaySound("explosion");
    }

    public override void Update(Scene scene, float deltaTime)
    {
        frameTimer += deltaTime;
        if (frameTimer >= 0.05f)
        {
            frame++;
            frameTimer = 0;
            sprite.TextureRect = new IntRect(125 * frame, 90, 125, 125);

            if(frame > 6)
            {
                isDead = true;
            }
        }
    }
}