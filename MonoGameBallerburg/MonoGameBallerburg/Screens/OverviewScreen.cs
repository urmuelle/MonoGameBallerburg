// <copyright file="OverviewScreen.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Screens
{
  using System;
  using Controls;
  using Manager;
  using Microsoft.Xna.Framework;

  public class OverviewScreen : MenuScreen
  {
    private readonly MenuEntry zurueckMenuEntry;

    public OverviewScreen(IBallerburgGraphicsManager graphicsManager, IScreenManager screenManager)
      : base(graphicsManager, screenManager, "Ballerburg3D")
    {
      // Zurück button
      zurueckMenuEntry = new MenuEntry(this, ResourceLoader.GetString("BackText"), 0) { Position = new Vector2(500, 450) };
      zurueckMenuEntry.Selected += ZurueckMenuEntrySelected;

      ControlsContainer.Add(zurueckMenuEntry);
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

    #region Event Handlers

    /// <summary>
    /// When the user presses this button, we go on to the messagebox screen
    /// asking for the gamestyle he wants to play
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void ZurueckMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
      ExitScreen();
    }

    #endregion
  }
}