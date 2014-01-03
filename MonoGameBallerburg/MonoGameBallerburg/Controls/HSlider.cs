// <copyright file="HSlider.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Controls
{
  using System;
  using System.Diagnostics;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Input;

  public class HSlider : Control
  {
    #region Fields

    private float scale;
    private float min = 0.0f;
    private float max = 1.0f;
    private float value; // 0.0f to 1.0f        

    private float keyTimer = 0.0f;
    private bool repeat = false;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="HSlider"/> class.
    /// </summary>
    /// <param name="screen">The screen.</param>
    /// <param name="rect">The rectangle this slider spans.</param>
    /// <param name="value">The initial slider value.</param>
    public HSlider(GameScreen screen, Rectangle rect, float value)
    {
      this.Owner = screen;
      this.Scale = 1.0f;
      ////Scale = scale;
      this.Value = value;
      ControlColor = Color.Yellow;
      this.Width = rect.Width;
      this.Rectangle = rect;
      ////CanFocus = canFocus;
      this.Visible = true;
      this.Enabled = true;
      ////Alignment = GUIAlignment.Top | GUIAlignment.Left;

      ////cnt++;
      ////Text = "XGControl-" + cnt.ToString();
      ControlColor = new Color(0.6f, 1.0f, 0.6f, 0.5f);
      ////ForeColor = new Color(0.6f, 1.0f, 0.6f, 0.707f);
      ////BkgColor = new Color(0.0f, 0.3f, 0.0f, 0.707f);
    }

    /// <summary>
    /// Event raised when the menu entry is selected.
    /// </summary>
    public event EventHandler<SliderChangedEventArgs> ValueChanged;

    /// <summary>
    /// Gets or sets the scale.
    /// </summary>
    /// <value>
    /// The scale.
    /// </value>
    public float Scale
    {
      get
      {
        return this.scale;
      }

      set
      {
        this.scale = value;
        if (this.scale < 0.01f)
        {
          this.scale = 0.01f;
        }

        this.max = this.min + this.scale;
      }
    }

    /// <summary>
    /// Gets or sets the min value.
    /// </summary>
    /// <value>
    /// The min value.
    /// </value>
    public float MinValue
    {
      get
      {
        return this.min;
      }

      set
      {
        if (value < this.max)
        {
          this.min = value;
          this.Scale = this.max - this.min;
        }
      }
    }

    /// <summary>
    /// Gets or sets the max value.
    /// </summary>
    /// <value>
    /// The max value.
    /// </value>
    public float MaxValue
    {
      get
      {
        return this.max;
      }

      set
      {
        if (value > this.min)
        {
          this.max = value;
          this.Scale = this.max - this.min;
        }
      }
    }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public float Value
    {
      get
      {
        return (this.value * this.scale) + this.min;
      }

      set
      {
        this.value = (value - this.min) / this.scale;
      }
    }

    /// <summary>
    /// Sets the range.
    /// </summary>
    /// <param name="value">The current value.</param>
    /// <param name="min">The min value.</param>
    /// <param name="max">The max value.</param>
    public void SetRange(float value, float min, float max)
    {
      MinValue = min;
      MaxValue = max;
      Value = value;
    }

    /// <summary>
    /// Implementiert die Logik des Controls
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public override void Update(GameTime gameTime)
    {
      if ((State != States.Hidden) && (State != States.None) && (State != States.Inactive))
      {
        var frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        MouseState curMouseState = Mouse.GetState();
        KeyboardState curKeyState = Keyboard.GetState();

        if (keyTimer > 0.0f)
        {
          keyTimer -= frameTime;
        }

        var oldValue = 0.0f;
        var downPos = Vector2.Zero;
        var mouseDown = false;
        var mousePos = new Vector2(curMouseState.X, curMouseState.Y);
        var mousePoint = new Point((int)mousePos.X, (int)mousePos.Y);

        ////if (XnaGUIManager.GetFocusControl() == this)
        ////{
        if (curKeyState.IsKeyDown(Keys.Right) || curKeyState.IsKeyDown(Keys.Left))
        {
          if (keyTimer <= 0.0f)
          {
            if (curKeyState.IsKeyDown(Keys.Right))
            {
              value += 0.01f;
              if (value > 1.0f)
              {
                value = 1.0f;
              }
            }
            else
            {
              value -= 0.01f;
              if (value < 0.0f)
              {
                value = 0.0f;
              }
            }

            keyTimer = 0.5f;
            if (repeat)
            {
              keyTimer = 0.1f;
            }

            repeat = true;
          }
        }
        else
        {
          keyTimer = 0.0f;
          repeat = false;
        }

        if (curMouseState.LeftButton == ButtonState.Pressed && Contains(mousePoint))
        {
          if (!mouseDown)
          {
            Debug.WriteLine("MouseDown");
            downPos = mousePos;
            ////float pos = downPos.X - (ToScreen(Rectangle).X + 2);
            var pos = downPos.X - (Rectangle.X + 2);
            if (pos < 0)
            {
              pos = 0;
            }

            if (pos > (Rectangle.Width - 4))
            {
              pos = Rectangle.Width - 4;
            }

            var v = pos / (Rectangle.Width - 4);
            value = v;
            oldValue = value;
          }

          mouseDown = true;
        }
        else
        {
          mouseDown = false;
        }

        if (mouseDown)
        {
          var diff = downPos.X - mousePos.X;
          value = value + (diff / (Rectangle.Width - 4));
        }
        ////}

        if (ValueChanged != null)
        {
          ValueChanged(this, new SliderChangedEventArgs(Value));
        }
      }

      base.Update(gameTime);
    }

    /// <summary>
    /// Implementiert den Render-Prozess des Controls
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public override void Draw(GameTime gameTime)
    {
      // Draw the selected entry in yellow, otherwise white.
      var color = State == States.Inactive ? Color.White : ThisControlColor;

      var rect = Rectangle; // new Rectangle(); // = ToScreen(Rectangle);

      var v = rect.Top + (((rect.Bottom - rect.Top) / 2) - 1);
      rect = new Rectangle(rect.Left, v, rect.Width, 4);

      // Draw the sliding - bar
      Owner.ScreenManager.SpriteBatch.Begin();
      Owner.ScreenManager.SpriteBatch.Draw(Owner.ScreenManager.WhiteTex, rect, color);

      var pos = (int)(value * (Width - 4)) + 2;
      rect = Rectangle;

      // Draw the slider
      rect.X += pos - 2;
      rect.Y += 2; // (int)Padding.Y;
      rect.Height -= 4; // (int)Padding.Y * 2; ;
      rect.Width = 10;
      Owner.ScreenManager.SpriteBatch.Draw(Owner.ScreenManager.WhiteTex, rect, color);
      Owner.ScreenManager.SpriteBatch.End();
    }
  }
}
