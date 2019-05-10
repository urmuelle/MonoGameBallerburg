// <copyright file="GameplayMenuItem.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Controls
{
  using System;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Input;

  /// <summary>
  ///   Menu item used in game's menu
  /// </summary>
  public class GameplayMenuItem : Control // : DrawableGameComponent
  {
    #region Fields

    private bool isMouseOver;

    private string mouseOverText;

    /// <summary>
    ///   The source bitmap for this entry.
    /// </summary>
    private Texture2D menuTexture;

    private Rectangle sourceRect;
    private Rectangle targetRect;

    #endregion

    /*
        protected internal virtual void OnSelectEntry()
        {
            if (MouseDown != null)
                MouseDown(this, EventArgs.Empty);
            //state = States.Visible;
        }
         * */

    /// <summary>
    /// Initializes a new instance of the <see cref="GameplayMenuItem"/> class.
    /// </summary>
    /// <param name="screen">The screen.</param>
    /// <param name="sourceRect">The source rect.</param>
    /// <param name="targetRect">The target rect.</param>
    /// <param name="id">The id value.</param>
    public GameplayMenuItem(GameScreen screen, Rectangle sourceRect, Rectangle targetRect, int id)
    {
      Id = id;
      Owner = screen;
      this.sourceRect = sourceRect;
      this.targetRect = targetRect;
      isMouseOver = false;
      State = States.Visible;
    }

    /// <summary>
    ///   Event raised when the menu entry is selected.
    /// </summary>
    public event EventHandler<EventArgs> MouseDown;

    /// <summary>
    ///   Event raised when the menu entry is selected.
    /// </summary>
    public event EventHandler<EventArgs> MouseUp;

    /// <summary>
    ///   Event raised, when the mouse enters the visible part of the button
    /// </summary>
    public event EventHandler<ButtonEventArgs> MouseEnter;

    /// <summary>
    ///   Event raised, when the mouse enters the visible part of the button
    /// </summary>
    public event EventHandler<EventArgs> MouseLeave;

    /// <summary>
    ///   Gets or sets the mouse over text.
    /// </summary>
    /// <value> The mouse over text. </value>
    public string MouseOverText
    {
      get { return mouseOverText; }
      set { mouseOverText = value; }
    }

    /*
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
     * */

    /// <summary>
    ///   Gets the rectangle occupied by this Buttonitem
    /// </summary>
    /// <returns> The rectangle this menuitem spans </returns>
    public Rectangle GetButtonRect()
    {
      var rect = default(Rectangle);
      var screenManager = Owner.ScreenManager;
      rect.X = (int)Position.X;
      ////rect.Y = (int)position.Y - font.LineSpacing / 2;
      ////rect.Width = (int)font.MeasureString(text).X;
      ////rect.Height = (int)font.MeasureString(text).Y;
      return rect;
    }

    /// <summary>
    ///   Determines whether the specified mouse pos is selected.
    /// </summary>
    /// <param name="mousePos"> The mouse pos. </param>
    /// <returns> <c>true</c> if the specified mouse pos is selected; otherwise, <c>false</c> . </returns>
    public bool IsSelected(Vector2 mousePos)
    {
      return targetRect.Contains((int)mousePos.X, (int)mousePos.Y);
    }

    /// <summary>
    ///   Determine current state of the button
    /// </summary>
    /// <param name="gameTime"> Time elapsed since the last call to Update </param>
    public override void Update(GameTime gameTime)
    {
      ////if ((this.State != States.Hidden) && (this.State != States.None))
      if (Visible && Enabled)
      {
        var curState = Mouse.GetState();

        // Determine wheter entry is focused
        var isSelected = IsSelected(new Vector2(curState.X, curState.Y));

        // Detect, whether the mouse is over the button
        if (isSelected)
        {
          // If the mouse Enters the button area, fire event
          if (isMouseOver == false)
          {
            var be = new ButtonEventArgs((int)Position.X, (int)Position.Y, mouseOverText);

            isMouseOver = true;

            if (!(MouseEnter == null))
            {
              MouseEnter(this, be);
            }
          }
        }
        else
        {
          // The button leaves the visible territory
          if (isMouseOver == true)
          {
            isMouseOver = false;
            if (!(MouseLeave == null))
            {
              MouseLeave(this, EventArgs.Empty);
            }
          }
        }

        if ((State == States.Visible) && isSelected)
        {
          State = States.MouseOver;
        }
        else if ((State != States.Pressed) && !isSelected)
        {
          State = States.Visible;
        }
        else if ((State == States.Pressed) && !isSelected)
        {
          State = States.Pressed;
        }

        if (State == States.MouseOver)
        {
          if (curState.LeftButton == ButtonState.Pressed)
          {
            State = States.Pressed;
            if (MouseDown != null)
            {
              MouseDown(this, EventArgs.Empty);
              ////break;
            }
          }
        }

        if (State == States.Pressed)
        {
          if (curState.LeftButton == ButtonState.Released)
          {
            State = States.Visible;
            ////if (isSelected)
            ////{
            if (MouseUp != null)
            {
              MouseUp(this, EventArgs.Empty);
              ////break;
            }
            ////}
          }
          else
          {
            ////OnSelectEntry();
          }
        }
      }
    }

    /// <summary>
    ///   Draws the menu entry. This can be overridden to customize the appearance.
    /// </summary>
    /// <param name="gameTime"> Time passed since the last call to Draw. </param>
    public override void Draw(GameTime gameTime)
    {
      menuTexture = Owner.ScreenManager.ContentManager.MenuTexture;

      if (Visible)
      {
        // Draw text, centered on the middle of each line.
        ScreenManager screenManager = Owner.ScreenManager;
        SpriteBatch spriteBatch = screenManager.SpriteBatch;
        byte fade = 255;

        ////spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
        spriteBatch.Begin();
        if (!(State == States.Pressed))
        {
          spriteBatch.Draw(menuTexture, targetRect, sourceRect, new Color(fade, fade, fade));
        }
        else
        {
          Rectangle movedRect = targetRect;
          movedRect.X += 5;
          movedRect.Y += 5;
          spriteBatch.Draw(menuTexture, movedRect, sourceRect, new Color(fade, fade, fade));
        }

        spriteBatch.End();
      }
    }
  }
}