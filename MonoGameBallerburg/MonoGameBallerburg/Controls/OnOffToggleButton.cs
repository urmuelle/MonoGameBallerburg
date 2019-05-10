// <copyright file="OnOffToggleButton.cs" company="Urs Müller">
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
  /// Class representing a simple "on off" toggle control element.
  /// </summary>
  public class OnOffToggleButton : Control
  {
    /// <summary>
    /// The text rendered for this entry.
    /// </summary>
    private string text;
    private bool value;

    /// <summary>
    /// Initializes a new instance of the <see cref="OnOffToggleButton"/> class.
    /// </summary>
    /// <param name="screen">The screen.</param>
    /// <param name="text">The text to display.</param>
    /// <param name="on">if set to <c>true</c> [on].</param>
    /// <param name="id">The id of this element.</param>
    public OnOffToggleButton(GameScreen screen, string text, bool on, int id)
    {
      this.text = text;
      this.Id = id;
      this.Owner = screen;
      this.value = on;
    }

    /// <summary>
    /// Event raised when the menu entry is selected.
    /// </summary>
    public event EventHandler<EventArgs> Selected;

    /// <summary>
    /// Sets a value indicating whether this instance is active.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
    /// </value>
    public bool IsActive
    {
      set { this.State = States.Hidden; }
    }

    /// <summary>
    /// Gets or sets the text of this menu entry.
    /// </summary>
    public string Text
    {
      get { return this.text; }
      set { this.text = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="OnOffToggleButton"/> is value.
    /// </summary>
    /// <value>
    ///   <c>true</c> if value; otherwise, <c>false</c>.
    /// </value>
    public bool Value
    {
      get { return this.value; }
      set { this.value = value; }
    }

    /// <summary>
    /// Gets the TextRect occupied by this value.
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
    /// Den Button als nicht aktiv setzen.
    /// </summary>
    public void SetInactive()
    {
      this.State = States.Inactive;
    }

    /// <summary>
    /// Den Button aktiv setzen.
    /// </summary>
    public void SetActive()
    {
      this.State = States.Visible;
    }

    /// <summary>
    /// Determines whether the specified mouse pos is selected.
    /// </summary>
    /// <param name="mousePos">The mouse pos.</param>
    /// <returns>
    ///   <c>true</c> if the specified mouse pos is selected; otherwise, <c>false</c>.
    /// </returns>
    public bool IsSelected(Vector2 mousePos)
    {
      return this.TextRect.Contains((int)mousePos.X, (int)mousePos.Y);
    }

    /// <summary>
    /// Determine current state of the button.
    /// </summary>
    /// <param name="gameTime">Time elapsed since the last call to Update.</param>
    public override void Update(GameTime gameTime)
    {
      if ((this.State != States.Hidden) && (this.State != States.None) && (this.State != States.Inactive))
      {
        var curState = Mouse.GetState();

        // Determine wheter entry is focused
        var isSelected = this.IsSelected(new Vector2(curState.X, curState.Y));

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
    /// <param name="gameTime">Time passed since the last call to Draw.</param>
    public override void Draw(GameTime gameTime)
    {
      // Draw the selected entry in yellow, otherwise white.
      var color = this.State == States.MouseOver ? Color.Yellow : Color.White;

      if (this.State == States.Inactive)
      {
        color = Color.Gray;
      }

      // Draw text, centered on the middle of each line.
      var spriteBatch = this.Owner.ScreenManager.SpriteBatch;
      var font = this.Owner.ScreenManager.Font;

      var origin = new Vector2(0, font.LineSpacing / 2);
      spriteBatch.Begin();
      spriteBatch.DrawString(font, this.text, this.Position, color, 0, origin, Constants.FontScale, SpriteEffects.None, 0);

      if (this.value)
      {
        spriteBatch.DrawString(font, "Ein", new Vector2(this.Position.X + 40, this.Position.Y + 20), color, 0, origin, 0.5f, SpriteEffects.None, 0);
      }
      else
      {
        spriteBatch.DrawString(font, "Aus", new Vector2(this.Position.X + 40, this.Position.Y + 20), color, 0, origin, 0.5f, SpriteEffects.None, 0);
      }

      spriteBatch.End();
    }

    /// <summary>
    /// Queries how much space this menu entry requires.
    /// </summary>
    /// <param name="screen">The screen.</param>
    /// <returns>The screens height.</returns>
    public virtual int GetHeight(MenuScreen screen)
    {
      return screen.ScreenManager.Font.LineSpacing;
    }

    /// <summary>
    /// Method for raising the Selected event.
    /// </summary>
    protected internal virtual void OnSelectEntry()
    {
      this.value = !this.value;

      if (this.Selected != null)
      {
        this.Selected(this, EventArgs.Empty);
      }

      this.State = States.Visible;
    }
  }
}
