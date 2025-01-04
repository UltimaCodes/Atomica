namespace Quickie003
{
    public class Particle
    {
        private readonly ParticleData _data;
        private Vector2 _position;
        private float _lifespanLeft;
        private float _lifespanAmount;
        private Color _color;
        private float _opacity;
        public bool isFinished = false;
        private float _scale;
        private Vector2 _origin;
        private Vector2 _direction;
        private Vector2 _velocity;
        private float _dragFactor = 0.98f;  // Drag for slowing particles down

        public Particle(Vector2 pos, ParticleData data)
        {
            _data = data;
            _lifespanLeft = data.lifespan;
            _lifespanAmount = 1f;
            _position = pos;
            _color = data.colorStart;  // Initial color set to colorStart
            _opacity = data.opacityStart;
            _origin = new(_data.texture.Width / 2, _data.texture.Height / 2);

            // Initialize direction and velocity based on angle and speed
            if (data.speed != 0)
            {
                _data.angle = MathHelper.ToRadians(_data.angle);
                _direction = new Vector2((float)Math.Sin(_data.angle), -(float)Math.Cos(_data.angle));
                _velocity = _direction * _data.speed;
            }
            else
            {
                _direction = Vector2.Zero;
                _velocity = Vector2.Zero;
            }
        }

        public void Update()
        {
            _lifespanLeft -= Globals.TotalSeconds;
            if (_lifespanLeft <= 0f)
            {
                isFinished = true;
                return;
            }

            // Calculate the life progress (from 0 to 1)
            _lifespanAmount = MathHelper.Clamp(_lifespanLeft / _data.lifespan, 0, 1);

            // Use the GetRainbowColor method to smoothly transition the color
            _color = _data.GetRainbowColor(_lifespanAmount);

            // Apply gravity and drag (resistance)
            _velocity.Y += 9.8f * Globals.TotalSeconds;  // Gravity
            _velocity *= _dragFactor;  // Apply drag

            // Add random sway to position for fluid-like movement
            float sway = (float)Math.Sin(_lifespanAmount * 10) * 2f;  // Sinusoidal sway
            _position += (_velocity + new Vector2(sway, 0)) * Globals.TotalSeconds;

            // Lerp opacity and scale based on lifespan
            _opacity = MathHelper.Clamp(MathHelper.Lerp(_data.opacityEnd, _data.opacityStart, _lifespanAmount), 0, 1);
            _scale = MathHelper.Lerp(_data.sizeEnd, _data.sizeStart, _lifespanAmount) / _data.texture.Width;
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(_data.texture, _position, null, _color * _opacity, 0f, _origin, _scale, SpriteEffects.None, 1f);
        }
    }
}
