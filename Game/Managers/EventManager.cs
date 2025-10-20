/*using System.Runtime;

namespace Invaders;

public delegate void ValueChangedEvent(Scene scene, int value);

public class EventManager
{
    private int healthChange = 0;
    private int scoreGained = 0;

    private readonly List<(Entity target, int damage)> damageEvents = new();

    public event ValueChangedEvent? UpdateHealth;
    public event ValueChangedEvent? GainScore;

    public void PublishUpdateHealth(int amount = 0) => healthChange += amount;
    public void PublishGainScore(int amount = 1) => scoreGained += amount;

    public void DamageBuffer(Entity target, int damage)
    {
        damageEvents.Add((target, damage));
    }

    public void DispatchEvents(Scene scene)
    {
        foreach ((Entity target, int damage) in damageEvents)
        {
            if (!target.isDead)
            {
                target.LoseHealth(scene, damage);
            }
        }

        damageEvents.Clear();
    }
}*/