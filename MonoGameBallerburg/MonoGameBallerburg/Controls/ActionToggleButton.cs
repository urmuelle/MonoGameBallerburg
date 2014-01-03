// <copyright file="ActionToggleButton.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Controls
{
  using System;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Input;
  
  using MonoGameBallerburg.Audio;
  using MonoGameBallerburg.Gameplay;
  using MonoGameBallerburg.Screens;

    /// <summary>
  /// Toggling Button
  /// </summary>
  public class ActionToggleButton : Control
  {
    /// <summary>
    /// The text rendered for this entry.
    /// </summary>
    private string text;
    private string stateText;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionToggleButton"/> class.
    /// </summary>
    /// <param name="screen">The screen.</param>
    /// <param name="text">The text to display.</param>
    /// <param name="stateText">The state text.</param>
    /// <param name="id">The id of this element.</param>
    public ActionToggleButton(GameScreen screen, string text, string stateText, int id)
    {
      this.text = text;
      this.Id = id;
      this.Owner = screen;
      this.stateText = stateText;
    }

    /// <summary>
    /// Event raised when the menu entry is selected.
    /// </summary>
    public event EventHandler<EventArgs> Selected;

    /// <summary>
    /// Gets a value indicating whether this instance is active.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
    /// </value>
    public bool IsInactive
    {
      get { return this.State == States.Inactive; }
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
    /// Gets or sets the text of this menu entry.
    /// </summary>
    public string StateText
    {
      get { return this.stateText; }
      set { this.stateText = value; }
    }

    /// <summary>
    /// Gets the rectangle occupied by this string
    /// </summary>
    /// <returns>Rectangle the size of the text</returns>
    public Rectangle TextRect
    {
      get
      {
        var rect = new Rectangle();
        var screenManager = this.Owner.ScreenManager;
        var font = screenManager.Font;
        rect.X = (int)this.Position.X;
        rect.Y = (int)this.Position.Y; // -(font.LineSpacing / 2);
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
    /// Den Button aktiv setzen
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
    /// Determine current state of the button
    /// </summary>
    /// <param name="gameTime">Time elapsed since the last call to Update</param>
    public override void Update(GameTime gameTime)
    {
      if ((this.State != States.Hidden) && (this.State != States.None) && (this.State != States.Inactive))
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
    /// <param name="gameTime">Time passed since the last call to Draw.</param>
    public override void Draw(GameTime gameTime)
    {
      // Draw the selected entry in yellow, otherwise white.
      Color color = this.State == States.MouseOver ? Color.Yellow : Color.White;

      if (this.State == States.Inactive)
      {
        color = Color.Gray;
      }

      // Draw text, centered on the middle of each line.            
      SpriteBatch spriteBatch = this.Owner.ScreenManager.SpriteBatch;
      SpriteFont font = this.Owner.ScreenManager.Font;

      Vector2 origin = new Vector2(0, 0);
      spriteBatch.Begin();
      spriteBatch.DrawString(font, this.text, this.Position, color, 0, origin, Constants.FontScale, SpriteEffects.None, 0);
      spriteBatch.DrawString(font, this.stateText, new Vector2(this.Position.X + 50, this.Position.Y + 30), color, 0, origin, 0.5f, SpriteEffects.None, 0);
      spriteBatch.End();
    }

    /// <summary>
    /// Queries how much space this menu entry requires.
    /// </summary>
    /// <param name="screen">The screen.</param>
    /// <returns>The screens height</returns>
    public virtual int GetHeight(MenuScreen screen)
    {
      return screen.ScreenManager.Font.LineSpacing;
    }

    /// <summary>
    /// Method for raising the Selected event.
    /// </summary>
    protected internal virtual void OnSelectEntry()
    {
      if (this.Selected != null)
      {
        this.Selected(this, EventArgs.Empty);
      }

      this.State = States.Visible;
    }
  }
}
