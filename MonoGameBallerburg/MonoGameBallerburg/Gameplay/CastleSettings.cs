// <copyright file="CastleSettings.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Gameplay
{
  using System;
  using System.Collections.ObjectModel;
  using Microsoft.Xna.Framework;

  /// <summary>
  /// Common Settings that represent a castle
  /// </summary>
  public class CastleSettings
  {
    /// <summary>
    /// A castle has a set of towers
    /// </summary>
    private Collection<TowerSettings> towerSettings;

    // The castles currently selected tower. Readonly, value
    // is modified through NextTower
    private int currentTower;

    private Vector3 cameraPosition;

    /// <summary>
    /// Initializes a new instance of the <see cref="CastleSettings"/> class.
    /// </summary>
    public CastleSettings()
    {
      this.towerSettings = new Collection<TowerSettings>();
    }

    /// <summary>
    /// Gets the current tower.
    /// </summary>
    public TowerSettings CurrentTower
    {
      get { return this.Towers[this.currentTower]; }
    }

    /// <summary>
    /// Gets or sets the type of the castle.
    /// </summary>
    /// <value>
    /// The type of the castle.
    /// </value>
    public int CastleType { get; set; }

    /// <summary>
    /// Gets or sets the start pos.
    /// </summary>
    /// <value>
    /// The start pos.
    /// </value>
    public Vector3 StartPos { get; set; }

    /// <summary>
    /// Gets the towers.
    /// </summary>
    public Collection<TowerSettings> Towers
    {
      get { return this.towerSettings; }
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

      if (this.currentTower >= this.Towers.Count)
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

      if (this.currentTower >= this.Towers.Count)
      {
        this.currentTower = 0;
      }
    }

    ////public Vector3 Position { get; set; }
  }
}
