// <copyright file="BackgroundScreen.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Screens
{
  using System;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;
  using MonoGameBallerburg.Manager;

  /// <summary>
  /// The background screen sits behind all the other menu screens.
  /// It draws a background image that remains fixed in place regardless
  /// of whatever transitions the screens on top of it may be doing.
  /// </summary>
  public class BackgroundScreen : GameScreen
  {
    #region Fields

    private Texture2D backgroundTexture;

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundScreen"/> class.
    /// </summary>
    /// <param name="graphicsManager">The graphics manager.</param>
    public BackgroundScreen(IBallerburgGraphicsManager graphicsManager)
        : base(graphicsManager)
    {
      TransitionOnTime = TimeSpan.FromSeconds(0);
      TransitionOffTime = TimeSpan.FromSeconds(0);
    }

    /// <summary>
    /// Loads graphics content for this screen. The background texture is quite
    /// big, so we use our own local ContentManager to load it. This allows us
    /// to unload before going from the menus into the game itself, wheras if we
    /// used the shared ContentManager provided by the Game class, the content
    /// would remain loaded forever.
    /// </summary>
    public override void LoadContent()
    {
      backgroundTexture = ScreenManager.ContentManager.BackgroundTexture;
    }

    #endregion

    #region Update and Draw

    /// <summary>
    /// Updates the background screen. Unlike most screens, this should not
    /// transition off even if it has been covered by another screen: it is
    /// supposed to be covered, after all! This overload forces the
    /// coveredByOtherScreen parameter to false in order to stop the base
    /// Update method wanting to transition off.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    /// <param name="otherScreenHasFocus">if set to <c>true</c> [other screen has focus].</param>
    /// <param name="coveredByOtherScreen">if set to <c>true</c> [covered by other screen].</param>
    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, false);
    }

    /// <summary>
    /// Draws the background screen.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public override void Draw(GameTime gameTime)
    {
      var spriteBatch = new SpriteBatch(GraphicsManager.GraphicsDevice);

      var viewport = GraphicsManager.GraphicsDevice.Viewport;
      var fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
      byte fade = TransitionAlpha;

      spriteBatch.Begin();

      spriteBatch.Draw(backgroundTexture, fullscreen, new Color(fade, fade, fade));

      spriteBatch.End();
    }

    #endregion
  }
}
