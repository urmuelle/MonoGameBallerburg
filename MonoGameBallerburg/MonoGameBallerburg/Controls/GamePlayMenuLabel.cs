// <copyright file="GamePlayMenuLabel.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Controls
{
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  using MonoGameBallerburg.Gameplay;
  using MonoGameBallerburg.Manager;

  public class GamePlayMenuLabel : Control
  {
    #region Fields

    private string labelText;

    private IBallerburgGraphicsManager graphicsDevice;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="GamePlayMenuLabel"/> class.
    /// </summary>
    /// <param name="screen">The screen to be drawn on.</param>
    /// <param name="id">The id of this item.</param>
    /// <param name="graphicsDevice">The graphics device.</param>
    public GamePlayMenuLabel(GameScreen screen, int id, IBallerburgGraphicsManager graphicsDevice)
    {
      this.Id = id;
      this.Owner = screen;
      this.State = States.Visible;
      this.graphicsDevice = graphicsDevice;
    }

    /// <summary>
    /// Sets a value indicating whether this instance is active.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
    /// </value>
    public bool IsActive
    {
      set
      {
        if (value == false)
        {
          this.State = States.Hidden;
        }
        else
        {
          this.State = States.Visible;
        }
      }
    }

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>
    /// The text value.
    /// </value>
    public string Text
    {
      get { return this.labelText; }
      set { this.labelText = value; }
    }

    /// <summary>
    /// Determine current state of the button.
    /// </summary>
    /// <param name="gameTime">Time elapsed since the last call to Update.</param>
    public override void Update(GameTime gameTime)
    {
    }

    /// <summary>
    /// Draws the menu entry. This can be overridden to customize the appearance.
    /// </summary>
    /// <param name="gameTime">Time passed since the last call to Draw.</param>
    public override void Draw(GameTime gameTime)
    {
      if (!(this.State == States.Hidden))
      {
        SpriteBatch spriteBatch = this.Owner.ScreenManager.SpriteBatch;
        Viewport viewport = this.graphicsDevice.GraphicsDevice.Viewport;
        Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

        Color color = Color.Yellow;
        SpriteFont font = this.Owner.ScreenManager.MenuFont;
        Vector2 origin = new Vector2(0, 0);

        spriteBatch.Begin();
        spriteBatch.DrawString(font, this.Text, this.Position, color, 0, origin, Constants.FontScale, SpriteEffects.None, 0);
        spriteBatch.End();
      }
    }
  }
}
