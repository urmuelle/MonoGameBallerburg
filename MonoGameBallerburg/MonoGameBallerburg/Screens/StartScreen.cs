// <copyright file="StartScreen.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Screens
{
  using System;
  using System.Globalization;
  using Controls;
  using Manager;
  using Microsoft.Xna.Framework;

  /// <summary>
  /// The game's Start Screen
  /// </summary>
  public class StartScreen : MenuScreen
  {
    private readonly MenuEntry spielStartenMenuEntry;
    private readonly MenuEntry spielerVerbindenMenuEntry;
    private readonly MenuEntry beendenMenuEntry;

    /// <summary>
    /// Initializes a new instance of the <see cref="StartScreen"/> class.
    /// </summary>
    /// <param name="graphicsManager">The graphics manager.</param>
    /// <param name="screenManager">The screen manager.</param>
    public StartScreen(IBallerburgGraphicsManager graphicsManager, IScreenManager screenManager)
      : base(graphicsManager, screenManager, "Ballerburg3D")
    {
      // Create our menu entries.
      spielStartenMenuEntry = new MenuEntry(this, "Spiel starten", 0) { Position = new Vector2(100, 150) };
      spielerVerbindenMenuEntry = new MenuEntry(this, "Spieler verbinden", 1) { Position = new Vector2(100, 150 + 50) };
      beendenMenuEntry = new MenuEntry(this, "Beenden", 2) { Position = new Vector2(100, 150 + 100) };

      // Hook up menu event handlers.
      spielStartenMenuEntry.Selected += SpielStartenMenuEntrySelected;
      spielerVerbindenMenuEntry.Selected += SpielerVerbindenMenuEntrySelected;
      beendenMenuEntry.Selected += OnCancel;

      ControlsContainer.Add(spielStartenMenuEntry);
      ControlsContainer.Add(spielerVerbindenMenuEntry);
      ControlsContainer.Add(beendenMenuEntry);
    }

    /// <summary>
    /// When the user cancels the main menu, ask if they want to exit the sample.
    /// </summary>
    protected override void OnCancel()
    {
      ScreenManager.AudioManager.PlayKlickSound();

      const string Message = "Wollen Sie\nBallerburg3D wirklich\nBeenden?";

      var confirmExitMessageBox = new YesNoMessageBoxScreen(GraphicsManager, "Beenden", Message);
      confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

      ScreenManager.AddScreen(confirmExitMessageBox);
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
      /*
      GameplayMenuScreen gameMenuScreen = new GameplayMenuScreen();
      ScreenManager.AddScreen(gameMenuScreen);
      GameplayScreen gameScreen = new GameplayScreen(gameMenuScreen);
      ScreenManager.AddScreen(gameScreen);
       * */
      InitGameSettings();
      var spielartMessageBoxScreen = new SpielartMessageBoxScreen(GraphicsManager, "Spielart");
      ScreenManager.AddScreen(spielartMessageBoxScreen);
      ScreenManager.RemoveScreen(this);
    }

    /// <summary>
    /// Inits the game settings.
    /// </summary>
    private void InitGameSettings()
    {
      GameSettings.NumPlayers = 2;

      for (int i = 0; i < 4; i++)
      {
        ScreenManager.PlayerSettings[i] = new Gameplay.PlayerSettings
                                               {
                                                 PlayerName = string.Format("Spieler {0}", (i + 1).ToString(CultureInfo.InvariantCulture)),
                                                 PlayerType = Gameplay.PlayerType.Human
                                               };
        ScreenManager.PlayerSettings[i].Castle.CastleType = 1;
      }

      // TODO: Startposition in bestimmtem Radius randomisieren.
      ScreenManager.PlayerSettings[0].Castle.StartPos = new Vector3(-10, 0, 10);
      ScreenManager.PlayerSettings[1].Castle.StartPos = new Vector3(10, 0, 10);
      ScreenManager.PlayerSettings[2].Castle.StartPos = new Vector3(10, 0, -10);
      ScreenManager.PlayerSettings[3].Castle.StartPos = new Vector3(-10, 0, -10);
    }

    /// <summary>
    /// When the user presses this button, we go on to the screen for
    /// the network connection
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void SpielerVerbindenMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
    }

    /// <summary>
    /// Event handler for when the user selects ok on the "are you sure
    /// you want to exit" message box.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
    {
      ///BallerburgGame.Instance.Exit();
    }
  }
}
