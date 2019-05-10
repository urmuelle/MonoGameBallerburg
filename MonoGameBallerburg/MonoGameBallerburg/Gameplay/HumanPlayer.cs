// <copyright file="HumanPlayer.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Gameplay
{
  using MonoGameBallerburg.Graphic;

  public class HumanPlayer : Player
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="HumanPlayer"/> class.
    /// </summary>
    /// <param name="castle">The castle.</param>
    public HumanPlayer(Castle castle)
      : base(castle)
    {
    }
  }
}
