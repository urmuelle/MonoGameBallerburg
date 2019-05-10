// <copyright file="GameSettings.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Gameplay
{
  using MonoGameBallerburg.Manager;

  public enum Season
  {
    /// <summary>
    /// Let the system choose
    /// </summary>
    Zufall,

    /// <summary>
    /// Winter season
    /// </summary>
    Winter,

    /// <summary>
    /// summer season
    /// </summary>
    Sommer,
  }

  public enum Weather
  {
    /// <summary>
    /// Random weather
    /// </summary>
    Zufall,

    /// <summary>
    /// Bad weather
    /// </summary>
    Schlecht,

    /// <summary>
    /// sunshine, nice weather
    /// </summary>
    Schoen,
  }

  public enum Daytime
  {
    /// <summary>
    /// Daytime: dawn
    /// </summary>
    Morgen,

    /// <summary>
    /// Noon, normal day
    /// </summary>
    Tag,

    /// <summary>
    /// Evening daytime
    /// </summary>
    Abend,

    /// <summary>
    /// Night, dark
    /// </summary>
    Nacht,

    /// <summary>
    /// Let the system choose
    /// </summary>
    Zufall,
  }

  public enum Difficulty
  {
    /// <summary>
    /// Very easy difficulty
    /// </summary>
    Amateur,

    /// <summary>
    /// Hard difficulty
    /// </summary>
    Profi,

    /// <summary>
    /// Very hard difficulty
    /// </summary>
    Experte,

    /// <summary>
    /// Easy difficulty
    /// </summary>
    Anfänger,
  }

  public enum GameType
  {
    /// <summary>
    /// Normaler Spieltyp
    /// </summary>
    Klassisch,

    /// <summary>
    /// Arkade Modus
    /// </summary>
    Arkade,
  }

  public class GameSettings : IGameSettingsManager
  {
    /// <summary>
    /// Gets or sets the num players.
    /// </summary>
    /// <value>
    /// The num players.
    /// </value>
    public int NumPlayers { get; set; }

    /// <summary>
    /// Gets or sets the game art.
    /// </summary>
    /// <value>
    /// The game art.
    /// </value>
    public GameType GameStyle { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether switching the camera is animated (bezier).
    /// </summary>
    /// <value>
    /// The flag value.
    /// </value>
    public bool AnimatedCamera
    {
      get;
      set;
    }
  }
}
