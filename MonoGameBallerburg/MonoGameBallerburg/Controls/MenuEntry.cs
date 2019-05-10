// <copyright file="MenuEntry.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Controls
{
  using System;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Input;
  using MonoGameBallerburg.Gameplay;
  using MonoGameBallerburg.Screens;

  /// <summary>
  /// Helper class represents a single entry in a MenuScreen. By default this
  /// just draws the entry text string, but it can be customized to display menu
  /// entries in different ways. This also provides an event that will be raised
  /// when the menu entry is selected.
  /// </summary>
  public class MenuEntry : Control
  {
    /// <summary>
    /// The text rendered for this entry.
    /// </summary>
    private string text;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuEntry"/> class.
    /// </summary>
    /// <param name="screen">The screen to be drawn on.</param>
    /// <param name="text">The text to be drawn.</param>
    /// <param name="id">The id of this item.</param>
    public MenuEntry(GameScreen screen, string text, int id)
    {
      this.Id = id;
      this.Owner = screen;
      this.State = States.Visible;
      this.text = text;
      this.Activate();
    }

    /// <summary>
    /// Event raised when the menu entry is selected.
    /// </summary>
    public event EventHandler<EventArgs> Selected;

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
                this.state = States.Hidden;
            }
            else
            {
                this.state = States.Visible;
            }
        }
    }
    */

    /// <summary>
    /// Gets or sets the text of this menu entry.
    /// </summary>
    public string Text
    {
      get { return this.text; }
      set { this.text = value; }
    }

    /// <summary>
    /// Gets the rectangle occupied by this string
    /// </summary>
    public Rectangle TextRect
    {
      get
      {
        var rect = default(Rectangle);
        var screenManager = this.Owner.ScreenManager;
        var font = screenManager.Font;
        rect.X = (int)this.Position.X;
        rect.Y = (int)this.Position.Y - (font.LineSpacing / 2);
        rect.Width = (int)font.MeasureString(this.text).X;
        rect.Height = (int)font.MeasureString(this.text).Y;
        return rect;
      }
    }

    /// <summary>
    /// Den Button als nicht aktiv setzen
    /// </summary>
    public void SetInactive()
    {
      this.State = States.Inactive;
    }

    /// <summary>
    /// Den Button Aktiv setzen
    /// </summary>
    public void SetActive()
    {
      this.State = States.Visible;
      this.Activate();
    }

    /// <summary>
    /// Method to check for mouse position over control
    /// </summary>
    /// <param name="mousePos">The current mouse position</param>
    /// <returns>Flag indicating, whether mouse is over the control</returns>
    public bool IsSelected(Vector2 mousePos)
    {
      return this.TextRect.Contains((int)mousePos.X, (int)mousePos.Y);
    }

    /// <summary>
    /// Queries how much space this menu entry requires.
    /// </summary>
    /// <param name="screen">The parent screen</param>
    /// <returns>The entries height</returns>
    public virtual int GetHeight(MenuScreen screen)
    {
      return screen.ScreenManager.MenuFont.LineSpacing;
    }

    /// <summary>
    /// Determine current state of the button
    /// </summary>
    /// <param name="gameTime">The games Time</param>
    public override void Update(GameTime gameTime)
    {
      if ((this.State != States.Hidden) &&
          (this.State != States.None) &&
          (this.State != States.Inactive) &&
          this.Enabled)
      {
        MouseState curState = Mouse.GetState();

        // Determine wheter entry is focused
        bool isSelected = this.IsSelected(new Vector2(curState.X, curState.Y));

        if ((this.State == States.Visible) && isSelected)
        {
          this.State = States.MouseOver;
        }
        else if ((this.State != States.Pressed) && !isSelected)
        {
          this.State = States.Visible;
        }

        if (this.State == States.MouseOver)
        {
          if (curState.LeftButton == ButtonState.Pressed)
          {
            if (this.State == States.MouseOver)
            {
              this.State = States.Pressed;
            }
          }
        }

        if (this.State == States.Pressed)
        {
          if (curState.LeftButton == ButtonState.Released)
          {
            this.State = States.Visible;
            if (isSelected)
            {
              this.OnSelectEntry();
            }
          }
        }
      }
    }

    /// <summary>
    /// Draws the menu entry. This can be overridden to customize the appearance.
    /// </summary>
    /// <param name="gameTime">The games time</param>
    public override void Draw(GameTime gameTime)
    {
      // Draw the selected entry in yellow, otherwise white.
      Color color = this.State == States.MouseOver ? Color.Yellow : Color.White;

      if (this.State == States.Inactive)
      {
        color = Color.Gray;
      }

      // Draw text, centered on the middle of each line.
      ScreenManager screenManager = this.Owner.ScreenManager;
      SpriteBatch spriteBatch = screenManager.SpriteBatch;
      SpriteFont font = screenManager.MenuFont;

      Vector2 origin = new Vector2(0, font.LineSpacing / 2);
      spriteBatch.Begin();
      spriteBatch.DrawString(font, this.text, this.Position, color, 0, origin, Constants.FontScale, SpriteEffects.None, 0);
      spriteBatch.End();
    }

    /// <summary>
    /// Method for raising the Selected event.
    /// </summary>
    protected internal virtual void OnSelectEntry()
    {
      if (this.State != States.Inactive)
      {
        if (this.Selected != null)
        {
          this.Selected(this, EventArgs.Empty);
        }

        this.State = States.Visible;
      }
    }
  }
}
