// <copyright file="PlayerSettings.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Gameplay
{
  using System.Linq;

  /// <summary>
  /// Player Type: human or ai.
  /// </summary>
  public enum PlayerType
  {
    /// <summary>
    /// A Human player
    /// </summary>
    Human,

    /// <summary>
    /// An ai player
    /// </summary>
    Computer,
  }

  /// <summary>
  /// The sort of cannonballs available in the game.
  /// </summary>
  public enum CannonballType
  {
    /// <summary>
    /// Stone cannonballs
    /// </summary>
    Stone,

    /// <summary>
    /// Iron cannonballs
    /// </summary>
    Iron,
  }

  /// <summary>
  /// Class holding the individual player settings like castle, money, etc.
  /// </summary>
  public class PlayerSettings
  {
    #region Fields

    private readonly CastleSettings castleSettings;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerSettings"/> class.
    /// </summary>
    public PlayerSettings()
    {
      this.castleSettings = new CastleSettings();
    }

    #region Properties

    /// <summary>
    /// Gets the castle.
    /// </summary>
    public CastleSettings Castle
    {
      get { return this.castleSettings; }
    }

    /// <summary>
    /// Gets or sets the cannonballtype.
    /// </summary>
    /// <value>
    /// The type value.
    /// </value>
    public CannonballType CannonballType { get; set; }

    /// <summary>
    /// Gets or sets the population.
    /// </summary>
    /// <value>
    /// The population.
    /// </value>
    public int Population { get; set; }

    /// <summary>
    /// Gets or sets the verzinsung.
    /// </summary>
    /// <value>
    /// The verzinsung.
    /// </value>
    public short Verzinsung { get; set; }

    /// <summary>
    /// Gets or sets the steinkugel.
    /// </summary>
    /// <value>
    /// The steinkugel.
    /// </value>
    public int Steinkugel { get; set; }

    /// <summary>
    /// Gets or sets the eisenkugeln.
    /// </summary>
    /// <value>
    /// The eisenkugeln.
    /// </value>
    public int Eisenkugeln { get; set; }

    /// <summary>
    /// Gets or sets the powder.
    /// </summary>
    /// <value>
    /// The powder.
    /// </value>
    public int Powder { get; set; }

    /// <summary>
    /// Gets or sets the money.
    /// </summary>
    /// <value>
    /// The money.
    /// </value>
    public int Money { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is human.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is human; otherwise, <c>false</c>.
    /// </value>
    public PlayerType PlayerType { get; set; }

    /// <summary>
    /// Gets or sets the name of the player.
    /// </summary>
    /// <value>
    /// The name of the player.
    /// </value>
    public string PlayerName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
    /// </value>
    public bool IsActive { get; set; }

    public Graphic.CameraState CurrentCameraState { get; set; }

    #endregion

    /// <summary>
    /// Get the number of active cannons.
    /// </summary>
    /// <returns>The number of active cannons.</returns>
    public int NumCannons()
    {
      return Castle.Towers.Count(t => t.HasCannon);
    }

    /// <summary>
    /// Get the number of active cannons.
    /// </summary>
    /// <returns>
    /// The number of active cannons.
    /// </returns>
    public int MaxNumCannons()
    {
      return Castle.Towers.Count();
    }

    /// <summary>
    /// Adds the cannon.
    /// </summary>
    public void AddCannon()
    {
      foreach (var tower in Castle.Towers.Where(tower => !tower.HasCannon))
      {
        tower.HasCannon = true;
        return;
      }
    }

    /// <summary>
    /// Adds the powder.
    /// </summary>
    public void AddPowder()
    {
      Powder += 1000;
    }

    /// <summary>
    /// Adds the iron ball.
    /// </summary>
    public void AddIronBall()
    {
      Eisenkugeln += 1;
    }

    /// <summary>
    /// Adds the stone ball.
    /// </summary>
    public void AddStoneBall()
    {
      Steinkugel += 1;
    }
  }
}
