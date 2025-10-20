using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

public abstract class Projectile : Entity
{
    protected Projectile(string textureName, string tag) : base(textureName, tag)
    {
        
    }
}