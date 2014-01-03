// <copyright file="TowerSettings.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Gameplay
{
  using System;

  using Graphic;
  using Microsoft.Xna.Framework;

  using MonoGameBallerburg.Manager;

  /// <summary>
  /// Settings used for common tower properties
  /// </summary>
  public class TowerSettings : IDisposable
  {
    // each Tower has a tower view.
    // The player can rotate arround the tower, this rotation must be
    // stored.

    // Information stored for the camera
    private Vector3 viewDirection;
    private Vector3 cameraPosition;

    // Cannoninformation
    private bool hasCannon;
    private Graphic.Cannon cannon;

    /// <summary>
    /// Initializes a new instance of the <see cref="TowerSettings"/> class.
    /// </summary>
    /// <param name="tower">The tower.</param>
    /// <param name="graphicsDevice">The graphics device.</param>
    public TowerSettings(Tower tower, IBallerburgGraphicsManager graphicsDevice)
    {
      this.X = tower.X;
      this.Y = tower.Y;
      this.Height = tower.Height;
      this.viewDirection = new Vector3(tower.X, tower.CoverHeight + 2, tower.Y + 1);
      this.ID = tower.ID;
      this.hasCannon = tower.HasCannon;

      if (this.hasCannon)
      {
        this.cannon = new Cannon(tower.X, tower.CoverHeight, tower.Y, null);
        this.cannon.InitGraphics(graphicsDevice.GraphicsDevice);
      }
    }

    /// <summary>
    /// Gets or sets the X.
    /// </summary>
    /// <value>
    /// The X value.
    /// </value>
    public int X
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the Y.
    /// </summary>
    /// <value>
    /// The Y value.
    /// </value>
    public int Y
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    /// <value>
    /// The height.
    /// </value>
    public float Height
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the tower's viewing direction
    /// </summary>
    /// <value>
    /// The view direction.
    /// </value>
    public Vector3 ViewDirection
    {
      get { return this.viewDirection; }
      set { this.viewDirection = value; }
    }

    /// <summary>
    /// Gets or sets the tower's camera position
    /// </summary>
    /// <value>
    /// The camera position.
    /// </value>
    public Vector3 CameraPosition
    {
      get { return this.cameraPosition; }
      set { this.cameraPosition = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the tower has a cannon
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has cannon; otherwise, <c>false</c>.
    /// </value>
    public bool HasCannon
    {
      get { return this.hasCannon; }
      set { this.hasCannon = value; }
    }

    /// <summary>
    /// Gets or sets the tower cannon object.
    /// </summary>
    /// <value>
    /// The tower cannon.
    /// </value>
    public Graphic.Cannon TowerCannon
    {
      get { return this.cannon; }
      set { this.cannon = value; }
    }

    public int ID { get; set; }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
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
        this.cannon.Dispose();
        this.cannon = null;
      }
    }
  }
}
