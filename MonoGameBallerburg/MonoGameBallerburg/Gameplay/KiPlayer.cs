// <copyright file="KiPlayer.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Gameplay
{
  using MonoGameBallerburg.Graphic;

  public class KiPlayer : Player
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="KiPlayer"/> class.
    /// </summary>
    /// <param name="castle">The KI Players castle.</param>
    public KiPlayer(Castle castle)
      : base(castle)
    {
    }
  }
}
