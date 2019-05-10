// <copyright file="EinstellungenScreen.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Screens
{
  using System;
  using System.Globalization;
  using Microsoft.Xna.Framework;
  using MonoGameBallerburg.Controls;
  using MonoGameBallerburg.Manager;

  /// <summary>
  /// The screen, where money, population and bying new items is handled
  /// TODO: Use Translation Keys
  /// TODO: Do not hardcode the prizes.
  /// </summary>
  public class EinstellungenScreen : MenuScreen
  {
    private readonly MenuEntry weiterMenuEntry;
    private readonly MenuEntry beendenMenuEntry;
    private readonly GamePlayMenuLabel lblInventar;
    private readonly GamePlayMenuLabel lblMoney;
    private readonly GamePlayMenuLabel lblCannons;
    private readonly GamePlayMenuLabel lblStone;
    private readonly GamePlayMenuLabel lblIron;
    private readonly GamePlayMenuLabel lblBevoelkerung;
    private readonly GamePlayMenuLabel lblNumBevoelkerung;
    private readonly GamePlayMenuLabel lblVerzinsung;
    private readonly GamePlayMenuLabel lblZinssatz;
    private readonly GamePlayMenuLabel lblKaufen;
    private readonly HSlider sldVerzinsung;
    private readonly ActionToggleButton atbCannon;
    private readonly ActionToggleButton atbPulver;
    private readonly ActionToggleButton atbSteinkugel;
    private readonly ActionToggleButton atbEisenkugel;

    private readonly int playerId;

    /// <summary>
    /// Initializes a new instance of the <see cref="EinstellungenScreen"/> class.
    /// </summary>
    /// <param name="graphicsManager">The graphics manager.</param>
    /// <param name="screenManager">The screen manager.</param>
    /// <param name="currentPlayerId">The current player ID.</param>
    public EinstellungenScreen(IBallerburgGraphicsManager graphicsManager, IScreenManager screenManager, int currentPlayerId)
      : base(graphicsManager, screenManager, "Ballerburg3D")
    {
      playerId = currentPlayerId;

      lblInventar = new GamePlayMenuLabel(this, 0, graphicsManager) { Text = string.Format("{0}:", "Inventar"), Position = new Vector2(20, 70) };
      lblBevoelkerung = new GamePlayMenuLabel(this, 0, graphicsManager) { Text = "Bevölkerung:", Position = new Vector2(20, 170) };
      lblVerzinsung = new GamePlayMenuLabel(this, 0, graphicsManager) { Text = "Verzinsung:", Position = new Vector2(20, 220) };
      lblKaufen = new GamePlayMenuLabel(this, 0, graphicsManager) { Text = "Kaufen:", Position = new Vector2(20, 310) };

      // Inventar
      lblMoney = new GamePlayMenuLabel(this, 0, graphicsManager) { Text = screenManager.PlayerSettings[playerId].Money.ToString(CultureInfo.InvariantCulture) + " $", Position = new Vector2(220, 70) };
      lblCannons = new GamePlayMenuLabel(this, 0, graphicsManager) { Text = screenManager.PlayerSettings[playerId].NumCannons() + " Kanonen", Position = new Vector2(420, 70) };
      lblStone = new GamePlayMenuLabel(this, 0, graphicsManager) { Text = screenManager.PlayerSettings[playerId].Steinkugel.ToString(CultureInfo.InvariantCulture) + " Steinkugeln", Position = new Vector2(220, 120) };
      lblIron = new GamePlayMenuLabel(this, 0, graphicsManager) { Text = screenManager.PlayerSettings[playerId].Eisenkugeln.ToString(CultureInfo.InvariantCulture) + " Eisenkugeln", Position = new Vector2(420, 120) };

      // Bevölkerung
      lblNumBevoelkerung = new GamePlayMenuLabel(this, 0, graphicsManager) { Text = screenManager.PlayerSettings[playerId].Population.ToString(CultureInfo.InvariantCulture) + " Leute", Position = new Vector2(220, 170) };

      // Verzinsing
      sldVerzinsung = new HSlider(this, new Rectangle(220, 240, 300, 20), screenManager.PlayerSettings[playerId].Verzinsung);
      sldVerzinsung.ValueChanged += OnVerzinsungChanged;
      sldVerzinsung.MaxValue = 100;
      sldVerzinsung.MinValue = 0;
      sldVerzinsung.Value = screenManager.PlayerSettings[playerId].Verzinsung;
      lblZinssatz = new GamePlayMenuLabel(this, 0, graphicsManager) { Text = string.Format("{0}%", screenManager.PlayerSettings[playerId].Verzinsung), Position = new Vector2(340, 260) };

      // Kaufbuttons
      atbCannon = new ActionToggleButton(this, "Kanone", "4000$", 0) { Position = new Vector2(220, 310) };
      atbCannon.Selected += (sender, e) =>
                                {
                                  screenManager.AudioManager.PlayKlickSound();
                                  screenManager.PlayerSettings[playerId].Money -= 4000;
                                  screenManager.PlayerSettings[playerId].AddCannon();
                                };

      atbPulver = new ActionToggleButton(this, "1000g Pulver", "1000$", 0) { Position = new Vector2(420, 310) };
      atbPulver.Selected += (sender, e) =>
                                {
                                  screenManager.AudioManager.PlayKlickSound();
                                  screenManager.PlayerSettings[playerId].Money -= 1000;
                                  screenManager.PlayerSettings[playerId].AddPowder();
                                };
      atbEisenkugel = new ActionToggleButton(this, "Eisenkugel", "1000$", 0) { Position = new Vector2(420, 360) };
      atbEisenkugel.Selected += (sender, e) =>
                                    {
                                      screenManager.AudioManager.PlayKlickSound();
                                      screenManager.PlayerSettings[playerId].Money -= 1000;
                                      screenManager.PlayerSettings[playerId].AddIronBall();
                                    };
      atbSteinkugel = new ActionToggleButton(this, "Steinkugel", "250$", 0) { Position = new Vector2(220, 360) };
      atbSteinkugel.Selected += (sender, e) =>
                                    {
                                      screenManager.AudioManager.PlayKlickSound();
                                      screenManager.PlayerSettings[playerId].Money -= 250;
                                      screenManager.PlayerSettings[playerId].AddStoneBall();
                                    };

      // Weiter button
      weiterMenuEntry = new MenuEntry(this, "Weiter", 0) { Position = new Vector2(350, 450) };
      weiterMenuEntry.Selected += WeiterMenuEntrySelected;

      // Beenden button
      beendenMenuEntry = new MenuEntry(this, "Beenden", 0) { Position = new Vector2(500, 450) };
      beendenMenuEntry.Selected += BeendenMenuEntrySelected;

      ControlsContainer.Add(weiterMenuEntry);
      ControlsContainer.Add(beendenMenuEntry);
      ControlsContainer.Add(lblInventar);
      ControlsContainer.Add(lblMoney);
      ControlsContainer.Add(lblCannons);
      ControlsContainer.Add(lblBevoelkerung);
      ControlsContainer.Add(lblNumBevoelkerung);
      ControlsContainer.Add(lblVerzinsung);
      ControlsContainer.Add(lblKaufen);
      ControlsContainer.Add(lblStone);
      ControlsContainer.Add(lblIron);
      ControlsContainer.Add(atbCannon);
      ControlsContainer.Add(atbEisenkugel);
      ControlsContainer.Add(atbPulver);
      ControlsContainer.Add(atbSteinkugel);
      ControlsContainer.Add(sldVerzinsung);
      ControlsContainer.Add(lblZinssatz);
    }

    /// <summary>
    /// Updates the menu - business rules for buying things in this game.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    /// <param name="otherScreenHasFocus">if set to <c>true</c> [other screen has focus].</param>
    /// <param name="coveredByOtherScreen">if set to <c>true</c> [covered by other screen].</param>
    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
      lblMoney.Text = ScreenManager.PlayerSettings[playerId].Money.ToString(CultureInfo.InvariantCulture) + " $";
      lblCannons.Text = ScreenManager.PlayerSettings[playerId].NumCannons() + " Kanonen";
      lblStone.Text = ScreenManager.PlayerSettings[playerId].Steinkugel.ToString(CultureInfo.InvariantCulture) + " Steinkugeln";
      lblIron.Text = ScreenManager.PlayerSettings[playerId].Eisenkugeln.ToString(CultureInfo.InvariantCulture) + " Eisenkugeln";

      if (ScreenManager.PlayerSettings[playerId].Money < 4000)
      {
        if (!atbCannon.IsInactive)
        {
          atbCannon.SetInactive();
        }
      }

      if (ScreenManager.PlayerSettings[playerId].Money >= 4000)
      {
        if (atbCannon.IsInactive)
        {
          atbCannon.SetActive();
        }
        ////atbCannon.Enabled = true;
      }

      if (ScreenManager.PlayerSettings[playerId].Money < 1000)
      {
        if (!atbPulver.IsInactive)
        {
          atbPulver.SetInactive();
          atbEisenkugel.SetInactive();
        }
      }

      if (ScreenManager.PlayerSettings[playerId].Money >= 1000)
      {
        if (atbPulver.IsInactive)
        {
          atbPulver.SetActive();
          atbEisenkugel.SetActive();
        }
      }

      if (ScreenManager.PlayerSettings[playerId].Money < 250)
      {
        if (!atbSteinkugel.IsInactive)
        {
          atbSteinkugel.SetInactive();
        }
      }

      if (ScreenManager.PlayerSettings[playerId].Money >= 250)
      {
        if (atbSteinkugel.IsInactive)
        {
          atbSteinkugel.SetActive();
        }
      }

      if (ScreenManager.PlayerSettings[playerId].NumCannons() == ScreenManager.PlayerSettings[playerId].MaxNumCannons())
      {
        if (!atbCannon.IsInactive)
        {
          atbCannon.SetInactive();
        }
      }
    }

    #region EventHandlers

    /// <summary>
    /// Called when [verzinsung changed].
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="Ballerburg.Controls.SliderChangedEventArgs"/> instance containing the event data.</param>
    private void OnVerzinsungChanged(object sender, SliderChangedEventArgs e)
    {
      ScreenManager.PlayerSettings[playerId].Verzinsung = (short)e.Value;
      lblZinssatz.Text = string.Format("{0}%", (short)e.Value);
    }

    #endregion

    /// <summary>
    /// When the user presses this button, we go on to the messagebox screen
    /// asking for the gamestyle he wants to play.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void WeiterMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
      ExitScreen();
    }

    /// <summary>
    /// When the user presses this button, we go on to the messagebox screen
    /// asking for the gamestyle he wants to play.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void BeendenMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();

      var spielBeendenMessageBoxScreen = new SpielBeendenMessageBoxScreen(GraphicsManager, "Beenden");
      ScreenManager.AddScreen(spielBeendenMessageBoxScreen);
      ScreenManager.RemoveScreen(this);
    }
  }
}
