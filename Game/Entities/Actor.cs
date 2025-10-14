using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

public abstract class Actor(string tag) : Entity("invaders", tag)
{
    protected int health;
}