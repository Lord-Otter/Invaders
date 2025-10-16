using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static SFML.Window.Keyboard.Key;


namespace Invaders;

public class Player() : Actor("Player")
{
    public override void Create(Scene scene)
    {
        speed = 100;
        health = 3;
        sprite.TextureRect = new IntRect(0, 0, 100, 76);
        base.Create(scene);
    }

    private void TakeDamage(int dmgTaken)
    {
        health -= dmgTaken;
        if (health <= 0)
        {
            isDead = true;

        }
    }
            // CONTINUE HERE AND MAKE MOVEMENT!!!
    public override void Update(Scene scene, float deltaTime)
    {
        Vector2f movement = new Vector2f(0, 0);
        if (Keyboard.IsKeyPressed(Down))
        {
            movement.Y += speed * deltaTime;
        }

        Position += movement;
    }
}