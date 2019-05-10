// <copyright file="EnterNameDialogScreen.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Screens
{
  using System;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;
  using MonoGameBallerburg.Controls;
  using MonoGameBallerburg.Gameplay;
  using MonoGameBallerburg.Manager;

  /// <summary>
  /// Dialog zur Eingabe des Spielernamens.
  /// </summary>
  public class EnterNameDialogScreen : GameScreen
  {
    private readonly MenuEntry zurueckMenuEntry;
    private readonly TextBox textBox;
    private readonly string boxTitle;

    private Texture2D gradientTexture;

    private PlayerSettings playerSettings;

    public EnterNameDialogScreen(IBallerburgGraphicsManager graphicManager, string boxTitle, PlayerSettings playerSettings)
      : base(graphicManager)
    {
      this.playerSettings = playerSettings;
      zurueckMenuEntry = new MenuEntry(this, "Zurück", 1) { Position = new Vector2(360, 360) };
      zurueckMenuEntry.Selected += ZurueckExitMessageBoxAccepted;

      textBox = new TextBox(this, true)
                    {
                      Position = new Vector2(200, 250),
                      Text = playerSettings.PlayerName,
                      ShowCursor = true,
                    };
      textBox.SetFocus();

      ControlsContainer.Add(zurueckMenuEntry);
      ControlsContainer.Add(textBox);

      this.boxTitle = boxTitle;

      IsPopup = true;

      TransitionOffTime = TimeSpan.FromSeconds(0.2);
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

    /// <summary>
    /// Draws the message box.
    /// </summary>
    /// <param name="gameTime">Game time passed.</param>
    public override void Draw(GameTime gameTime)
    {
      SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

      // Darken down any other screens that were drawn beneath the popup.
      ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

      // Center the message text in the viewport.
      gradientTexture = ScreenManager.ContentManager.GradientTexture;

      // The background includes a border somewhat larger than the text itself.
      ////const int hPad = 32;
      ////const int vPad = 16;

      var backgroundRectangle = new Rectangle(160, 90, 320, 300);

      // Fade the popup alpha during transitions.
      var color = new Color((byte)255, (byte)255, (byte)255, TransitionAlpha);

      spriteBatch.Begin();

      // Draw the background rectangle.
      spriteBatch.Draw(gradientTexture, backgroundRectangle, color);

      spriteBatch.End();

      DrawTitle();
      base.Draw(gameTime);
    }

    #endregion

    /// <summary>
    /// Draws the title.
    /// </summary>
    public void DrawTitle()
    {
      var spriteBatch = ScreenManager.SpriteBatch;
      var font = ScreenManager.Font;

      // Draw the menu title.
      var titlePosition = 1.25f * (font.MeasureString(boxTitle) / 2);
      titlePosition.X += 170;
      titlePosition.Y += 85;
      var titleOrigin = font.MeasureString(boxTitle) / 2;
      var titleColor = Color.Blue;
      const float TitleScale = 1.25f;

      spriteBatch.Begin();

      spriteBatch.DrawString(font, boxTitle, titlePosition, titleColor, 0, titleOrigin, TitleScale, SpriteEffects.None, 0);

      spriteBatch.End();
    }

    #region EventHandlers

    /// <summary>
    /// Event handler for when the user selects ok on the "are you sure
    /// you want to exit" message box.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void ZurueckExitMessageBoxAccepted(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
      playerSettings.PlayerName = textBox.Text;
      var spielerDialog = new SpielerDialogScreen(GraphicsManager, ScreenManager, 1, playerSettings);
      ScreenManager.AddScreen(spielerDialog);
      ExitScreen();
    }

    #endregion
  }
}
