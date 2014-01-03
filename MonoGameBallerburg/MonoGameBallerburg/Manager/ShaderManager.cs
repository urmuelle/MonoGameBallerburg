// <copyright file="ShaderManager.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
namespace MonoGameBallerburg.Manager
{
    using System;
    using Microsoft.Xna.Framework.Graphics;
    using XnaContentManager = Microsoft.Xna.Framework.Content.ContentManager;

    /// <summary>
    /// Class encapsulating access on the shaders used throughout the game
    /// </summary>
    public class ShaderManager : IShaderManager
    {
        private XnaContentManager content;

        /// <summary>
        /// Gets the effect.
        /// </summary>
        public Effect TheEffect { get; private set; }

        /// <summary>
        /// Gets the basic effect.
        /// </summary>
        public BasicEffect BasicEffect { get; private set; }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="xnaContent">Content of the xna.</param>
        /// <param name="graphicsDevice">The graphics device.</param>
        public void LoadContent(XnaContentManager xnaContent, IBallerburgGraphicsManager graphicsDevice)
        {
            if (null != this.content)
            {
                return;
            }

            this.content = xnaContent;            

            TheEffect = this.content.Load<Effect>(@"Shader\OurHLSLfile");

            BasicEffect = new BasicEffect(graphicsDevice.GraphicsDevice)
            {
                TextureEnabled = true,
                VertexColorEnabled = false,
                LightingEnabled = false
            };
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            if (null == this.content)
            {
                return;
            }

            this.content.Unload();
        }
    }
}
