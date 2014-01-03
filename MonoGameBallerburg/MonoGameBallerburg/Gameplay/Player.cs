// <copyright file="Player.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Gameplay
{
  using Graphic;

  public class Player
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Player"/> class.
    /// </summary>
    /// <param name="castle">The castle.</param>
    public Player(Castle castle)
    {
      this.PlayerCastle = castle;
    }

    /// <summary>
    /// Gets or sets the settings.
    /// </summary>
    /// <value>
    /// The settings.
    /// </value>
    public PlayerSettings Settings { get; set; }

    /// <summary>
    /// Gets or sets the player castle.
    /// </summary>
    /// <value>
    /// The player castle.
    /// </value>
    public Castle PlayerCastle { get; set; }
  }
}
