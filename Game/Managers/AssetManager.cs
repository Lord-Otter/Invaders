using SFML.Audio;
using SFML.Graphics;


namespace Invaders;

public class AssetManager
{
    private readonly Dictionary<string, Texture> textures;
    private readonly Dictionary<string, Font> fonts;
    private readonly Dictionary<string, SoundBuffer> soundBuffers;
    private readonly List<Sound> soundPool = new List<Sound>();

    private readonly int soundCap = 8;

    public AssetManager()
    {
        textures = new Dictionary<string, Texture>();
        fonts = new Dictionary<string, Font>();
        soundBuffers = new Dictionary<string, SoundBuffer>();
    }

    public Texture LoadTexture(string name)
    {
        if (textures.TryGetValue(name, out Texture? cached))
        {
            return cached;
        }
        Texture texture = new Texture($"assets/{name}.png");
        texture.Smooth = false;
        textures.Add(name, texture);
        return texture;
    }

    public Font LoadFont(string name)
    {
        if (fonts.TryGetValue(name, out Font? cached))
        {
            return cached;
        }
        Font font = new Font($"assets/{name}.ttf");
        fonts.Add(name, font);
        return font;
    }

    public SoundBuffer LoadSoundBuffer(string name)
    {
        if (soundBuffers.TryGetValue(name, out SoundBuffer? cached))
        {
            return cached;
        }
        SoundBuffer soundBuffer = new SoundBuffer($"assets/{name}.wav");
        soundBuffers.Add(name, soundBuffer);
        return soundBuffer;
    }

    public void PlaySound(string name)
    {
        SoundBuffer buffer = LoadSoundBuffer(name);
        foreach (Sound sound in soundPool)
        {
            if (sound.Status == SoundStatus.Stopped)
            {
                sound.SoundBuffer = buffer;
                sound.Play();
                return;
            }
        }
        
        if(soundPool.Count < soundCap)
        {
            Sound newSound = new Sound(buffer);
            soundPool.Add(newSound);
            newSound.Play();
        }
    }
}