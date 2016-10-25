using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Shader
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public Effect Effect;
        public Texture2D Texture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();

        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Effect = Content.Load<Effect>("RestrictedDrawing");
            Texture = Content.Load<Texture2D>("10mm_pistol_icon");
        }

        /// <summary>
        /// UnloadContent wird einmal pro Spiel aufgerufen und ist der Ort, wo
        /// Ihr gesamter Content entladen wird.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Entladen Sie jeglichen Nicht-ContentManager-Content hier
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront,
            BlendState.AlphaBlend,
            SamplerState.LinearClamp,
            DepthStencilState.None,
            RasterizerState.CullNone,
            Effect);
            spriteBatch.Draw(Texture, new Vector2(100,100), Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public Texture2D ToTexture(System.Drawing.Bitmap Bitmap)
        {
            Texture2D tex = new Texture2D(GraphicsDevice, Bitmap.Width, Bitmap.Height, false, SurfaceFormat.Color);

            BitmapData data = Bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, Bitmap.Width, Bitmap.Height), 
                System.Drawing.Imaging.ImageLockMode.ReadOnly, 
                Bitmap.PixelFormat);

            int bufferSize = data.Height * data.Stride;
            byte[] bytes = new byte[bufferSize];
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
            tex.SetData(bytes);
            Bitmap.UnlockBits(data);

            return tex;
        }
    }
}
