// <copyright file="MainMenuScreen.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Screens
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using Controls;
  using Manager;
  using Microsoft.Xna.Framework;

  /// <summary>
  /// The games Main menu, shown, before gameplay starts
  /// </summary>
  public class MainMenuScreen : MenuScreen
  {
    private readonly MenuEntry spielStartenMenuEntry;
    private readonly MenuEntry zurueckMenuEntry;
    private readonly MenuEntry spielOptionenMenuEntry;
    private readonly MenuEntry soundMenuEntry;
    private readonly MenuEntry creditsMenuEntry;
    private readonly MenuEntry graphikMenuEntry;
    private readonly ComboToggleButton anzahlSpielerToggleButton;
    private readonly ActionToggleButton[] spielerActionButtons;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainMenuScreen"/> class.
    /// </summary>
    /// <param name="graphicsManager">The graphics manager.</param>
    /// <param name="screenManager">The screen manager.</param>
    public MainMenuScreen(IBallerburgGraphicsManager graphicsManager, IScreenManager screenManager)
      : base(graphicsManager, screenManager, "Ballerburg3D")
    {
      // Create the menu entries.
      // Starten Button
      spielStartenMenuEntry = new MenuEntry(this, "starten", 0) { Position = new Vector2(350, 450) };
      spielStartenMenuEntry.Selected += SpielStartenMenuEntrySelected;

      // Zurück button
      zurueckMenuEntry = new MenuEntry(this, "Zurück", 0) { Position = new Vector2(500, 450) };
      zurueckMenuEntry.Selected += ZurueckMenuEntrySelected;

      // Anzahl Spieler Buttons
      var numPlayerList = new List<string> { "Anzahl: 2", "Anzahl: 3", "Anzahl: 4" };

      anzahlSpielerToggleButton = new ComboToggleButton(this, "Spieler", new Collection<string>(numPlayerList), 0, 0)
                                      {
                                        Position = new Vector2(10, 100)
                                      };
      anzahlSpielerToggleButton.Selected += ToggleNumPlayersMenuEntrySelected;

      spielOptionenMenuEntry = new MenuEntry(this, "Optionen", 0) { Position = new Vector2(10, 250) };
      spielOptionenMenuEntry.Selected += SpielOptionenMenuEntrySelected;

      soundMenuEntry = new MenuEntry(this, "Sound", 0) { Position = new Vector2(220, 250) };
      soundMenuEntry.Selected += SoundMenuEntrySelected;

      creditsMenuEntry = new MenuEntry(this, "Credits", 0) { Position = new Vector2(430, 250) };
      creditsMenuEntry.Selected += CreditsMenuEntrySelected;

      graphikMenuEntry = new MenuEntry(this, "Graphik", 0) { Position = new Vector2(220, 350) };
      graphikMenuEntry.Selected += GraphikMenuEntrySelected;

      spielerActionButtons = new ActionToggleButton[4];

      spielerActionButtons[0] = new ActionToggleButton(this, "Spieler 1", screenManager.PlayerSettings[0].PlayerName, 0)
                                    {
                                      Position = new Vector2(220, 100)
                                    };
      spielerActionButtons[0].Selected += Spieler1KonfigurierenMenuEntrySelected;
      screenManager.PlayerSettings[0].IsActive = true;

      spielerActionButtons[1] = new ActionToggleButton(this, "Spieler 2", screenManager.PlayerSettings[1].PlayerName, 0)
                                    {
                                      Position = new Vector2(430, 100)
                                    };
      spielerActionButtons[1].Selected += Spieler2KonfigurierenMenuEntrySelected;
      screenManager.PlayerSettings[1].IsActive = true;

      spielerActionButtons[2] = new ActionToggleButton(this, "Spieler 3", screenManager.PlayerSettings[2].PlayerName, 0)
                                    {
                                      Position = new Vector2(220, 150)
                                    };
      spielerActionButtons[2].Selected += Spieler3KonfigurierenMenuEntrySelected;
      spielerActionButtons[2].SetInactive();
      screenManager.PlayerSettings[2].IsActive = false;

      spielerActionButtons[3] = new ActionToggleButton(this, "Spieler 4", screenManager.PlayerSettings[3].PlayerName, 0)
                                    {
                                      Position = new Vector2(430, 150)
                                    };
      spielerActionButtons[3].Selected += Spieler4KonfigurierenMenuEntrySelected;
      spielerActionButtons[3].SetInactive();
      screenManager.PlayerSettings[3].IsActive = false;

      for (int i = 0; i < GameSettings.NumPlayers; i++)
      {
        spielerActionButtons[i].SetActive();
      }

      for (int i = GameSettings.NumPlayers; i < 4; i++)
      {
        spielerActionButtons[i].SetInactive();
      }

      ControlsContainer.Add(spielStartenMenuEntry);
      ControlsContainer.Add(zurueckMenuEntry);
      ControlsContainer.Add(anzahlSpielerToggleButton);
      ControlsContainer.Add(spielerActionButtons[0]);
      ControlsContainer.Add(spielerActionButtons[1]);
      ControlsContainer.Add(spielerActionButtons[2]);
      ControlsContainer.Add(spielerActionButtons[3]);
      ControlsContainer.Add(spielOptionenMenuEntry);
      ControlsContainer.Add(soundMenuEntry);
      ControlsContainer.Add(creditsMenuEntry);
      ControlsContainer.Add(graphikMenuEntry);

      ScreenActivated += OnScreenActivated;
    }

    /// <summary>
    /// When the user presses this button, we go on to the messagebox screen
    /// asking for the gamestyle he wants to play
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void SpielStartenMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
      ScreenManager.AudioManager.StopMenuBackgroundMusic();
      var gameScreen = new GameplayScreen(GraphicsManager);
      ScreenManager.AddScreen(gameScreen);
      ExitScreen();
    }

    /// <summary>
    /// When the user presses this button, we go on to the messagebox screen
    /// asking for the gamestyle he wants to play
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void ZurueckMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
      var startScreen = new StartScreen(GraphicsManager, ScreenManager);
      ScreenManager.AddScreen(startScreen);
      ExitScreen();
    }

    /// <summary>
    /// When the user presses this button, we go on to the screen for
    /// the network connection
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void ToggleNumPlayersMenuEntrySelected(object sender, ActionToggleButtonEventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();

      GameSettings.NumPlayers = e.SelectedIndex + 2;

      for (var i = 0; i < GameSettings.NumPlayers; i++)
      {
        spielerActionButtons[i].SetActive();
      }

      for (var i = GameSettings.NumPlayers; i < 4; i++)
      {
        spielerActionButtons[i].SetInactive();
      }
    }

    /// <summary>
    /// When the user presses this button, we go on to the screen for
    /// the network connection
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void Spieler1KonfigurierenMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();

      var spielerDialog = new SpielerDialogScreen(GraphicsManager, ScreenManager, 1, ScreenManager.PlayerSettings[0]);
      ScreenManager.AddScreen(spielerDialog);
    }

    /// <summary>
    /// When the user presses this button, we go on to the screen for
    /// the network connection
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void Spieler2KonfigurierenMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();

      var spielerDialog = new SpielerDialogScreen(GraphicsManager, ScreenManager, 2, ScreenManager.PlayerSettings[1]);
      ScreenManager.AddScreen(spielerDialog);
    }

    /// <summary>
    /// When the user presses this button, we go on to the screen for
    /// the network connection
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void Spieler3KonfigurierenMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();

      var spielerDialog = new SpielerDialogScreen(GraphicsManager, ScreenManager, 3, ScreenManager.PlayerSettings[2]);
      ScreenManager.AddScreen(spielerDialog);
    }

    /// <summary>
    /// When the user presses this button, we go on to the screen for
    /// the network connection
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void Spieler4KonfigurierenMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();

      var spielerDialog = new SpielerDialogScreen(GraphicsManager, ScreenManager, 4, ScreenManager.PlayerSettings[3]);
      ScreenManager.AddScreen(spielerDialog);
    }

    /// <summary>
    /// Spiels the optionen menu entry selected.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void SpielOptionenMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
      var optionenScreen = new SpielOptionenScreen(GraphicsManager, ScreenManager);
      ScreenManager.AddScreen(optionenScreen);
    }

    /// <summary>
    /// Sounds the menu entry selected.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void SoundMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
      var soundScreen = new SoundScreen(GraphicsManager, ScreenManager);
      ScreenManager.AddScreen(soundScreen);
    }

    /// <summary>
    /// Graphiks the menu entry selected.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void GraphikMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
      var graphikScreen = new GraphikScreen(GraphicsManager, ScreenManager);
      ScreenManager.AddScreen(graphikScreen);
    }

    /// <summary>
    /// Creditses the menu entry selected.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void CreditsMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
      var creditsScreen = new CreditsScreen(GraphicsManager, ScreenManager);
      ScreenManager.AddScreen(creditsScreen);
    }

    /// <summary>
    /// Handler that is being called, when the screen is activated when it had been covered before
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void OnScreenActivated(object sender, EventArgs e)
    {
      spielerActionButtons[0].StateText = ScreenManager.PlayerSettings[0].PlayerName;
      spielerActionButtons[1].StateText = ScreenManager.PlayerSettings[1].PlayerName;
      spielerActionButtons[2].StateText = ScreenManager.PlayerSettings[2].PlayerName;
      spielerActionButtons[3].StateText = ScreenManager.PlayerSettings[3].PlayerName;
    }
  }
}
