namespace Quickie003
{
    public static class ParticleManager
    {
        private static readonly List<Particle> _particles = new();
        private static readonly List<ParticleEmitter> _particleEmitters = new();
        private static Vector2 _gravity = new(0, 9.8f); // Default gravity (downwards)

        public static void AddParticle(Particle p)
        {
            _particles.Add(p);
        }

        public static void AddParticleEmitter(ParticleEmitter e)
        {
            _particleEmitters.Add(e);
        }

        public static void UpdateEmitters()
        {
            foreach (var emitter in _particleEmitters)
            {
                emitter.Update();
            }
        }

        public static void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                _gravity = new Vector2(0, -9.8f); // Gravity upwards
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                _gravity = new Vector2(0, 9.8f); // Gravity downwards (default)
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                _gravity = new Vector2(-9.8f, 0); // Gravity left
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                _gravity = new Vector2(9.8f, 0); // Gravity right

            Vector2 cursorPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                foreach (var particle in _particles)
                {
                    particle.StartFollowingCursor();
                }
            }
            else
            {
                foreach (var particle in _particles)
                {
                    particle.StopFollowingCursor();
                }
            }

            UpdateParticles(cursorPosition);
            UpdateEmitters();
        }

        public static void UpdateParticles(Vector2 cursorPosition)
        {
            foreach (var particle in _particles)
            {
                particle.Update(_gravity, cursorPosition); // Pass cursor position
            }

            // Remove finished particles
            _particles.RemoveAll(p => p.isFinished);
        }


        public static void Draw()
        {
            foreach (var particle in _particles)
            {
                particle.Draw();
            }
        }
    }
}
