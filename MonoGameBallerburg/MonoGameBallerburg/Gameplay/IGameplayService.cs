// <copyright file="IGameplayService.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Gameplay
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  /// <summary>
  /// Interface for accessing the actual gamelogic
  /// </summary>
  public interface IGameplayService
  {
    /// <summary>
    /// Switches the player.
    /// </summary>
    void SwitchPlayer();

    /// <summary>
    /// Checks for game over.
    /// </summary>
    /// <returns>True if the game is over.</returns>
    bool CheckForGameOver();
  }
}
