using System.Runtime.InteropServices;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

public sealed class Scene
{
    private List<Entity> entities = new List<Entity>();
    public readonly AssetManager AssetManager = new AssetManager();
    public readonly GameManager GameManager = new GameManager();

    public void Spawn(Entity entity)
    {
        entities.Add(entity);
        entity.Create(this);
    }

    public void Clear()
    {
        for (int i = entities.Count - 1; i >= 0; i--)
        {
            Entity entity = entities[i];
            if (entity.DestroyOnLoad)
            {
                entities.RemoveAt(i);
                entity.Destroy(this);
            }
        }
    }

    public void UpdateAll(float deltaTime)
    {
        for (int i = 0; i < entities.Count;)
        {
            if (entities[i].isDead)
            {
                entities.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }

        for (int i = entities.Count - 1; i >= 0; i--)
        {
            entities[i].Update(this, deltaTime);
        }
    }

    public void RenderAll(RenderTarget target)
    {
        foreach (Entity entity in entities)
        {
            entity.Render(target);
        }
    }
    
        public IEnumerable<Entity> FindIntersects(FloatRect bounds)
    {
        int lastEntity = entities.Count - 1;
        for (int i = lastEntity - 1; i >= 0; i--)
        {
            Entity entity = entities[i];
            if (entity.isDead)
            {
                continue;
            }
            if (entity.Bounds.Intersects(bounds))
            {
                yield return entity;
            }
        }
    }
}