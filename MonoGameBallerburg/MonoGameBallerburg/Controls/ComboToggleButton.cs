// <copyright file="ComboToggleButton.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Controls
{
  using System;
  using System.Collections.Generic;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Input;

  using MonoGameBallerburg.Audio;
  using MonoGameBallerburg.Gameplay;
  using MonoGameBallerburg.Screens;

  /// <summary>
  /// Button, where options are changed in a combobox like manner.
  /// </summary>
  public class ComboToggleButton : Control
  {
    private static object thisLock;

    /// <summary>
    /// The text rendered for this entry.
    /// </summary>
    private readonly List<string> entries;
    private readonly int numEntries;
    private string text;
    private int selectedIndex;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComboToggleButton"/> class.
    /// </summary>
    /// <param name="screen">The screen.</param>
    /// <param name="text">The text to display.</param>
    /// <param name="displayEntries">The display entries.</param>
    /// <param name="selectedIndex">Index of the selected.</param>
    /// <param name="id">The id of this element.</param>
    public ComboToggleButton(GameScreen screen, string text, IEnumerable<string> displayEntries, int selectedIndex, int id)
    {
      this.text = text;
      Id = id;
      Owner = screen;
      this.selectedIndex = selectedIndex;
      entries = new List<string>(displayEntries);
      numEntries = this.entries.Count;
      thisLock = new object();
    }

    /// <summary>
    /// Event raised when the menu entry is selected.
    /// </summary>
    public event EventHandler<ActionToggleButtonEventArgs> Selected;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
    /// </value>
    public bool IsActive
    {
      get { return State == States.Visible; }
      set { State = States.Hidden; }
    }

    /// <summary>
    /// Gets or sets the text of this menu entry.
    /// </summary>
    public string Text
    {
      get { return text; }
      set { text = value; }
    }

    /// <summary>
    /// Gets the selected index of the combo toggle button.
    /// </summary>
    public int SelectedIndex
    {
      get { return selectedIndex; }
    }

    /// <summary>
    /// Den Button als nicht aktiv setzen.
    /// </summary>
    public void SetInactive()
    {
      State = States.Inactive;
    }

    /// <summary>
    /// Den Button aktiv setzen.
    /// </summary>
    public void SetActive()
    {
      State = States.Visible;
    }

    /// <summary>
    /// Gets the rectangle occupied by this string.
    /// </summary>
    /// <returns>Rectangle the size of the text.</returns>
    public Rectangle GetTextRect()
    {
      var rect = default(Rectangle);
      var screenManager = this.Owner.ScreenManager;
      var font = screenManager.Font;
      rect.X = (int)this.Position.X;
      rect.Y = (int)this.Position.Y; // -(font.LineSpacing / 2);
      rect.Width = (int)font.MeasureString(this.text).X;
      rect.Height = (int)font.MeasureString(this.text).Y;
      return rect;
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
      return GetTextRect().Contains((int)mousePos.X, (int)mousePos.Y);
    }

    /// <summary>
    /// Determine current state of the button.
    /// </summary>
    /// <param name="gameTime">Time elapsed since the last call to Update.</param>
    public override void Update(GameTime gameTime)
    {
      if ((State != States.Hidden) && (State != States.None) && (State != States.Inactive))
      {
        MouseState curState = Mouse.GetState();

        // Determine wheter entry is focused
        bool isSelected = IsSelected(new Vector2(curState.X, curState.Y));

        if ((State == States.Visible) && isSelected)
        {
          State = States.MouseOver;
        }
        else if ((State != States.Pressed) && !isSelected)
        {
          State = States.Visible;
        }

        if (State == States.MouseOver)
        {
          if (curState.LeftButton == ButtonState.Pressed)
          {
            if (State == States.MouseOver)
            {
              State = States.Pressed;
            }
          }
        }

        if (State == States.Pressed)
        {
          if (curState.LeftButton == ButtonState.Released)
          {
            State = States.Visible;

            if (isSelected)
            {
              OnSelectEntry();
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
      var color = State == States.MouseOver ? Color.Yellow : Color.White;

      if (State == States.Inactive)
      {
        color = Color.Gray;
      }

      // Draw text, centered on the middle of each line.
      var spriteBatch = Owner.ScreenManager.SpriteBatch;
      var font = Owner.ScreenManager.Font;

      ////Vector2 origin = new Vector2(0, font.LineSpacing / 2);
      var origin = new Vector2(0, 0);
      spriteBatch.Begin();
      spriteBatch.DrawString(font, text, Position, color, 0, origin, Constants.FontScale, SpriteEffects.None, 0);
      spriteBatch.DrawString(
          font,
          this.entries[this.selectedIndex],
          new Vector2(this.Position.X + 50, this.Position.Y + 30),
          color,
          0,
          origin,
          0.5f,
          SpriteEffects.None,
          0);

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
      lock (thisLock)
      {
        selectedIndex += 1;

        if (selectedIndex == numEntries)
        {
          selectedIndex = 0;
        }

        // return the selected index to the caller - if he is interested in it.
        if (Selected != null)
        {
          var args = new ActionToggleButtonEventArgs(selectedIndex);
          Selected(this, args);
        }

        State = States.Visible;
      }
    }
  }
}
