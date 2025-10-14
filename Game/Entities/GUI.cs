using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

public class GUI : Entity
{
    private Text scoreText;

    protected GUI() : base("invaders", "GUI")
    {
        scoreText = new Text();
        sprite.TextureRect = new IntRect(90, 192, 35, 27);
    }
}