// <copyright file="MessageBoxScreen.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Screens
{
  using System;
  using Manager;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  /// <summary>
  /// A popup message box screen, used to display "are you sure?"
  /// confirmation messages.
  /// </summary>
  public class MessageBoxScreen : GameScreen
  {
    #region Fields

    private readonly string message;
    private Texture2D gradientTexture;

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageBoxScreen"/> class.
    /// </summary>
    /// <param name="graphicsManager">The graphics manager.</param>
    /// <param name="message">The message.</param>
    public MessageBoxScreen(IBallerburgGraphicsManager graphicsManager, string message)
      : this(graphicsManager, message, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageBoxScreen"/> class.
    /// </summary>
    /// <param name="graphicsManager">The graphics manager.</param>
    /// <param name="message">The message.</param>
    /// <param name="includeUsageText">if set to <c>true</c> [include usage text].</param>
    public MessageBoxScreen(IBallerburgGraphicsManager graphicsManager, string message, bool includeUsageText)
      : base(graphicsManager)
    {
      const string UsageText = "\nA button, Space, Enter = ok" +
                               "\nB button, Esc = cancel";

      if (includeUsageText)
      {
        this.message = message + UsageText;
      }
      else
      {
        this.message = message;
      }

      IsPopup = true;

      TransitionOffTime = TimeSpan.FromSeconds(0.2);
    }

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

    #endregion

    #region Handle Input

    /// <summary>
    /// Responds to user input, accepting or cancelling the message box.
    /// </summary>
    /// <param name="input">The input.</param>
    public override void HandleInput(InputState input)
    {
      if (input.MenuSelect)
      {
        // Raise the accepted event, then exit the message box.
        if (Accepted != null)
        {
          Accepted(this, EventArgs.Empty);
        }

        ExitScreen();
      }
      else if (input.MenuCancel)
      {
        // Raise the cancelled event, then exit the message box.
        if (Cancelled != null)
        {
          Cancelled(this, EventArgs.Empty);
        }

        ExitScreen();
      }
    }

    #endregion

    #region Draw

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
      var color = new Color(255, 255, 255, TransitionAlpha);

      spriteBatch.Begin();

      // Draw the background rectangle.
      spriteBatch.Draw(gradientTexture, backgroundRectangle, color);

      // Draw the message box text.
      spriteBatch.DrawString(font, message, textPosition, color);

      spriteBatch.End();
    }

    #endregion
  }
}
