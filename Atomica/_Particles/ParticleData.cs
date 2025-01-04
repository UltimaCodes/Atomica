namespace Quickie003
{
    public struct ParticleData
    {
        private static Texture2D _defaultTexture;
        public Texture2D texture = _defaultTexture ??= Globals.Content.Load<Texture2D>("particle");
        public float lifespan = 2f;
        public Color colorStart = Color.Red;  // Starting color (can be any color)
        public Color colorEnd = Color.Purple; // Ending color (can be any color)
        public float opacityStart = 1f;
        public float opacityEnd = 0f;
        public float sizeStart = 32f;
        public float sizeEnd = 4f;
        public float speed = 100f;
        public float angle = 0f;

        public ParticleData()
        {
        }

        // Method to get the interpolated rainbow color based on lifespan or time
        public Color GetRainbowColor(float lifeProgress)
        {
            // Ensure lifeProgress is between 0 and 1
            lifeProgress = Math.Clamp(lifeProgress, 0f, 1f);

            // Calculate the hue based on lifeProgress (0 to 1)
            float hue = lifeProgress;  // The hue will progress from 0 to 1 over the lifespan

            // Convert the hue to a color (rainbow colors based on HSV)
            return ColorFromHSV(hue * 360f, 1f, 1f);  // hue * 360 maps 0-1 to 0-360° for the rainbow
        }

        // Helper method to convert HSV to Color (HSL to RGB)
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
