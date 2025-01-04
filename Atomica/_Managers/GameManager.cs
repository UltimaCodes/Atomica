namespace Quickie003;

public class GameManager
{
    private readonly MouseEmitter _mouseEmitter = new(); 

    public void Init()
    {
        ParticleEmitterData ped = new()
        {
            interval = 0.01f,
            emitCount = 10,
            angleVariance = 180f
        };

        ParticleEmitter pe = new(_mouseEmitter, ped);
        ParticleManager.AddParticleEmitter(pe);
    }

    public static void Update()
    {
        InputManager.Update();
        ParticleManager.Update();
    }

    public static void Draw()
    {
        ParticleManager.Draw();
    }
}