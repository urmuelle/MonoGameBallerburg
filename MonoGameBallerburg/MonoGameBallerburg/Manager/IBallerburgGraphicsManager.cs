// <copyright file="IBallerburgGraphicsManager.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Manager
{
    using System;
    using Microsoft.Xna.Framework.Graphics;
    using XnaGraphicsDeviceManager = Microsoft.Xna.Framework.GraphicsDeviceManager;

    /// <summary>
    /// GraphicsManager is responsible for managing all graphics properties.
    /// </summary>
    public interface IBallerburgGraphicsManager
    {
        GraphicsDevice GraphicsDevice { get; }

        float AspectRatio { get; }

        SpriteBatch SpriteBatch { get; }

        void Initialize(XnaGraphicsDeviceManager xnaGraphics);
    }
}
