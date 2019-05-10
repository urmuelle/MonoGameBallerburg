// <copyright file="BallerburgGraphicsManager.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Manager
{
  using Microsoft.Xna.Framework.Graphics;
  using XnaGraphicsDeviceManager = Microsoft.Xna.Framework.GraphicsDeviceManager;

  public class BallerburgGraphicsManager : IBallerburgGraphicsManager
  {
    private XnaGraphicsDeviceManager graphics;

    private SpriteBatch spriteBatch;

    /// <summary>
    /// Gets the aspect ratio.
    /// </summary>
    public float AspectRatio
    {
      get { return GraphicsDevice.Viewport.AspectRatio; }
    }

    /// <summary>
    /// Gets the sprite batch.
    /// </summary>
    public SpriteBatch SpriteBatch
    {
      get
      {
        return this.spriteBatch;
      }
    }

    /// <summary>
    /// Gets the graphics device.
    /// </summary>
    public GraphicsDevice GraphicsDevice
    {
      get
      {
        return this.graphics.GraphicsDevice;
      }
    }

    /// <inheritdoc/>
    public void Initialize(XnaGraphicsDeviceManager xnaGraphics)
    {
      if (this.graphics != null)
      {
        return;
      }

      this.graphics = xnaGraphics;
      this.graphics.PreferredBackBufferWidth = 640;
      this.graphics.PreferredBackBufferHeight = 480;
      this.graphics.IsFullScreen = false;
      this.graphics.ApplyChanges();

      this.spriteBatch = new SpriteBatch(this.graphics.GraphicsDevice);
    }
  }
}
