using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

public abstract class Actor(string tag) : Entity("invaders", tag)
{
    protected int health;
    protected float speed;
    protected int direction;

    protected Vector2i textureOffset = new Vector2i(0, 0);
    public float iFramesTimer { get; private set; } = 0f;
}