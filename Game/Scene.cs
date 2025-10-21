using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

public sealed class Scene
{
    private List<Entity> entities = new List<Entity>();
    public readonly AssetManager AssetManager = new AssetManager();
    //public readonly EventManager eventManager = new EventManager();

    public void Spawn(Entity entity, Vector2f? position = null)
    {
        entities.Add(entity);
        entity.Create(this);
        if(position.HasValue)
        {
            entity.Position = position.Value;   
        }
    }

    public void Clear()
    {
        for (int i = entities.Count - 1; i >= 0; i--)
        {
            Entity entity = entities[i];
            entities.RemoveAt(i);
            entity.OnDestroy(this);  
        }
    }

    public void UpdateAll(float deltaTime)
    {
        for (int i = entities.Count - 1; i >= 0; i--)
        {
            entities[i].Update(this, deltaTime);
        }
        
        for (int i = 0; i < entities.Count;)
        {
            Entity entity = entities[i];
            if (entities[i].isDead)
            {
                entities.RemoveAt(i);
                entity.OnDestroy(this);
            }
            else
            {
                i++;
            }
        }
    }

    public void RenderAll(RenderTarget target)
    {
        foreach (Entity entity in entities)
        {
            entity.Render(target);
        }
    }

    public IEnumerable<Entity> FindIntersects(Entity self)
    {
        foreach (Entity other in entities)
        {
            if (other == self || other.isDead || other.tag == self.tag || other.tag == "Background")
                continue;

            float distanceX = self.CollisionCenter.X - other.CollisionCenter.X;
            float distanceY = self.CollisionCenter.Y - other.CollisionCenter.Y;
            float distanceSq = distanceX * distanceX + distanceY * distanceY;
            float combinedRadius = self.CollisionRadius + other.CollisionRadius;

            if (distanceSq <= combinedRadius * combinedRadius)
                yield return other;
        }
    }
}