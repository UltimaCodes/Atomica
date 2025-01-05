namespace Quickie003
{
    public class Particle
    {
        private readonly ParticleData _data;
        private Vector2 _position;
        private Vector2 _velocity;
        private float _lifespanLeft;
        private float _lifespanAmount;
        private Color _color;
        private float _opacity;
        public bool isFinished = false;
        private float _scale;
        private Vector2 _origin;

        private bool _shouldFollowCursor; // New flag to track if particles should follow the cursor

        // Public property for accessing Position
        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public Particle(Vector2 pos, ParticleData data, Vector2 initialDirection)
        {
            _data = data;
            _lifespanLeft = data.lifespan;
            _lifespanAmount = 1f;
            _position = pos;
            _color = data.GetRainbowColor(0);
            _opacity = data.opacityStart;
            _origin = new(_data.texture.Width / 2, _data.texture.Height / 2);
            _velocity = initialDirection * data.speed;
            _shouldFollowCursor = false; // Default is not following the cursor
        }

        public void Update(Vector2 gravity, Vector2 cursorPosition)
        {
            _lifespanLeft -= Globals.TotalSeconds;
            if (_lifespanLeft <= 0f)
            {
                isFinished = true;
                return;
            }

            _lifespanAmount = MathHelper.Clamp(_lifespanLeft / _data.lifespan, 0, 1);
            _color = _data.GetRainbowColor(_lifespanAmount);
            _opacity = MathHelper.Clamp(MathHelper.Lerp(_data.opacityEnd, _data.opacityStart, _lifespanAmount), 0, 1);
            _scale = MathHelper.Lerp(_data.sizeEnd, _data.sizeStart, _lifespanAmount) / _data.texture.Width;

            if (_shouldFollowCursor)
            {
                // Make the particle follow the cursor position directly
                Vector2 directionToCursor = cursorPosition - _position;
                directionToCursor.Normalize();
                _velocity = directionToCursor * _data.speed;
            }
            else
            {
                // Apply gravity force if not following the cursor
                ApplyForce(gravity);
            }

            // Update position based on velocity
            _position += _velocity * Globals.TotalSeconds;
        }

        public void ApplyForce(Vector2 force)
        {
            _velocity += force;
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(_data.texture, _position, null, _color * _opacity, 0f, _origin, _scale, SpriteEffects.None, 1f);
        }

        public void StartFollowingCursor()
        {
            _shouldFollowCursor = true;
        }

        public void StopFollowingCursor()
        {
            _shouldFollowCursor = false;
        }
    }
}
