using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Drawing.Imaging;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;

namespace Quickie003
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameManager _gameManager;
        private Texture2D _textTexture;
        private Texture2D _cursorTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false; // Hide default system cursor
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // Set the window to be resizable
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            Globals.Content = Content;
            _gameManager = new();
            _gameManager.Init();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = _spriteBatch;

            // Create a texture from custom text
            _textTexture = CreateTextTexture("Controls:\nArrow Keys: Change Gravity Direction\nSpace: Cursor Follow\nEsc: Exit", 14);

            // Create the custom cursor texture (a white circle)
            _cursorTexture = new Texture2D(GraphicsDevice, 1, 1);
            _cursorTexture.SetData(new Color[] { Color.White }); // Set it to white
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Globals.Update(gameTime);
            _gameManager.Update(); // Update the game logic
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            // Draw the control instructions from the texture
            _spriteBatch.Draw(_textTexture, new Vector2(10, 10), Color.White);

            _gameManager.Draw(); // Draw the particles

            // Draw the custom cursor at the mouse position
            Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            _spriteBatch.Draw(_cursorTexture, mousePosition - new Vector2(_cursorTexture.Width / 2, _cursorTexture.Height / 2), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        // Create a texture from the given text and font size
        private Texture2D CreateTextTexture(string text, int fontSize)
        {
            // Create a Bitmap to draw the text
            using (var bitmap = new Bitmap(1, 1))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                // Set the font
                var font = new System.Drawing.Font("Arial", fontSize);

                // Measure the size of the text
                var size = graphics.MeasureString(text, font);

                // Create a new Bitmap with the correct size
                using (var newBitmap = new Bitmap((int)size.Width, (int)size.Height))
                using (var newGraphics = Graphics.FromImage(newBitmap))
                {
                    newGraphics.Clear(System.Drawing.Color.Transparent);
                    SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                    newGraphics.DrawString(text, font, brush, 0, 0);

                    // Convert the System.Drawing.Bitmap to a MonoGame Texture2D
                    var stream = new MemoryStream();
                    newBitmap.Save(stream, ImageFormat.Png);
                    stream.Seek(0, SeekOrigin.Begin);
                    var texture = Texture2D.FromStream(GraphicsDevice, stream);
                    return texture;
                }
            }
        }
    }
}
