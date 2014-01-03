// <copyright file="MousePointer.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg
{
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Input;

  /// <summary>
  /// This is a game component that implements IUpdateable.
  /// </summary>
  public class MousePointer : DrawableGameComponent
  {
    public Vector2 MousePosition;

    private SpriteBatch spriteBatch;
    private Vector2 clickBeginPosition;
    private MouseState currentMouseState;
    private MouseState previousMouseState;
    private Color pointerColor = Color.White;
    private Texture2D pointerTexture;
    private Rectangle restrictZone;
    private bool mouseButtonLeftPressed;
    private bool mouseButtonLeftClicked;

    /// <summary>
    /// Initializes a new instance of the <see cref="MousePointer"/> class.
    /// </summary>
    /// <param name="game">The Game that the game component should be attached to.</param>
    public MousePointer(Game game)
      : base(game)
    {
      // TODO: Construct any child components here
    }

    /// <summary>
    /// Gets or sets the pointer texture.
    /// </summary>
    /// <value>
    /// The pointer texture.
    /// </value>
    public Texture2D PointerTexture
    {
      get { return this.pointerTexture; }
      set { this.pointerTexture = value; }
    }

    /// <summary>
    /// Gets or sets the restrict zone.
    /// </summary>
    /// <value>
    /// The restrict zone.
    /// </value>
    public Rectangle RestrictZone
    {
      get { return this.restrictZone; }
      set { this.restrictZone = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether [mouse button left pressed].
    /// </summary>
    /// <value>
    /// <c>true</c> if [mouse button left pressed]; otherwise, <c>false</c>.
    /// </value>
    public bool MouseButtonLeftPressed
    {
      get { return this.mouseButtonLeftPressed; }
      set { this.mouseButtonLeftPressed = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether [mouse button left clicked].
    /// </summary>
    /// <value>
    /// <c>true</c> if [mouse button left clicked]; otherwise, <c>false</c>.
    /// </value>
    public bool MouseButtonLeftClicked
    {
      get { return this.mouseButtonLeftClicked; }
      set { this.mouseButtonLeftClicked = value; }
    }

    /// <summary>
    /// Allows the game component to perform any initialization it needs to before starting
    /// to run.  This is where it can query for any required services and load content.
    /// </summary>
    public override void Initialize()
    {
      // TODO: Add your initialization code here
      base.Initialize();
    }

    /// <summary>
    /// Allows the game component to update itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public override void Update(GameTime gameTime)
    {
      // A click has a duration of one update tick
      if (this.mouseButtonLeftClicked)
      {
        this.mouseButtonLeftClicked = false;
      }

      // TODO: Add your update code here
      this.currentMouseState = Mouse.GetState();
      this.MousePosition.X = this.currentMouseState.X;
      this.MousePosition.Y = this.currentMouseState.Y;

      // Restrict the Mouse so that it stays inside the current display
      if (this.MousePosition.X < 0)
      {
        this.MousePosition.X = 0;
      }

      if (this.MousePosition.X > this.restrictZone.Width)
      {
        this.MousePosition.X = this.restrictZone.Width;
      }

      if (this.MousePosition.Y < 0)
      {
        this.MousePosition.Y = 0;
      }

      if (this.MousePosition.Y > this.restrictZone.Height)
      {
        this.MousePosition.Y = this.restrictZone.Height;
      }

      if ((this.currentMouseState.LeftButton == ButtonState.Pressed) || (this.currentMouseState.RightButton == ButtonState.Pressed))
      {
        this.pointerColor = Color.Red;
      }
      else
      {
        this.pointerColor = Color.White;
      }

      if (this.currentMouseState.LeftButton == ButtonState.Pressed)
      {
        if (this.mouseButtonLeftPressed == false)
        {
          this.mouseButtonLeftPressed = true;
          this.clickBeginPosition = this.MousePosition;
        }
      }

      if (this.currentMouseState.LeftButton == ButtonState.Released && this.mouseButtonLeftPressed)
      {
        if (this.MousePosition == this.clickBeginPosition)
        {
          this.mouseButtonLeftClicked = true;
        }

        this.mouseButtonLeftPressed = false;
      }

      ////this.MouseButtonLeftPressed = false;

      this.previousMouseState = this.currentMouseState;

      base.Update(gameTime);
    }

    /// <summary>
    /// Allows the game commponent to Draw itself
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values</param>
    public override void Draw(GameTime gameTime)
    {
      ////this.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
      this.spriteBatch.Begin();
      this.spriteBatch.Draw(this.pointerTexture, this.MousePosition, this.pointerColor);
      this.spriteBatch.End();
      base.Draw(gameTime);
    }

    /// <summary>
    /// Called when graphics resources need to be loaded. Override this method to load any component-specific graphics resources.
    /// </summary>
    protected override void LoadContent()
    {
      // TODO: Load any ResourceManagementMode.Automatic content
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

      // TODO: Load any ResourceManagementMode.Manual content
      base.LoadContent();
    }

    /// <summary>
    /// Called when graphics resources need to be unloaded. Override this method to unload any component-specific graphics resources.
    /// </summary>
    protected override void UnloadContent()
    {
      base.UnloadContent();
    }
  }
}