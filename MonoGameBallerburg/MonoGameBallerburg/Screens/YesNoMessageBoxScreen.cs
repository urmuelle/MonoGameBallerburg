// <copyright file="YesNoMessageBoxScreen.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Screens
{
  using System;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;
  using MonoGameBallerburg.Controls;
  using MonoGameBallerburg.Manager;

  [System.Diagnostics.CodeAnalysis.SuppressMessage(
  "Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Rule does not apply here")]
  public class YesNoMessageBoxScreen : GameScreen
  {
    #region Fields

    private readonly string message;
    private readonly int selectedEntry;
    private readonly string boxTitle;
    private readonly MenuEntry jaMenuEntry;
    private readonly MenuEntry neinMenuEntry;

    private Texture2D gradientTexture;

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes a new instance of the <see cref="YesNoMessageBoxScreen"/> class.
    /// </summary>
    /// <param name="graphicsManager">The graphics manager.</param>
    /// <param name="boxTitle">The box title.</param>
    /// <param name="message">The message.</param>
    public YesNoMessageBoxScreen(IBallerburgGraphicsManager graphicsManager, string boxTitle, string message)
      : base(graphicsManager)
    {
      jaMenuEntry = new MenuEntry(this, "Ja", 0) { Position = new Vector2(320, 360) };
      jaMenuEntry.Selected += ConfirmExitMessageBoxAccepted;

      neinMenuEntry = new MenuEntry(this, "Nein", 1) { Position = new Vector2(380, 360) };
      neinMenuEntry.Selected += CancelExitMessageBoxAccepted;

      ControlsContainer.Add(jaMenuEntry);
      ControlsContainer.Add(neinMenuEntry);

      this.boxTitle = boxTitle;

      this.message = message;

      IsPopup = true;

      selectedEntry = -1;

      TransitionOffTime = TimeSpan.FromSeconds(0.2);
    }

    #endregion

    #region Events

    public event EventHandler<EventArgs> Accepted;

    public event EventHandler<EventArgs> Cancelled;

    #endregion

    /// <summary>
    /// Loads graphics content for this screen. This uses the shared ContentManager
    /// provided by the Game class, so the content will remain loaded forever.
    /// Whenever a subsequent MessageBoxScreen tries to load this same content,
    /// it will just get back another reference to the already loaded data.
    /// </summary>
    public override void LoadContent()
    {
      gradientTexture = ScreenManager.ContentManager.GradientTexture;
    }

    #region Handle Input

    /// <summary>
    /// Responds to user input, changing the selected entry and accepting
    /// or cancelling the menu.
    /// </summary>
    /// <param name="input">The input.</param>
    public override void HandleInput(InputState input)
    {
      /*
      if (ScreenManager.GameMousePointer.MouseButtonLeftClicked)
      {
          if (selectedEntry > -1)
              OnSelectEntry(selectedEntry);
      }
       * */
    }

    /// <summary>
    /// Handler for when the user has chosen a menu entry.
    /// </summary>
    /// <param name="entryIndex">Index of the entry.</param>
    public virtual void OnSelectEntry(int entryIndex)
    {
      if (selectedEntry == 0)
      {
        if (Accepted != null)
        {
          Accepted(this, EventArgs.Empty);
        }

        ExitScreen();
      }
      else
      {
        if (Cancelled != null)
        {
          Cancelled(this, EventArgs.Empty);
        }

        ExitScreen();
      }
    }

    #endregion

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
        /*
        // Draw each menu entry in turn.
        selectedEntry = -1;

        for (int i = 0; i < menuEntries.Count; i++)
        {
            MenuEntry menuEntry = menuEntries[i];

            bool isSelected = menuEntry.IsSelected(this, ScreenManager.GameMousePointer.MousePosition);

            if (isSelected && !(ScreenManager.GameMousePointer.MouseButtonLeftPressed))
                selectedEntry = menuEntry.Id;
        }
         * */
      }

      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }

    /// <summary>
    /// Draws the message box.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public override void Draw(GameTime gameTime)
    {
      var spriteBatch = ScreenManager.SpriteBatch;
      var font = ScreenManager.Font;

      // Darken down any other screens that were drawn beneath the popup.
      ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

      // Center the message text in the viewport.
      var viewport = GraphicsManager.GraphicsDevice.Viewport;
      var viewportSize = new Vector2(viewport.Width, viewport.Height);
      var textSize = font.MeasureString(message);
      var textPosition = (viewportSize - textSize) / 2;

      // The background includes a border somewhat larger than the text itself.
      ////const int hPad = 32;
      ////const int vPad = 16;

      var backgroundRectangle = new Rectangle(160, 90, 320, 300);

      // Fade the popup alpha during transitions.
      var color = new Color((byte)255, (byte)255, (byte)255, TransitionAlpha);

      spriteBatch.Begin();

      // Draw the background rectangle.
      spriteBatch.Draw(gradientTexture, backgroundRectangle, color);

      // Draw the message box text.
      spriteBatch.DrawString(font, message, textPosition, color);

      spriteBatch.End();

      /*
      // Draw each menu entry in turn.
      for (int i = 0; i < menuEntries.Count; i++)
      {
          MenuEntry menuEntry = menuEntries[i];

          bool isSelected = menuEntry.IsSelected(this, ScreenManager.GameMousePointer.MousePosition);

          if (isSelected && !(ScreenManager.GameMousePointer.MouseButtonLeftPressed)) { }
          //selectedEntry = menuEntry.Id;
          else
          {
              //selectedEntry = -1;
              isSelected = false;
          }

          menuEntry.Draw(this, ScreenManager.GameMousePointer, isSelected, gameTime);
      }
       * */

      DrawTitle();

      base.Draw(gameTime);
    }

    /// <summary>
    /// Draws the title.
    /// </summary>
    private void DrawTitle()
    {
      var spriteBatch = ScreenManager.SpriteBatch;
      var font = ScreenManager.Font;

      // Draw the menu title.
      var titlePosition = 1.25f * (font.MeasureString(boxTitle) / 2); ////new Vector2(320, 10);
      titlePosition.X += 170;
      titlePosition.Y += 85;
      var titleOrigin = font.MeasureString(boxTitle) / 2;
      var titleColor = Color.Blue; ////new Color(192, 192, 192, TransitionAlpha);
      const float TitleScale = 1.25f;

      spriteBatch.Begin();

      spriteBatch.DrawString(
          font,
          boxTitle,
          titlePosition,
          titleColor,
          0,
          titleOrigin,
          TitleScale,
          SpriteEffects.None,
          0);

      spriteBatch.End();
    }

    #endregion

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

    /// <summary>
    /// Event handler for when the user selects ok on the "are you sure
    /// you want to exit" message box.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void CancelExitMessageBoxAccepted(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
      ExitScreen();
    }
  }
}
