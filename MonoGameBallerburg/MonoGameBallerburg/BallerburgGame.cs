// <copyright file="BallerburgGame.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg
{
  using Audio;
  using Gameplay;
  using Manager;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Input;
  using Screens;

  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class BallerburgGame : Game
  {
    public static Game Instance;
    public MousePointer MousePointer;

    private readonly ApplicationSettings applicationSettings;

    private readonly GameObjectManager gameObjectManager;
    private readonly GraphicsDeviceManager graphics;
    private readonly ScreenManager screenManager;
    private readonly GameSettings gameSettings;

    private readonly AudioManager audioManager;
    private readonly ContentManager contentManager;
    private readonly ShaderManager shaderManager;

    private readonly PlayerSettings[] playerSettings;

    private readonly BallerburgGraphicsManager graphicsManager;

    /// <summary>
    /// Initializes a new instance of the BallerburgGame class
    /// </summary>
    public BallerburgGame()
    {
      Instance = this;
      gameSettings = new GameSettings();
      playerSettings = new PlayerSettings[4];

      applicationSettings = new ApplicationSettings();

      graphics = new GraphicsDeviceManager(this)
                     {
                       PreferredBackBufferWidth = 640,
                       PreferredBackBufferHeight = 480
                     };

      graphicsManager = new BallerburgGraphicsManager();
      contentManager = new ContentManager();
      shaderManager = new ShaderManager();
      audioManager = new AudioManager(applicationSettings, contentManager);
      gameObjectManager = new GameObjectManager(contentManager, this.audioManager, this.graphicsManager);

      MousePointer = new MousePointer(this)
                         {
                           DrawOrder = 1000,
                           RestrictZone = new Rectangle(0, 0, 640, 480)
                         };
      Components.Add(MousePointer);

      // Create the screen manager component.
      screenManager = new ScreenManager(graphicsManager, contentManager, gameObjectManager, applicationSettings, gameSettings, shaderManager, audioManager, playerSettings)
                          {
                            GameMousePointer = MousePointer
                          };
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      graphicsManager.Initialize(graphics);
      contentManager.LoadContent(Content, graphicsManager);
      shaderManager.LoadContent(Content, graphicsManager);
      screenManager.LoadContent();
      gameObjectManager.LoadContent();

      // Activate the first screens.
      screenManager.AddScreen(new BackgroundScreen(graphicsManager));
      screenManager.AddScreen(new StartScreen(graphicsManager, screenManager));

      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      MousePointer.PointerTexture = contentManager.MousePointerTexture;

      this.audioManager.PlayMenuBackgroundMusic();
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent()
    {
      //// TODO: Unload any non ContentManager content here
      screenManager.UnloadContent();
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      // Allows the game to exit
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
      {
        Exit();
      }

      this.audioManager.Update();
      screenManager.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      ////GraphicsManager.GraphicsDevice.Clear(Color.CornflowerBlue);            
      ///// TODO: Add your drawing code here            
      screenManager.Draw(gameTime);

      base.Draw(gameTime);
    }
  }
}
