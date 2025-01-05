namespace Quickie003
{
    public struct ParticleData
    {
        private static Texture2D _defaultTexture;
        public Texture2D texture = _defaultTexture ??= Globals.Content.Load<Texture2D>("particle");
        public float lifespan = 3f;
        public Color colorStart = Color.Red;
        public Color colorEnd = Color.Purple;
        public float opacityStart = 1f;
        public float opacityEnd = 0f;
        public float sizeStart = 32f;
        public float sizeEnd = 4f;
        public float speed = 100f;
        public float angle = 0f;
        public float initialSpread = 50f;  // Controls the spread radius
        public float clumpingStrength = 0.1f;  // Controls the strength of the clumping
        public float dragFactor = 0.98f;

        public ParticleData()
        {
        }

        public Color GetRainbowColor(float lifeProgress)
        {
            lifeProgress = Math.Clamp(lifeProgress, 0f, 1f);
            float hue = lifeProgress;
            return ColorFromHSV(hue * 360f, 1f, 1f);
        }

        private Color ColorFromHSV(float hue, float saturation, float value)
        {
            int h = (int)(hue / 60) % 6;
            float f = hue / 60 - (int)(hue / 60);
            float p = value * (1 - saturation);
            float q = value * (1 - f * saturation);
            float t = value * (1 - (1 - f) * saturation);

            float r = 0, g = 0, b = 0;
            switch (h)
            {
                case 0: r = value; g = t; b = p; break;
                case 1: r = q; g = value; b = p; break;
                case 2: r = p; g = value; b = t; break;
                case 3: r = p; g = q; b = value; break;
                case 4: r = t; g = p; b = value; break;
                case 5: r = value; g = p; b = q; break;
            }

            return new Color(r, g, b);
        }
    }
}
