// <copyright file="Hud.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Controls
{
  using System;
  using System.Globalization;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  using MonoGameBallerburg.Gameplay;

  /// <summary>
  /// Class to be used as a HUD
  /// </summary>
  public class Hud : IDisposable
  {
    private readonly GameScreen parentScreen;

    private SpriteBatch spriteBatch;

    /// <summary>
    /// Initializes a new instance of the <see cref="Hud"/> class.
    /// </summary>
    /// <param name="screen">The screen.</param>
    public Hud(GameScreen screen)
    {
      this.parentScreen = screen;
      this.spriteBatch = parentScreen.GraphicsManager.SpriteBatch;
    }

    /// <summary>
    /// Loads the content.
    /// </summary>
    public void LoadContent()
    {
    }

    #region IDisposable

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        this.spriteBatch.Dispose();
        this.spriteBatch = null;
      }
    }

    #endregion

    #region Update and Draw

    /*
        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }
        */

    /// <summary>
    /// Draws the background screen.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    /// <param name="playerId">The player id.</param>
    public void Draw(GameTime gameTime, int playerId)
    {
      // Load content belonging to the screen manager.
      var viewport = parentScreen.GraphicsManager.GraphicsDevice.Viewport;
      var fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
      var font = this.parentScreen.ScreenManager.MenuFont;

      spriteBatch.Begin();
      /*
      DrawString(font, BallerburgGame.PlayerSettings[playerId].Steinkugel.ToString(CultureInfo.InvariantCulture) + " " + Strings.StoneBallsText, new Vector2(10, 10), Color.White);
      DrawString(font, BallerburgGame.PlayerSettings[playerId].Eisenkugeln.ToString(CultureInfo.InvariantCulture) + " " + Strings.IronBallsText, new Vector2(10, 30), Color.White);
      DrawString(font, BallerburgGame.PlayerSettings[playerId].Powder.ToString(CultureInfo.InvariantCulture) + " " + Strings.PowderText, new Vector2(10, 30), Color.White);
       * */
      spriteBatch.End();
    }

    /// <summary>
    /// Lets the game respond to player input. Unlike the Update method,
    /// this will only be called when the gameplay screen is active.
    /// </summary>
    /// <param name="input">The input.</param>
    public void HandleInput(InputState input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      if (input.PauseGame)
      {
        // If they pressed pause, bring up the pause menu screen.
        ////ScreenManager.AddScreen(new PauseMenuScreen());
      }
      else
      {
      }
    }

    /// <summary>
    /// // A simple helper to draw shadowed text.
    /// </summary>
    /// <param name="font">The font value.</param>
    /// <param name="text">The text value.</param>
    /// <param name="position">The position.</param>
    /// <param name="color">The color value.</param>
    private void DrawString(SpriteFont font, string text, Vector2 position, Color color)
    {
      var origin = new Vector2(0, 0);
      this.spriteBatch.DrawString(
          font,
          text,
          new Vector2(position.X + 1, position.Y + 1),
          Color.Black,
          0,
          origin,
          Constants.FontScale,
          SpriteEffects.None,
          0);
      this.spriteBatch.DrawString(font, text, position, color, 0, origin, Constants.FontScale, SpriteEffects.None, 0);
    }

    /// <summary>
    /// Draws the string.
    /// </summary>
    /// <param name="font">The font value.</param>
    /// <param name="text">The text value.</param>
    /// <param name="position">The position.</param>
    /// <param name="color">The color value.</param>
    /// <param name="fontScale">The font scale.</param>
    private void DrawString(SpriteFont font, string text, Vector2 position, Color color, float fontScale)
    {
      this.spriteBatch.DrawString(
          font,
          text,
          new Vector2(position.X + 1, position.Y + 1),
          Color.Black,
          0,
          new Vector2(0, font.LineSpacing / 2),
          fontScale,
          SpriteEffects.None,
          0);

      this.spriteBatch.DrawString(
          font,
          text,
          position,
          color,
          0,
          new Vector2(0, font.LineSpacing / 2),
          fontScale,
          SpriteEffects.None,
          0);
    }

    #endregion
  }
}
