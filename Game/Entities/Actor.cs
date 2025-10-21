using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

public abstract class Actor : Entity
{
    protected float shootCooldown;
    protected float shootTimer;   

    protected Actor(string textureName, string tag) : base(textureName, tag)
    {
        
    }

    public override void Update(Scene scene, float deltaTime)
    {
        base.Update(scene, deltaTime);

        
    }
    


    protected virtual void Shooting(Scene scene, float deltaTime)
    {
        if (shootTimer > 0)
        {
            shootTimer -= deltaTime;
        }
    }

    protected void Shoot(Scene scene, Vector2f offset)
    {
        Bullet bullet = new Bullet(this, damage);
        Vector2f perpendicular = new Vector2f(-FacingDirection.Y, FacingDirection.X);
        Vector2f spawnPosition = Position + FacingDirection * offset.Y + perpendicular * offset.X;
        scene.Spawn(bullet, spawnPosition);
    }
}