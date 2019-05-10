// <copyright file="CreditsScreen.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Screens
{
  using System;
  using Microsoft.Xna.Framework;
  using MonoGameBallerburg.Controls;
  using MonoGameBallerburg.Manager;

  public class CreditsScreen : MenuScreen
  {
    private readonly MenuEntry zurueckMenuEntry;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreditsScreen"/> class.
    /// </summary>
    /// <param name="graphicsManager">The graphics manager.</param>
    /// <param name="screenManager">The screen manager.</param>
    public CreditsScreen(IBallerburgGraphicsManager graphicsManager, IScreenManager screenManager)
      : base(graphicsManager, screenManager, "Ballerburg3D")
    {
      // Zurück button
      zurueckMenuEntry = new MenuEntry(this, "Zurück", 0) { Position = new Vector2(500, 450) };
      zurueckMenuEntry.Selected += ZurueckMenuEntrySelected;

      ControlsContainer.Add(zurueckMenuEntry);
    }

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
  }
}
