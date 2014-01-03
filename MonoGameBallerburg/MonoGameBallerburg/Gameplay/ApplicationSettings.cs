// <copyright file="ApplicationSettings.cs" company="Urs Müller">
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
  /// The Background Music tracks
  /// </summary>
  public enum BackgroundMusicTrack
  {
    /// <summary>
    /// The track "Darkstar"
    /// </summary>
    Darkstar,

    /// <summary>
    /// The track "High Tension"
    /// </summary>
    HighTension,

    /// <summary>
    /// The track "Darkstar"
    /// </summary>
    Tentacle,

    /// <summary>
    /// The track "Death Row"
    /// </summary>
    DeathRow,

    /// <summary>
    /// The track "Boomerang"
    /// </summary>
    Boomerang,

    /// <summary>
    /// No background sound
    /// </summary>
    Aus
  }

  /// <summary>
  /// Common settings for sound and display adapter
  /// </summary>
  public class ApplicationSettings
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationSettings"/> class.
    /// </summary>
    public ApplicationSettings()
    {
      this.FxVolume = 0.8f;
      this.MusicVolume = 0.8f;
      this.MenuEffectsVolume = 0.8f;
      this.PlayMusic = true;
      this.PlaySoundFx = true;
      this.PlaySoundMenu = true;
      this.ActiveBackgroundMusicTrack = BackgroundMusicTrack.Darkstar;
    }

    /// <summary>
    /// Gets or sets the fx volume.
    /// </summary>
    /// <value>
    /// The fx volume.
    /// </value>
    public float FxVolume { get; set; }

    /// <summary>
    /// Gets or sets the music volume.
    /// </summary>
    /// <value>
    /// The music volume.
    /// </value>
    public float MusicVolume { get; set; }

    /// <summary>
    /// Gets or sets the menu effects volume.
    /// </summary>
    /// <value>
    /// The menu effects volume.
    /// </value>
    public float MenuEffectsVolume { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [play music].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [play music]; otherwise, <c>false</c>.
    /// </value>
    public bool PlayMusic { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [play sound fx].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [play sound fx]; otherwise, <c>false</c>.
    /// </value>
    public bool PlaySoundFx { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [play sound menu].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [play sound menu]; otherwise, <c>false</c>.
    /// </value>
    public bool PlaySoundMenu { get; set; }

    /// <summary>
    /// Gets or sets the active background music track.
    /// </summary>
    /// <value>
    /// The active background music track.
    /// </value>
    public BackgroundMusicTrack ActiveBackgroundMusicTrack { get; set; }
  }
}
