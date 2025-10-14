using System.ComponentModel;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Invaders;

public class Entity
{
    public string Tag { get; set; }
    protected Sprite sprite = new Sprite();
    private string textureName;
    public bool isDead;
    public bool DestroyOnLoad = true;

    protected Entity(string textureName, string tag)
    {
        this.textureName = textureName;
        Tag = tag;
    }

    public Vector2f Position
    {
        get => sprite.Position;
        set => sprite.Position = value;
    }

    public virtual FloatRect Bounds => sprite.GetGlobalBounds();
    //public virtual bool Solid => false; Tror inte den behövs men vi får se

    public virtual void Create(Scene scene)
    {
        sprite.Texture = scene.AssetManager.LoadTexture(textureName);
    }

    public virtual void Destroy(Scene scene) { }

    public virtual void Update(Scene scene, float deltaTime)
    {

    }
    public virtual void Render(RenderTarget target)
    {
        target.Draw(sprite);
    }

    //protected virtual void CollideWith(Scene scene, Entity other) { }
}
