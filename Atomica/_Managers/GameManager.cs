namespace Quickie003
{
    public class GameManager
    {
        private readonly MouseEmitter _mouseEmitter = new();

        public GameManager()
        {
            Init();
        }

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

        public void Update()
        {
            InputManager.Update();
            ParticleManager.Update();
        }

        public void Draw()
        {
            ParticleManager.Draw();
        }
    }
}
