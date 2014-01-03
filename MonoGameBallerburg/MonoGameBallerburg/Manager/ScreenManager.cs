// <copyright file="ScreenManager.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;

  using Gameplay;
  using Manager;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  using MonoGameBallerburg.Audio;

  /// <summary>
  /// The screen manager is a component which manages one or more GameScreen
  /// instances. It maintains a stack of screens, calls their Update and Draw
  /// methods at the appropriate times, and automatically routes input to the
  /// topmost active screen.
  /// </summary>
  public class ScreenManager : IScreenManager, IDisposable
  {
    #region Fields

    private readonly IContentManager contentManager;
    private readonly IGameObjectManager gameObjectManager;
    private readonly AudioManager audioManager;

    private MousePointer mousePointer;

    private List<GameScreen> screens = new List<GameScreen>();
    private List<GameScreen> screensToUpdate = new List<GameScreen>();

    private InputState input = new InputState();

    private SpriteBatch spriteBatch;
    private SpriteFont menuFont;
    private Texture2D blankTexture;
    private Texture2D whiteTex;

    private bool isInitialized;

    private bool traceEnabled;

    private IBallerburgGraphicsManager graphicsManager;
    private ApplicationSettings applicationSettings;

    private PlayerSettings[] playerSettings;
    private IShaderManager shaderManager;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ScreenManager"/> class.
    /// </summary>
    /// <param name="graphicsManager">The graphics manager.</param>
    /// <param name="contentManager">The content manager.</param>
    /// <param name="gameObjectManager">The game object manager.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="gameSettings">The game settings.</param>
    /// <param name="shaderManager">The shader manager.</param>
    /// <param name="audioManager">The audio manager.</param>
    /// <param name="playerSettings">The player settings.</param>
    public ScreenManager(
        IBallerburgGraphicsManager graphicsManager,
        IContentManager contentManager,
        IGameObjectManager gameObjectManager,
        ApplicationSettings settings,
        IGameSettingsManager gameSettings,
        IShaderManager shaderManager,
        AudioManager audioManager,
        PlayerSettings[] playerSettings)
    {
      // Load content belonging to the screen manager.
      this.graphicsManager = graphicsManager;
      this.contentManager = contentManager;
      this.gameObjectManager = gameObjectManager;
      this.GameSettings = gameSettings;
      this.shaderManager = shaderManager;
      this.audioManager = audioManager;
      this.playerSettings = playerSettings;
      applicationSettings = settings;
      isInitialized = true;
    }

    #region Properties

    /// <summary>
    /// Gets the sprite batch.
    /// </summary>
    public SpriteBatch SpriteBatch
    {
      get { return spriteBatch; }
    }

    /// <summary>
    /// Gets the font.
    /// </summary>
    public SpriteFont Font
    {
      get { return menuFont; }
    }

    /// <summary>
    /// Gets the menu font.
    /// </summary>
    public SpriteFont MenuFont
    {
      get { return menuFont; }
    }

    /// <summary>
    /// Gets the white tex.
    /// </summary>
    public Texture2D WhiteTex
    {
      get { return whiteTex; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether [trace enabled].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [trace enabled]; otherwise, <c>false</c>.
    /// </value>
    public bool TraceEnabled
    {
      get { return traceEnabled; }
      set { traceEnabled = value; }
    }

    /// <summary>
    /// Gets or sets the game mouse pointer.
    /// </summary>
    /// <value>
    /// The game mouse pointer.
    /// </value>
    public MousePointer GameMousePointer
    {
      get { return mousePointer; }
      set { mousePointer = value; }
    }

    /// <summary>
    /// Gets the mouse position.
    /// </summary>
    public Vector2 MousePosition
    {
      get { return mousePointer.MousePosition; }
    }

    /// <summary>
    /// Gets the content manager.
    /// </summary>
    /// <value>
    /// The content manager.
    /// </value>
    public IContentManager ContentManager
    {
      get { return contentManager; }
    }

    /// <summary>
    /// Gets the game object manager.
    /// </summary>
    public IGameObjectManager GameObjectManager
    {
      get { return gameObjectManager; }
    }

    public IShaderManager ShaderManager
    {
      get { return this.shaderManager; }
    }

    /// <summary>
    /// Gets the application settings.
    /// </summary>
    /// <value>
    /// The application settings.
    /// </value>
    public ApplicationSettings ApplicationSettings
    {
      get
      {
        return applicationSettings;
      }
    }

    /// <summary>
    /// Gets the player settings.
    /// </summary>
    public PlayerSettings[] PlayerSettings
    {
      get
      {
        return playerSettings;
      }
    }

    /// <summary>
    /// Gets the game settings.
    /// </summary>
    public IGameSettingsManager GameSettings { get; private set; }

    /// <summary>
    /// Gets the audio manager.
    /// </summary>
    public AudioManager AudioManager
    {
      get { return this.audioManager; }
    }

    #endregion

    /// <summary>
    /// Initializes the screen manager component.
    /// </summary>
    public void Initialize()
    {
      ////base.Initialize();

      isInitialized = true;
    }

    #region Update and Draw

    /// <summary>
    /// Allows each screen to run logic.
    /// </summary>
    /// <param name="gameTime">Time elapsed since the last call to Update</param>
    public void Update(GameTime gameTime)
    {
      // Read the keyboard and gamepad.
      input.Update();

      // Make a copy of the master screen list, to avoid confusion if
      // the process of updating one screen adds or removes others.
      screensToUpdate.Clear();

      foreach (GameScreen screen in screens)
      {
        screensToUpdate.Add(screen);
      }

      ////screensToUpdate.Reverse();

      bool otherScreenHasFocus = !BallerburgGame.Instance.IsActive;
      bool coveredByOtherScreen = false;

      // Loop as long as there are screens waiting to be updated.
      while (screensToUpdate.Count > 0)
      {
        // Pop the topmost screen off the waiting list.
        GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

        // If this is an active non-popup, inform any subsequent               
        screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

        /*
        // screens that they are covered by it.
        if (screen is GameplayScreen)
        {
            coveredByOtherScreen = false;
        }

        /*
        if (screen is GameplayScreen)
        {
            coveredByOtherScreen = false;
            otherScreenHasFocus = false;
        }
        */

        // Update the screen.
        screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        if (////screen.ScreenState == ScreenState.TransitionOn ||
            screen.ScreenState == ScreenState.Active)
        {
          // If this is the first active screen we came across,
          // give it a chance to handle input.
          if (!otherScreenHasFocus)
          {
            screen.HandleInput(input);

            otherScreenHasFocus = true;
          }

          // If this is an active non-popup, inform any subsequent
          // screens that they are covered by it.
          if (!screen.IsPopup)
          {
            coveredByOtherScreen = true;
          }
        }
      }

      // Print debug trace?
      if (traceEnabled)
      {
        TraceScreens();
      }
    }

    /// <summary>
    /// Prints a list of all the screens, for debugging.
    /// </summary>
    public void TraceScreens()
    {
      ////Trace.WriteLine(string.Join(", ", screens.Select(screen => screen.GetType().Name).ToArray()));
    }

    /// <summary>
    /// Tells each screen to draw itself.
    /// </summary>
    /// <param name="gameTime">Time passed since the last call to Draw.</param>
    public void Draw(GameTime gameTime)
    {
      foreach (var screen in screens)
      {
        if (screen.ScreenState == ScreenState.Hidden)
        {
          continue;
        }

        screen.Draw(gameTime);
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Adds a new screen to the screen manager.
    /// </summary>
    /// <param name="screen">The screen.</param>
    /// <exception cref="ArgumentNullException">screen parameter was null</exception>
    public void AddScreen(GameScreen screen)
    {
      if (screen == null)
      {
        throw new ArgumentNullException("screen");
      }

      screen.ScreenManager = this;
      screen.IsExiting = false;

      // If we have a graphics device, tell the screen to load content.
      if (isInitialized)
      {
        screen.LoadContent();
      }

      screens.Add(screen);
    }

    /// <summary>
    /// Removes a screen from the screen manager. You should normally
    /// use GameScreen.ExitScreen instead of calling this directly, so
    /// the screen can gradually transition off rather than just being
    /// instantly removed.
    /// </summary>
    /// <param name="screen">The added screen.</param>
    public void RemoveScreen(GameScreen screen)
    {
      // If we have a graphics device, tell the screen to unload content.
      if (isInitialized)
      {
        screen.UnloadContent();
      }

      screens.Remove(screen);
      screensToUpdate.Remove(screen);
    }

    /// <summary>
    /// Expose an array holding all the screens. We return a copy rather
    /// than the real master list, because screens should only ever be added
    /// or removed using the AddScreen and RemoveScreen methods.
    /// </summary>
    /// <returns>The Screens contained in this screen manager.</returns>
    public GameScreen[] GetScreens()
    {
      return screens.ToArray();
    }

    /// <summary>
    /// Helper draws a translucent black fullscreen sprite, used for fading
    /// screens in and out, and for darkening the background behind popups.
    /// </summary>
    /// <param name="alpha">The alpha value.</param>
    public void FadeBackBufferToBlack(int alpha)
    {
      Viewport viewport = graphicsManager.GraphicsDevice.Viewport;

      spriteBatch.Begin();

      spriteBatch.Draw(
          blankTexture,
          new Rectangle(0, 0, viewport.Width, viewport.Height),
          new Color(0, 0, 0, (byte)alpha));

      spriteBatch.End();
    }

    #endregion

    #region Content

    /// <summary>
    /// Load your graphics content.
    /// </summary>
    public void LoadContent()
    {
      // Load content belonging to the screen manager.
      spriteBatch = new SpriteBatch(graphicsManager.GraphicsDevice);
      menuFont = contentManager.MenuFont;
      blankTexture = contentManager.BlankTexture;
      whiteTex = contentManager.WhiteTexture;

      // Tell each of the screens to load their content.
      foreach (var screen in screens)
      {
        screen.LoadContent();
      }
    }

    /// <summary>
    /// Unload your graphics content.
    /// </summary>
    public void UnloadContent()
    {
      // Tell each of the screens to unload their content.
      foreach (var screen in screens)
      {
        screen.UnloadContent();
      }
    }

    #endregion

    #region IDisposable

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        spriteBatch.Dispose();
        spriteBatch = null;
      }
    }

    #endregion
  }
}
