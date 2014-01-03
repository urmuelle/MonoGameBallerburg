// <copyright file="Castle.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Graphic
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Graphics;

  using MonoGameBallerburg.Manager;

  /// <summary>
  /// Class used to draw a castle
  /// </summary>
  public class Castle
  {
    #region Fields

    private Collection<Tower> towers;

    private string name;
    private int price;

    // Center of the castle, calculated as average of the tower centers on
    // initialization.
    private Vector3 center;

    // The ID of this game object
    private int id;

    // The castles currently selected tower. Readonly, value
    // is modified through NextTower
    private int currentTower;

    private List<Wall> walls;

    /*
    private List<Cannon> cannons;

    private int currentCannon;
    public Cannon CurrentCannon
    {
        get { return this.cannons[currentCannon]; }
    }
     * */

    private Vector3 cameraPosition;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Castle"/> class.
    /// </summary>
    /// <param name="reader">The castle content reader</param>
    public Castle(ContentReader reader)
    {
      this.id = 0;

      if (reader == null)
      {
        throw new ArgumentNullException("reader");
      }

      this.name = reader.ReadString();
      this.price = reader.ReadInt32();

      var tempTowers = reader.ReadObject<Tower[]>();

      var sumX = 0.0f;
      var sumZ = 0.0f;

      this.towers = new Collection<Tower>();
      foreach (Tower t in tempTowers)
      {
        sumX += t.X;
        sumZ += t.Y;
        this.towers.Add(t);
      }

      this.walls = new List<Wall>();
      var tempWalls = reader.ReadObject<Wall[]>();
      foreach (var w in tempWalls)
      {
        w.SetFromToVectors(
            new Vector3(this.towers[w.FromIndex].X, 0.0f, this.towers[w.FromIndex].Y),
            new Vector3(this.towers[w.ToIndex].X, 0.0f, this.towers[w.ToIndex].Y));

        this.walls.Add(w);
      }

      this.center = new Vector3(sumX / this.towers.Count, 0.0f, sumZ / this.towers.Count);
      this.currentTower = 0;
    }

    /// <summary>
    /// Gets the towers.
    /// </summary>        
    public Collection<Tower> Towers
    {
      get { return this.towers; }
    }

    /// <summary>
    /// Gets or sets the position.
    /// </summary>
    /// <value>
    /// The position.
    /// </value>
    public Vector3 Position { get; set; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name
    {
      get { return this.name; }
    }

    /// <summary>
    /// Gets the price.
    /// </summary>
    public int Price
    {
      get { return this.price; }
    }

    /// <summary>
    /// Gets the center.
    /// </summary>
    public Vector3 Center
    {
      get { return this.center; }
    }

    /// <summary>
    /// Gets the ID.
    /// </summary>
    public int ID
    {
      get { return this.id; }
    }

    /// <summary>
    /// Gets the current tower.
    /// </summary>
    public Tower CurrentTower
    {
      get { return this.towers[this.currentTower]; }
    }

    /// <summary>
    /// Gets or sets the camera position.
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
    /// Sets the castle's current Tower to the given tower number.
    /// If the tower number is greater than the amount of Towers
    /// an ArgumentException is thrown. Tower count starts with 0!
    /// </summary>
    /// <param name="towerNum">The number (i.e. the index) of the tower</param>
    public void SelectTower(int towerNum)
    {
      this.currentTower = towerNum;
      if (this.currentTower >= this.towers.Count)
      {
        throw new ArgumentException("Given tower number exceeds number of towers");
      }
    }

    /// <summary>
    /// Increases the current tower by 1 or sets it to 0, if the amount of
    /// Towers is reached.
    /// </summary>
    public void SwitchTower()
    {
      this.currentTower += 1;

      if (this.currentTower >= this.towers.Count)
      {
        this.currentTower = 0;
      }
    }

    /// <summary>
    /// Initializes the specified graphics device.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device.</param>
    public void Initialize(GraphicsDevice graphicsDevice)
    {
      foreach (var t in this.towers)
      {
        t.InitGraphics(graphicsDevice);
      }

      foreach (var w in this.walls)
      {
        w.InitGraphics(graphicsDevice);
      }
    }

    /// <summary>
    /// Draws the castle.
    /// </summary>
    /// <param name="world">The world.</param>
    /// <param name="viewMatrix">The view matrix</param>
    /// <param name="projectionMatrix">The projection Matrix</param>
    /// <param name="lightView">The light view.</param>
    /// <param name="shaderEffect">The shader effect.</param>
    /// <param name="contentManager">The content manager.</param>
    /// <param name="graphicsDevice">The graphics device.</param>
    public void Draw(Matrix world, Matrix viewMatrix, Matrix projectionMatrix, Matrix lightView, Effect shaderEffect, IContentManager contentManager, GraphicsDevice graphicsDevice)
    {
      foreach (var t in this.towers)
      {
        t.Draw(world, viewMatrix, projectionMatrix, lightView, shaderEffect, contentManager, graphicsDevice);
      }

      foreach (var w in this.walls)
      {
        w.Draw(world, viewMatrix, projectionMatrix, lightView, shaderEffect, graphicsDevice);
      }
    }

    /// <summary>
    /// Draws the castle.
    /// </summary>
    /// <param name="world">The world.</param>
    /// <param name="viewMatrix">The view matrix</param>
    /// <param name="projectionMatrix">The projection Matrix</param>
    /// <param name="lightView">The light view.</param>
    /// <param name="shaderEffect">The shader effect.</param>
    /// <param name="contentManager">The content manager.</param>
    /// <param name="graphicsDevice">The graphics device.</param>
    public void DrawDefault(Matrix world, Matrix viewMatrix, Matrix projectionMatrix, Matrix lightView, Effect shaderEffect, IContentManager contentManager, GraphicsDevice graphicsDevice)
    {
      foreach (var t in this.towers)
      {
        t.DrawDefault(world, viewMatrix, projectionMatrix, lightView, shaderEffect, contentManager, graphicsDevice);
      }

      foreach (var w in this.walls)
      {
        w.Draw(world, viewMatrix, projectionMatrix, lightView, shaderEffect, graphicsDevice);
      }
    }
  }
}
