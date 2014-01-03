// <copyright file="SpielartMessageBoxScreen.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Screens
{
  using System;
  using Controls;
  using Manager;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  public class SpielartMessageBoxScreen : GameScreen
  {
    #region Fields

    private readonly string boxTitle;

    private readonly MenuEntry klassischMenuEntry;
    private readonly MenuEntry arkadeMenuEntry;
    private readonly MenuEntry zurueckMenuEntry;

    private Texture2D gradientTexture;

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes a new instance of the <see cref="SpielartMessageBoxScreen"/> class.
    /// </summary>
    /// <param name="graphicManager">The graphic manager.</param>
    /// <param name="boxTitle">The box title.</param>
    public SpielartMessageBoxScreen(IBallerburgGraphicsManager graphicManager, string boxTitle)
      : base(graphicManager)
    {
      klassischMenuEntry = new MenuEntry(this, "Klassisch", 0) { Position = new Vector2(200, 200) };
      klassischMenuEntry.Selected += KlassischMenuEntrySelected;
      arkadeMenuEntry = new MenuEntry(this, "Arkade", 1) { Position = new Vector2(200, 250) };
      arkadeMenuEntry.Selected += ArkadeMenuEntrySelected;
      zurueckMenuEntry = new MenuEntry(this, ResourceLoader.GetString("BackText"), 1) { Position = new Vector2(360, 360) };
      zurueckMenuEntry.Selected += ZurueckExitMessageBoxAccepted;

      ControlsContainer.Add(klassischMenuEntry);
      ControlsContainer.Add(arkadeMenuEntry);
      ControlsContainer.Add(zurueckMenuEntry);

      this.boxTitle = boxTitle;

      IsPopup = true;

      TransitionOffTime = TimeSpan.FromSeconds(0.2);
    }

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
    /// <param name="gameTime">Game time passed</param>
    public override void Draw(GameTime gameTime)
    {
      var spriteBatch = ScreenManager.SpriteBatch;

      // Darken down any other screens that were drawn beneath the popup.
      ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

      // Center the message text in the viewport.
      gradientTexture = ScreenManager.ContentManager.GradientTexture;

      var backgroundRectangle = new Rectangle(160, 90, 320, 300);

      // Fade the popup alpha during transitions.
      var color = new Color(255, 255, 255, TransitionAlpha);

      spriteBatch.Begin();

      // Draw the background rectangle.
      spriteBatch.Draw(gradientTexture, backgroundRectangle, color);

      spriteBatch.End();

      ////spriteBatch.Begin();

      DrawTitle();
      base.Draw(gameTime);
    }

    #endregion

    /// <summary>
    /// Draws the title.
    /// </summary>
    private void DrawTitle()
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

    /// <summary>
    /// Event handler for when the user selects ok on the "are you sure
    /// you want to exit" message box.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void ZurueckExitMessageBoxAccepted(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
      var startScreen = new StartScreen(GraphicsManager, ScreenManager);
      ScreenManager.AddScreen(startScreen);
      ExitScreen();
    }

    /// <summary>
    /// Arkades the menu entry selected.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void ArkadeMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
      ScreenManager.GameSettings.GameStyle = Gameplay.GameType.Arkade;
      var gameScreen = new GameplayScreen(GraphicsManager);
      ScreenManager.AddScreen(gameScreen);
      ExitScreen();
    }

    /// <summary>
    /// Klassisches the menu entry selected.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void KlassischMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
      ScreenManager.GameSettings.GameStyle = Gameplay.GameType.Klassisch;
      var mainMenuScreen = new MainMenuScreen(GraphicsManager, ScreenManager);
      ScreenManager.AddScreen(mainMenuScreen);
      ExitScreen();
    }
  }
}