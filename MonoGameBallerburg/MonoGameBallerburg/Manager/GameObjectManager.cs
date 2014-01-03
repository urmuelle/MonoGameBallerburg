// <copyright file="GameObjectManager.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Manager
{
  using System;
  using System.Collections.ObjectModel;
  using Animation;

  using Graphic;
  using Microsoft.Xna.Framework;

  using MonoGameBallerburg.Audio;

  public class GameObjectManager : IGameObjectManager, IDisposable
  {
    private const int MaxCannonBalls = 100;

    private readonly AudioManager audioManager;

    private readonly IBallerburgGraphicsManager graphicsDevice;

    ////private readonly ICameraManager cameraManager;
    private readonly IContentManager contentManager;

    private readonly Castle[] castles;
    private AnimatedBillboard explosionBillboard;

    private Cannonball[] activeCannonballs;
    private SkyBox skyBox;
    private Terrain terrain;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameObjectManager"/> class.
    /// </summary>
    /// <param name="contentManager">The content manager.</param>
    /// <param name="audioManager">The audio manager.</param>
    /// <param name="graphicsDevice">The graphics device.</param>
    public GameObjectManager(IContentManager contentManager, AudioManager audioManager, IBallerburgGraphicsManager graphicsDevice)
    {
      this.contentManager = contentManager;
      this.castles = new Castle[Gameplay.Constants.NumCastles];
      this.audioManager = audioManager;
      this.graphicsDevice = graphicsDevice;
    }

    #region IGameObjectManager Members

    /// <summary>
    /// Gets the castles.
    /// </summary>
    public Castle[] Castles
    {
      get { return this.castles; }
    }

    /// <summary>
    /// Gets the terrain.
    /// </summary>
    public Terrain Terrain
    {
      get { return this.terrain; }
    }

    /// <summary>
    /// Gets the explosion billboard.
    /// </summary>
    public AnimatedBillboard ExplosionBillboard
    {
      get { return this.explosionBillboard; }
    }

    /// <summary>
    /// Gets the sky box.
    /// </summary>
    public SkyBox SkyBox
    {
      get { return this.skyBox; }
    }

    /// <summary>
    /// Gets the active cannonballs.
    /// </summary>
    public Collection<Cannonball> ActiveCannonballs
    {
      get { return new Collection<Cannonball>(activeCannonballs); }
    }

    /// <summary>
    /// Loads the content.
    /// </summary>
    public void LoadContent()
    {
      ////this.castles = new Castle[Ballerburg.Gameplay.Constants.NumCastles];
      castles[0] = contentManager.Castle1;
      castles[1] = contentManager.Castle2;
      castles[2] = contentManager.Castle3;
      castles[3] = contentManager.Castle4;
      castles[4] = contentManager.Castle5;
      castles[5] = contentManager.Castle6;
      castles[6] = contentManager.Castle7;
      castles[7] = contentManager.Castle8;

      // Init the cannonball array
      this.activeCannonballs = new Cannonball[MaxCannonBalls];
      for (var i = 0; i < MaxCannonBalls; i++)
      {
        activeCannonballs[i] = new Cannonball(contentManager);
      }

      skyBox = contentManager.SkyBox;

      terrain = new Terrain(contentManager, graphicsDevice);

      explosionBillboard = new AnimatedBillboard(false, contentManager);
      explosionBillboard.LoadContent();
      explosionBillboard.InitGraphics(graphicsDevice.GraphicsDevice);

      explosionBillboard.PrepareAnimation();
      explosionBillboard.Pause();
    }

    /// <summary>
    /// Updates the specified game time.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public void Update(GameTime gameTime)
    {
      UpdateCannonballs(gameTime);
      explosionBillboard.Update(gameTime);
    }

    /// <summary>
    /// Draws this instance.
    /// </summary>
    public void Draw()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Updates the cannon balls.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    private void UpdateCannonballs(GameTime gameTime)
    {
      foreach (var cannonBall in this.activeCannonballs)
      {
        if (cannonBall.Alive)
        {
          cannonBall.Update(gameTime);

          if (cannonBall.Alive)
          {
            if (cannonBall.Position.Y < 0.0f)
            {
              cannonBall.Alive = false;
              ////explosionBillboard.Play();
              this.audioManager.PlayHitSound();
            }
          }
        }
      }
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        this.terrain.Dispose();
        this.terrain = null;
        this.explosionBillboard.Dispose();
        this.explosionBillboard = null;
      }
    }

    #endregion
  }
}
