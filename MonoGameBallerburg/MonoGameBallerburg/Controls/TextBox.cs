// <copyright file="TextBox.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
namespace MonoGameBallerburg.Controls
{
  using System;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Input;

  using MonoGameBallerburg.Manager;

  public class TextBox : Control
  {
    private string textString;

    private Rectangle textBox;
    private Texture2D cursor;
    private Vector2 cursorPosition;
    private Vector2 textPosition;
    private TimeSpan blinkTime;
    private bool blink;
    private Vector2 position;
    private bool focused;

    /// <summary>
    /// Keyboard state
    /// </summary>
    private KeyboardState oldKeyboardState, currentKeyboardState;
    private bool inEntryMode;
    private SpriteFont font;
    private bool isEditable;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextBox"/> class.
    /// </summary>
    /// <param name="screen">The screen.</param>
    /// <param name="isEditable">if set to <c>true</c> [is editable].</param>
    public TextBox(GameScreen screen, bool isEditable)
    {
      Owner = screen;
      inEntryMode = false;
      this.isEditable = isEditable;

      textBox = new Rectangle(10, 10, 220, 50);
      Position = new Vector2(10, 10);
      textString = string.Empty;
      cursor = BallerburgGame.Instance.Content.Load<Texture2D>(@"GUI\cursor");
      Position = new Vector2();
      cursorPosition = new Vector2(
          Position.X + 5,
          Position.Y + 5);
      textPosition = new Vector2(
          Position.X + 5,
          Position.Y + 5);
      blink = false;
    }

    /// <summary>
    /// Gets or sets the text of this menu entry.
    /// </summary>
    /// <value>
    /// The position.
    /// </value>
    public new Vector2 Position
    {
      get
      {
        return position;
      }

      set
      {
        position = value;
        textBox.X = (int)value.X;
        textBox.Y = (int)value.Y;
        SetTextPosition();
      }
    }

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>
    /// The text to be displayed.
    /// </value>
    public string Text
    {
      get { return textString; }
      set { textString = value; }
    }

    public bool ShowCursor
    {
      get;
      set;
    }

    /// <summary>
    /// Gets the rectangle occupied by this string
    /// </summary>
    /// <returns>The rect this textbox spans</returns>
    public Rectangle GetTextRect()
    {
      Rectangle rect = new Rectangle();
      ScreenManager screenManager = Owner.ScreenManager;
      SpriteFont font = screenManager.Font;
      rect.X = (int)position.X;
      rect.Y = (int)position.Y; ////-font.LineSpacing / 2;
      rect.Width = textBox.Width; ////(int)font.MeasureString(textString).X;
      rect.Height = textBox.Height; ////(int)font.MeasureString(textString).Y;
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
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public override void Update(GameTime gameTime)
    {
      UpdateInput();
      blinkTime += gameTime.ElapsedGameTime;

      if (blinkTime > TimeSpan.FromMilliseconds(500))
      {
        blink = !blink;
        blinkTime -= TimeSpan.FromMilliseconds(500);
      }

      Vector2 textSize = Owner.ScreenManager.MenuFont.MeasureString(Text);
      cursorPosition.X = textPosition.X + textSize.X;
      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public override void Draw(GameTime gameTime)
    {
      // Draw the selected entry in yellow, otherwise white.
      var color = State == States.MouseOver ? Color.Yellow : Color.White;

      if (State == States.Inactive)
      {
        color = Color.Gray;
      }

      // Draw text, centered on the middle of each line.            
      font = Owner.ScreenManager.MenuFont;
      var spriteBatch = Owner.ScreenManager.SpriteBatch;

      var origin = new Vector2(0, font.LineSpacing / 2);

      /*
      var texture = new Texture2D(BallerburgGame.Instance.GraphicsDevice, 1, 1);
      texture.SetData(new[] { Color.White });
      */

      spriteBatch.Begin();

      ////int bw = 2; // Border width

      if (ShowCursor)
      {
        if (!blink)
        {
          spriteBatch.Draw(cursor, cursorPosition, Color.White);
        }
      }

      /*
      spriteBatch.Draw(t, new Rectangle(this.textBox.Left, this.textBox.Top, bw, this.textBox.Height), Color.Yellow); // Left
      spriteBatch.Draw(t, new Rectangle(this.textBox.Right, this.textBox.Top, bw, this.textBox.Height), Color.Yellow); // Right
      spriteBatch.Draw(t, new Rectangle(this.textBox.Left, this.textBox.Top, this.textBox.Width, bw), Color.Yellow); // Top
      spriteBatch.Draw(t, new Rectangle(this.textBox.Left, this.textBox.Bottom, this.textBox.Width, bw), Color.Yellow); // Bottom
      */

      spriteBatch.DrawString(font, textString, new Vector2(position.X + 10, Position.Y + (font.LineSpacing / 2)), color, 0, origin, 1, SpriteEffects.None, 0);
      spriteBatch.End();

      ////base.Draw(gameTime);
    }

    /// <summary>
    /// Sets the focus.
    /// </summary>
    public void SetFocus()
    {
      focused = true;
      inEntryMode = true;
    }

    /// <summary>
    /// Sets the text position.
    /// </summary>
    private void SetTextPosition()
    {
      cursorPosition = new Vector2(
      position.X + 5,
      position.Y + 5);
      textPosition = new Vector2(
      position.X + 5,
      position.Y + 5);
    }

    /*
    /// <summary>
    /// Called when graphics resources need to be loaded. Override this method to load any component-specific graphics resources.
    /// </summary>
    protected override void LoadContent()
    {            
        //this.font = this.parentScreen.ScreenManager.MenuFont;
        base.LoadContent();
    }
    */

    /// <summary>
    /// Updates the input.
    /// </summary>
    private void UpdateInput()
    {
      if ((State != States.Hidden) && (State != States.None) && (State != States.Inactive))
      {
        MouseState curState = Mouse.GetState();

        // Determine wheter entry is focused
        bool isSelected = IsSelected(new Vector2(curState.X, curState.Y)) || focused;

        if ((State == States.Visible) && isSelected)
        {
          State = States.MouseOver;
        }
        else if ((State != States.Pressed) && !isSelected)
        {
          State = States.Visible;
        }

        if (State == States.Visible)
        {
          if (curState.LeftButton == ButtonState.Pressed)
          {
            if (!isSelected)
            {
              inEntryMode = false;
            }
          }
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
            inEntryMode = true;
          }
        }

        if (inEntryMode && isEditable)
        {
          oldKeyboardState = currentKeyboardState;
          currentKeyboardState = Keyboard.GetState();
          Keys[] pressedKeys;

          pressedKeys = currentKeyboardState.GetPressedKeys();
          foreach (Keys key in pressedKeys)
          {
            if (oldKeyboardState.IsKeyUp(key))
            {
              // overflows
              if (key == Keys.Back)
              {
                if (textString.Length > 0)
                {
                  textString = textString.Remove(textString.Length - 1, 1);
                }
              }
              else
              {
                if (key == Keys.Space)
                {
                  textString = textString.Insert(textString.Length, " ");
                }
                else
                {
                  if (key == Keys.Enter)
                  {
                    ////posFont = new Vector2(20, 20);                                     
                  }
                  else
                  {
                    if ((key != Keys.Right) &&
                        (key != Keys.RightAlt) &&
                        (key != Keys.RightControl) &&
                        (key != Keys.RightShift) &&
                        (key != Keys.RightWindows) &&
                        (key != Keys.Left) &&
                        (key != Keys.LeftAlt) &&
                        (key != Keys.LeftControl) &&
                        (key != Keys.LeftShift) &&
                        (key != Keys.LeftWindows))
                    {
                      if (font.MeasureString(textString).X < textBox.Width - 40)
                      {
                        textString += key.ToString();
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}