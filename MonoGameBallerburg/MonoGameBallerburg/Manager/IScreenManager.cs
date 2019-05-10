// <copyright file="IScreenManager.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Manager
{
  using Microsoft.Xna.Framework;
  using MonoGameBallerburg.Audio;
  using MonoGameBallerburg.Gameplay;

  /// <summary>
  /// Nötige Funktionen für einen ScreenManager.
  /// </summary>
  public interface IScreenManager
  {
    /// <summary>
    /// Gets the application settings.
    /// </summary>
    /// <value>
    /// The application settings.
    /// </value>
    ApplicationSettings ApplicationSettings { get; }

    /// <summary>
    /// Gets the player settings.
    /// </summary>
    PlayerSettings[] PlayerSettings { get; }

    /// <summary>
    /// Gets the game settings.
    /// </summary>
    IGameSettingsManager GameSettings { get; }

    /// <summary>
    /// Gets the audio manager.
    /// </summary>
    AudioManager AudioManager { get; }

    /// <summary>
    /// Updates the specified game time.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    void Update(GameTime gameTime);

    /// <summary>
    /// Draws this instance.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    void Draw(GameTime gameTime);

    /// <summary>
    /// Loads the content.
    /// </summary>
    void LoadContent();

    /// <summary>
    /// Unloads the content.
    /// </summary>
    void UnloadContent();
  }
}
