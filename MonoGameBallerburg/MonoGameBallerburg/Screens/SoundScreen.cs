// <copyright file="SoundScreen.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Screens
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using Microsoft.Xna.Framework;
  using MonoGameBallerburg.Controls;
  using MonoGameBallerburg.Gameplay;
  using MonoGameBallerburg.Manager;

  public class SoundScreen : MenuScreen
  {
    private readonly MenuEntry zurueckMenuEntry;
    private readonly HSlider musicVolumeSlider;
    private readonly HSlider soundFxVolumeSlider;
    private readonly HSlider menuEffectsVolumeSlider;
    private readonly ComboToggleButton musicSelectButton;
    private readonly OnOffToggleButton toggleMenuEffectsActionButton;
    private readonly OnOffToggleButton toggleSoundEffectsActionButton;

    /// <summary>
    /// Initializes a new instance of the <see cref="SoundScreen"/> class.
    /// </summary>
    /// <param name="graphicsManager">The graphics manager.</param>
    /// <param name="screenManager">The screen manager.</param>
    public SoundScreen(IBallerburgGraphicsManager graphicsManager, IScreenManager screenManager)
      : base(graphicsManager, screenManager, "Ballerburg3D")
    {
      // Zurück button
      zurueckMenuEntry = new MenuEntry(this, "Zurück", 0) { Position = new Vector2(500, 450) };
      zurueckMenuEntry.Selected += ZurueckMenuEntrySelected;

      musicVolumeSlider = new HSlider(this, new Rectangle(250, 100, 300, 20), screenManager.ApplicationSettings.MusicVolume);
      musicVolumeSlider.ValueChanged += OnMusicVolumeChanged;
      soundFxVolumeSlider = new HSlider(this, new Rectangle(250, 200, 300, 20), screenManager.ApplicationSettings.FxVolume);
      menuEffectsVolumeSlider = new HSlider(this, new Rectangle(250, 300, 300, 20), screenManager.ApplicationSettings.MenuEffectsVolume);

      var musicList = new List<string> { "DarkStar", "High Tension", "Tentacle", "Death Row", "Boomerang", "Aus" };

      musicSelectButton = new ComboToggleButton(this, "Musik", new Collection<string>(musicList), 0, 0) { Position = new Vector2(20, 100) };
      musicSelectButton.Selected += OnMusicButtonSelected;

      toggleMenuEffectsActionButton = new OnOffToggleButton(this, "Menueeffekte", true, 0) { Position = new Vector2(20, 200) };

      toggleSoundEffectsActionButton = new OnOffToggleButton(this, "Soundeffekte", true, 0) { Position = new Vector2(20, 300) };

      ControlsContainer.Add(zurueckMenuEntry);
      ControlsContainer.Add(musicVolumeSlider);
      ControlsContainer.Add(soundFxVolumeSlider);
      ControlsContainer.Add(menuEffectsVolumeSlider);
      ControlsContainer.Add(musicSelectButton);
      ControlsContainer.Add(toggleMenuEffectsActionButton);
      ControlsContainer.Add(toggleSoundEffectsActionButton);
    }

    #region Update and Draw

    /// <summary>
    /// Updates the menu.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    /// <param name="otherScreenHasFocus">if set to <c>true</c> [other screen has focus].</param>
    /// <param name="coveredByOtherScreen">if set to <c>true</c> [covered by other screen].</param>
    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
      if (!otherScreenHasFocus)
      {
      }

      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }

    #endregion

    #region EventHandlers

    /// <summary>
    /// When the user presses this button, we go on to the messagebox screen
    /// asking for the gamestyle he wants to play.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void ZurueckMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
      ExitScreen();
    }

    /// <summary>
    /// Called when [music button selected].
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="Ballerburg.Controls.ActionToggleButtonEventArgs"/> instance containing the event data.</param>
    private void OnMusicButtonSelected(object sender, ActionToggleButtonEventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();

      int index = e.SelectedIndex;

      if (index == (int)BackgroundMusicTrack.Aus)
      {
        ScreenManager.ApplicationSettings.PlayMusic = false;
        musicVolumeSlider.State = States.Inactive;
        ScreenManager.AudioManager.StopMenuBackgroundMusic();
      }
      else
      {
        ScreenManager.ApplicationSettings.ActiveBackgroundMusicTrack = (BackgroundMusicTrack)index;

        if (ScreenManager.ApplicationSettings.PlayMusic == false)
        {
          musicVolumeSlider.State = States.Visible;
          ScreenManager.ApplicationSettings.PlayMusic = true;
          ScreenManager.AudioManager.PlayMenuBackgroundMusic();
        }
        else
        {
          ScreenManager.AudioManager.StopMenuBackgroundMusic();
          ScreenManager.AudioManager.PlayMenuBackgroundMusic();
        }
      }
    }

    /// <summary>
    /// Called when [music volume changed].
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="Ballerburg.Controls.ActionToggleButtonEventArgs"/> instance containing the event data.</param>
    private void OnMusicVolumeChanged(object sender, SliderChangedEventArgs e)
    {
      ScreenManager.AudioManager.SetMusicVolume(e.Value);
    }

    #endregion
  }
}
