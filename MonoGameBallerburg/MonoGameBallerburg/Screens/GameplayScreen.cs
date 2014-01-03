// <copyright file="GameplayScreen.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Screens
{
  using System;
  using System.Globalization;
  using System.Linq;
  using Controls;
  using Gameplay;
  using Graphic;
  using Manager;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Input;

  /// <summary>
  /// The game Screen and its logic
  /// TODO: Powder and start velocity
  /// TODO: If cannon lost to change to, go to castle view
  /// TODO: If cannon destroyed, do a castle view move
  /// TODO: HUD Update
  /// TODO: Game Over Screen
  /// TODO: KI Animation (move cannon)
  /// TODO: Directional Light
  /// TODO: Daytime
  /// TODO: Season
  /// TODO: Weather
  /// TODO: Weather: Rain
  /// TODO: Weather: Fog  
  /// TODO: Correct Initialization of Bezier Curve for Cannon Camera
  /// TODO: Correct Initialization of Bezier Curve for Castle Camera
  /// </summary>
  public class GameplayScreen : GameScreen, IDisposable
  {
    #region Fields

    // Offsets for the bitmap containing the Menuitems
    private const int TurnFastLeftx = 2;
    private const int TurnFastLefty = 0;
    private const int TurnLeftx = 0;
    private const int TurnLefty = 0;
    private const int Castlex = 224;
    private const int Castley = 0;
    private const int Towerx = 192;
    private const int Towery = 0;
    private const int Cannonx = 0;
    private const int Cannony = 1;
    private const int IronKugelx = 4;
    private const int IronKugely = 1;
    private const int StoneKugelx = 5;
    private const int StoneKugely = 1;
    private const int PowderPlusx = 6;
    private const int PowderPlusy = 1;
    private const int PowderMinusx = 7;
    private const int PowderMinusy = 1;
    private const int ButtonWidth = 32;

    private const float FFHorizontalRotationSpeed = 0.25f;
    private const float HorizontalRotationSpeed = 0.025f;
    private const float VertikalRotationSpeed = 0.025f;
    private const float CastleCameraStartPosX = 0.0f;
    private const float CastleCameraStartPosY = 20.0f;
    private const float CastleCameraStartPosZ = -30.0f;
    private const int MaxCannonBalls = 100;
    private const int PowderPlusMinusAmount = 50;
    private const int MaxPowderAmount = 1000;
    private const int MinPowderAmount = 150;
    private const int MaxTextShowTime = 3000;

    // Menu Buttons
    private GameplayMenuItem btnFfRight;
    private GameplayMenuItem btnRight;
    private GameplayMenuItem btnCamUp;
    private GameplayMenuItem btnCamDown;
    private GameplayMenuItem btnMoney;
    private GameplayMenuItem btnSwitchPlayer;
    private GameplayMenuItem btnFire;
    private GameplayMenuItem btnLessPowder;
    private GameplayMenuItem btnMorePowder;
    private GameplayMenuItem btnSwitchStone;
    private GameplayMenuItem btnSwitchIron;
    private GameplayMenuItem btnSelectCannon;
    private GameplayMenuItem btnSwitchTower;
    private GameplayMenuItem btnSelectTower;
    private GameplayMenuItem btnSelectCastle;
    private GameplayMenuItem btnLeft;
    private GameplayMenuItem btnFfLeft;
    private GamePlayMenuLabel lblHoverText;
    private GamePlayMenuLabel lblInfoText;
    private int showTextCounter = 0;

    private GraphicsDevice graphics;

    // Camera
    private Camera camera;
    private bool enableRotation;
    private bool doCameraMove;
    private float cameraYaw;
    private float cameraPitch;

    // Shadow
    ////Random random = new Random();
    private Hud hud;
    private Texture2D shadowMap;
    private RenderTarget2D renderTarget;
    private Effect effect;

    // The size of the shadow map
    // The larger the size the more detail we will have for our entire scene
    ////private const int ShadowMapWidthHeight = 2048;

    // Light direction
    private Vector3 lightPos;
    private float lightPower;
    private float ambientPower;
    private Matrix lightsViewProjectionMatrix;

    ////Vector3 lightDir = new Vector3(-0.3333333f, 0.6666667f, 0.6666667f);
    ////BoundingFrustum cameraFrustum = new BoundingFrustum(Matrix.Identity);

    /// <summary>
    /// The player currently active
    /// </summary>
    private int activePlayer;
    private int winnerId;
    private bool cannonFired;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="GameplayScreen"/> class.
    /// </summary>
    /// <param name="graphicsManager">The graphics manager.</param>
    public GameplayScreen(IBallerburgGraphicsManager graphicsManager)
      : base(graphicsManager)
    {
      TransitionOnTime = TimeSpan.FromSeconds(1.5);
      TransitionOffTime = TimeSpan.FromSeconds(0);
      doCameraMove = false;

      // Init the game - logic
      InitGameLogic();
    }

    /// <summary>
    /// Load graphics content for the game.
    /// - The initial view is the castle view
    /// - The initally selected tower is intialized is tower 0
    /// </summary>
    public override void LoadContent()
    {
      btnFfRight = new GameplayMenuItem(this, new Rectangle(96, 1, ButtonWidth, ButtonWidth), new Rectangle(600, 420, ButtonWidth, ButtonWidth), 1);
      btnRight = new GameplayMenuItem(this, new Rectangle(32, 1, ButtonWidth, ButtonWidth), new Rectangle(560, 420, ButtonWidth, ButtonWidth), 2);
      btnCamDown = new GameplayMenuItem(this, new Rectangle(160, 1, ButtonWidth, ButtonWidth), new Rectangle(520, 420, ButtonWidth, ButtonWidth), 3);
      btnCamUp = new GameplayMenuItem(this, new Rectangle(128, 1, ButtonWidth, ButtonWidth), new Rectangle(480, 420, ButtonWidth, ButtonWidth), 4);
      btnSwitchPlayer = new GameplayMenuItem(this, new Rectangle(1, 64, ButtonWidth, ButtonWidth), new Rectangle(400, 420, ButtonWidth, ButtonWidth), 6);
      btnFire = new GameplayMenuItem(this, new Rectangle(64, 32, ButtonWidth, ButtonWidth), new Rectangle(320, 420, ButtonWidth, ButtonWidth), 6);
      btnLessPowder = new GameplayMenuItem(this, new Rectangle(7 * 32, 32, ButtonWidth, ButtonWidth), new Rectangle(280, 420, ButtonWidth, ButtonWidth), 6);
      btnMorePowder = new GameplayMenuItem(this, new Rectangle(6 * 32, 32, ButtonWidth, ButtonWidth), new Rectangle(240, 420, ButtonWidth, ButtonWidth), 6);
      btnSwitchStone = new GameplayMenuItem(this, new Rectangle(4 * 32, 32, ButtonWidth, ButtonWidth), new Rectangle(200, 420, ButtonWidth, ButtonWidth), 6);
      btnSwitchIron = new GameplayMenuItem(this, new Rectangle(5 * 32, 32, ButtonWidth, ButtonWidth), new Rectangle(200, 420, ButtonWidth, ButtonWidth), 6);
      btnSelectCannon = new GameplayMenuItem(this, new Rectangle(1, 32, ButtonWidth, ButtonWidth), new Rectangle(160, 420, ButtonWidth, ButtonWidth), 3);
      btnSwitchTower = new GameplayMenuItem(this, new Rectangle(32, 32, ButtonWidth, ButtonWidth), new Rectangle(120, 420, ButtonWidth, ButtonWidth), 3) { Visible = false };
      btnSelectTower = new GameplayMenuItem(this, new Rectangle(6 * 32, 1, ButtonWidth, ButtonWidth), new Rectangle(120, 420, ButtonWidth, ButtonWidth), 3);
      btnSelectCastle = new GameplayMenuItem(this, new Rectangle(7 * 32, 1, ButtonWidth, ButtonWidth), new Rectangle(80, 420, ButtonWidth, ButtonWidth), 3);
      btnLeft = new GameplayMenuItem(this, new Rectangle(1, 1, ButtonWidth, ButtonWidth), new Rectangle(40, 420, ButtonWidth, ButtonWidth), 8);
      btnFfLeft = new GameplayMenuItem(this, new Rectangle(64, 1, ButtonWidth, ButtonWidth), new Rectangle(1, 420, ButtonWidth, ButtonWidth), 9);

      lblHoverText = new GamePlayMenuLabel(this, 12, GraphicsManager) { Text = string.Empty, Position = new Vector2(0.0f, 444.0f) };
      lblInfoText = new GamePlayMenuLabel(this, 13, GraphicsManager) { Text = string.Empty, Position = new Vector2(300.0f, 100.0f) };

      ControlsContainer.Add(btnFfRight);
      ControlsContainer.Add(btnRight);
      ControlsContainer.Add(btnCamUp);
      ControlsContainer.Add(btnCamDown);
      ControlsContainer.Add(btnSwitchPlayer);
      ControlsContainer.Add(btnFire);
      ControlsContainer.Add(btnLessPowder);
      ControlsContainer.Add(btnMorePowder);
      ControlsContainer.Add(btnSwitchStone);
      ControlsContainer.Add(btnSwitchIron);
      ControlsContainer.Add(btnSelectCannon);
      ControlsContainer.Add(btnSwitchTower);
      ControlsContainer.Add(btnSelectTower);
      ControlsContainer.Add(btnSelectCastle);
      ControlsContainer.Add(btnLeft);
      ControlsContainer.Add(btnFfLeft);
      ControlsContainer.Add(lblHoverText);
      ControlsContainer.Add(lblInfoText);

      btnFfRight.MouseOverText = "Schnell nach rechts";
      btnFfRight.MouseDown += PerformOnFfRightDown;
      btnFfRight.MouseUp += PerformOnFfRightUp;
      btnFfRight.MouseEnter += PerformOnMouseEnter;
      btnFfRight.MouseLeave += PerformOnMouseLeave;

      btnRight.MouseOverText = "Nach rechts";
      btnRight.MouseDown += PerformOnRightDown;
      btnRight.MouseEnter += PerformOnMouseEnter;
      btnRight.MouseLeave += PerformOnMouseLeave;
      btnRight.MouseUp += PerformOnRightUp;

      btnCamUp.MouseOverText = "Nach oben";
      btnCamUp.MouseDown += PerformOnUpDown;
      btnCamUp.MouseUp += PerformOnUpUp;
      btnCamUp.MouseEnter += PerformOnMouseEnter;
      btnCamUp.MouseLeave += PerformOnMouseLeave;

      btnCamDown.MouseOverText = "Nach unten";
      btnCamDown.MouseDown += PerformOnDownDown;
      btnCamDown.MouseUp += PerformOnDownUp;
      btnCamDown.MouseEnter += PerformOnMouseEnter;
      btnCamDown.MouseLeave += PerformOnMouseLeave;

      btnSelectCannon.MouseOverText = "Kanonenansicht";
      btnSelectCannon.MouseUp += PerformOnCannonViewUp;
      btnSelectCannon.MouseEnter += PerformOnMouseEnter;
      btnSelectCannon.MouseLeave += PerformOnMouseLeave;

      btnSelectTower.MouseOverText = "Turmansicht";
      btnSelectTower.MouseUp += PerformOnTowerViewUp;
      btnSelectTower.MouseEnter += PerformOnMouseEnter;
      btnSelectTower.MouseLeave += PerformOnMouseLeave;

      btnSelectCastle.MouseOverText = "Burgansicht";
      btnSelectCastle.MouseUp += PerformOnCastleViewUp;
      btnSelectCastle.MouseEnter += PerformOnMouseEnter;
      btnSelectCastle.MouseLeave += PerformOnMouseLeave;

      btnLeft.MouseOverText = "Nach links";
      btnLeft.MouseDown += PerformOnLeftDown;
      btnLeft.MouseUp += PerformOnLeftUp;
      btnLeft.MouseEnter += PerformOnMouseEnter;
      btnLeft.MouseLeave += PerformOnMouseLeave;

      btnFfLeft.MouseOverText = "Schnell nach links";
      btnFfLeft.MouseDown += PerformOnFfLeftDown;
      btnFfLeft.MouseUp += PerformOnFfLeftUp;
      btnFfLeft.MouseEnter += PerformOnMouseEnter;
      btnFfLeft.MouseLeave += PerformOnMouseLeave;

      btnSwitchTower.MouseOverText = "Turm wechseln";
      btnSwitchTower.MouseUp += PerformOnTowerSwitchUp;
      btnSwitchTower.MouseEnter += PerformOnMouseEnter;
      btnSwitchTower.MouseLeave += PerformOnMouseLeave;

      btnFire.MouseOverText = "Feuer";
      btnFire.MouseUp += PerformOnFireDown;
      btnFire.MouseEnter += PerformOnMouseEnter;
      btnFire.MouseLeave += PerformOnMouseLeave;

      if (ScreenManager.GameSettings.GameStyle == GameType.Klassisch)
      {
        btnMoney = new GameplayMenuItem(this, new Rectangle(32, 64, 32, 32), new Rectangle(440, 420, 32, 32), 5);

        ControlsContainer.Add(btnMoney);

        btnMoney.MouseOverText = "Geldverwaltung";
        btnMoney.MouseUp += PerformOnMoneyDown;
        btnMoney.MouseEnter += PerformOnMouseEnter;
        btnMoney.MouseLeave += PerformOnMouseLeave;
      }

      btnSwitchPlayer.MouseOverText = "Runde Beenden";
      btnSwitchPlayer.MouseUp += PerformOnPlayerSwitchDown;
      btnSwitchPlayer.MouseEnter += PerformOnMouseEnter;
      btnSwitchPlayer.MouseLeave += PerformOnMouseLeave;

      btnLessPowder.MouseOverText = "Weniger Pulver";
      btnLessPowder.MouseUp += PerformOnPowderMinus;
      btnLessPowder.MouseEnter += PerformOnMouseEnter;
      btnLessPowder.MouseLeave += PerformOnMouseLeave;

      btnMorePowder.MouseOverText = "Mehr Pulver";
      btnMorePowder.MouseUp += PerformOnPowderPlus;
      btnMorePowder.MouseEnter += PerformOnMouseEnter;
      btnMorePowder.MouseLeave += PerformOnMouseLeave;

      btnSwitchIron.MouseOverText = "Eisenkugel";
      btnSwitchIron.MouseUp += PerformOnCannonBallSwitch;
      btnSwitchIron.MouseEnter += PerformOnMouseEnter;
      btnSwitchIron.MouseLeave += PerformOnMouseLeave;

      btnSwitchStone.MouseOverText = "Steinkugel";
      btnSwitchStone.MouseUp += PerformOnCannonBallSwitch;
      btnSwitchStone.MouseEnter += PerformOnMouseEnter;
      btnSwitchStone.MouseLeave += PerformOnMouseLeave;

      btnSelectCastle.Visible = false;
      btnSelectCannon.Visible = false;

      this.ScreenActivated += PerformOnActivate;

      // Init the game - graphic
      if (graphics == null)
      {
        graphics = GraphicsManager.GraphicsDevice;
      }

      InitPlayerGraphics();

      effect = ScreenManager.ShaderManager.TheEffect;

      var pp = GraphicsManager.GraphicsDevice.PresentationParameters;
      renderTarget = new RenderTarget2D(GraphicsManager.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, GraphicsManager.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
      hud = new Hud(this);

      // Init the game - logic
      InitNewGame();

      // once the load has finished, we use ResetElapsedTime to tell the game's
      // timing mechanism that we have just finished a very long frame, and that
      // it should not try to catch up.
      BallerburgGame.Instance.ResetElapsedTime();
    }

    private void InitPlayerGraphics()
    {
      // Init the players castle from what castles they have chosen
      for (var i = 0; i < 4; i++)
      {
        if (ScreenManager.PlayerSettings[i].IsActive)
        {
          ScreenManager.PlayerSettings[i].Castle.Towers.Clear();

          var tow = 0;

          // The player has chosen a castle, now, setup the castle for the player
          foreach (var t in ScreenManager.GameObjectManager.Castles[ScreenManager.PlayerSettings[i].Castle.CastleType].Towers)
          {
            var ts = new TowerSettings(t, GraphicsManager)
            {
              HasCannon = ScreenManager.GameObjectManager.Castles[ScreenManager.PlayerSettings[i].Castle.CastleType].Towers[tow].HasCannon
            };

            ScreenManager.PlayerSettings[i].Castle.Towers.Add(ts);
            tow++;
          }

          ScreenManager.PlayerSettings[i].Castle.SelectTower(0);
        }
      }

      // Init all castle camera positions
      for (var i = 3; i >= 0; i--)
      {
        ScreenManager.PlayerSettings[i].CurrentCameraState = CameraState.CastleView;
        camera = new Camera(GraphicsManager) { ViewDirection = ScreenManager.PlayerSettings[i].Castle.StartPos };
        camera.UpdateCamera(
            new Vector3(
                ScreenManager.PlayerSettings[i].Castle.StartPos.X + CastleCameraStartPosX,
                ScreenManager.PlayerSettings[i].Castle.StartPos.Y + CastleCameraStartPosY,
                ScreenManager.PlayerSettings[i].Castle.StartPos.Z + CastleCameraStartPosZ),
            1.0f,
            0.0f);

        Matrix trans = Matrix.Invert(Matrix.CreateTranslation(ScreenManager.PlayerSettings[i].Castle.StartPos));

        if (camera.State == CameraState.CastleView)
        {
          ScreenManager.PlayerSettings[i].Castle.CameraPosition = Vector3.Transform(camera.Position, trans);
        }

        if (ScreenManager.PlayerSettings[i].IsActive)
        {
          for (var j = 0; j < ScreenManager.GameObjectManager.Castles[ScreenManager.PlayerSettings[i].Castle.CastleType].Towers.Count; j++)
          {
            ScreenManager.PlayerSettings[i].Castle.Towers[j].CameraPosition =
                new Vector3(
                    ScreenManager.GameObjectManager.Castles[ScreenManager.PlayerSettings[i].Castle.CastleType].Towers[j].X,
                    ScreenManager.GameObjectManager.Castles[ScreenManager.PlayerSettings[i].Castle.CastleType].Towers[j].CoverHeight + 2,
                    ScreenManager.GameObjectManager.Castles[ScreenManager.PlayerSettings[i].Castle.CastleType].Towers[j].Y);
          }
        }
      }
    }

    /// <summary>
    /// Unload graphics content used by the game.
    /// </summary>
    public override void UnloadContent()
    {
      ////this.content.Unload();
    }

    /*
    /// <summary>
    /// Creates the WorldViewProjection matrix from the perspective of the 
    /// light using the cameras bounding frustum to determine what is visible 
    /// in the scene.
    /// </summary>
    /// <returns>The WorldViewProjection for the light</returns>
    public Matrix CreateLightViewProjectionMatrix()
    {
        // Matrix with that will rotate in points the direction of the light
        Matrix lightRotation = Matrix.CreateLookAt(Vector3.Zero,
                                                   -lightDir,
                                                   Vector3.Up);

        // Get the corners of the frustum
        Vector3[] frustumCorners = cameraFrustum.GetCorners();

        // Transform the positions of the corners into the direction of the light
        for (int i = 0; i < frustumCorners.Length; i++)
        {
            frustumCorners[i] = Vector3.Transform(frustumCorners[i], lightRotation);
        }

        // Find the smallest box around the points
        BoundingBox lightBox = BoundingBox.CreateFromPoints(frustumCorners);

        Vector3 boxSize = lightBox.Max - lightBox.Min;
        Vector3 halfBoxSize = boxSize * 0.5f;

        // The position of the light should be in the center of the back
        // pannel of the box. 
        Vector3 lightPosition = lightBox.Min + halfBoxSize;
        lightPosition.Z = lightBox.Min.Z;

        // We need the position back in world coordinates so we transform 
        // the light position by the inverse of the lights rotation
        lightPosition = Vector3.Transform(lightPosition,
                                          Matrix.Invert(lightRotation));

        // Create the view matrix for the light
        Matrix lightView = Matrix.CreateLookAt(lightPosition,
                                               lightPosition - lightDir,
                                               Vector3.Up);

        // Create the projection matrix for the light
        // The projection is orthographic since we are using a directional light
        Matrix lightProjection = Matrix.CreateOrthographic(boxSize.X, boxSize.Y,
                                                           -boxSize.Z, boxSize.Z);

        return lightView * lightProjection;
    }
    */

    /// <summary>
    /// Disables the input.
    /// </summary>
    private void DisableInput()
    {
      btnFfRight.Enabled = false;
      btnRight.Enabled = false;
      btnCamUp.Enabled = false;
      btnCamDown.Enabled = false;
      btnMoney.Enabled = false;
      btnSwitchPlayer.Enabled = false;
      btnFire.Enabled = false;
      btnLessPowder.Enabled = false;
      btnMorePowder.Enabled = false;
      btnSwitchStone.Enabled = false;
      btnSwitchIron.Enabled = false;
      btnSelectCannon.Enabled = false;
      btnSwitchTower.Enabled = false;
      btnSelectTower.Enabled = false;
      btnSelectCastle.Enabled = false;
      btnLeft.Enabled = false;
      btnFfLeft.Enabled = false;
    }

    /// <summary>
    /// Enables the input.
    /// </summary>
    private void EnableInput()
    {
      if (btnFfRight.Visible)
      {
        btnFfRight.Enabled = true;
      }

      if (btnRight.Visible)
      {
        btnRight.Enabled = true;
      }

      if (btnCamUp.Visible)
      {
        btnCamUp.Enabled = true;
      }

      if (btnCamDown.Visible)
      {
        btnCamDown.Enabled = true;
      }

      if (btnMoney != null && btnMoney.Visible)
      {
        btnMoney.Enabled = true;
      }

      if (btnSwitchPlayer.Visible)
      {
        btnSwitchPlayer.Enabled = true;
      }

      if (btnFire.Visible)
      {
        btnFire.Enabled = true;
      }

      if (btnLessPowder.Visible)
      {
        btnLessPowder.Enabled = true;
      }

      if (btnMorePowder.Visible)
      {
        btnMorePowder.Enabled = true;
      }

      if (btnSwitchStone.Visible)
      {
        btnSwitchStone.Enabled = true;
      }

      if (btnSwitchIron.Visible)
      {
        btnSwitchIron.Enabled = true;
      }

      if (btnSelectCannon.Visible)
      {
        btnSelectCannon.Enabled = true;
      }

      if (btnSwitchTower.Visible)
      {
        btnSwitchTower.Enabled = true;
      }

      if (btnSelectTower.Visible)
      {
        btnSelectTower.Enabled = true;
      }

      if (btnSelectCastle.Visible)
      {
        btnSelectCastle.Enabled = true;
      }

      if (btnLeft.Visible)
      {
        btnLeft.Enabled = true;
      }

      if (btnFfLeft.Visible)
      {
        btnFfLeft.Enabled = true;
      }
    }

    #region Update and Draw

    /// <summary>
    /// Updates the state of the game. This method checks the GameScreen.IsActive
    /// property, so the game will stop updating when the pause menu is active,
    /// or if you tab away to a different application.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    /// <param name="otherScreenHasFocus">if set to <c>true</c> [other screen has focus].</param>
    /// <param name="coveredByOtherScreen">if set to <c>true</c> [covered by other screen].</param>
    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
      if (lblInfoText.Text != string.Empty)
      {
        showTextCounter += gameTime.ElapsedGameTime.Milliseconds;
        if (showTextCounter >= MaxTextShowTime)
        {
          lblInfoText.Text = string.Empty;
          showTextCounter = 0;
        }
      }

      if (IsActive)
      {
        camera.UpdateBezier();
        if (camera.State != CameraState.Animated)
        {
          EnableInput();
        }

        if (doCameraMove)
        {
          if (camera.State == CameraState.Cannon)
          {
            camera.UpdateCamera(camera.Position, cameraYaw, cameraPitch);

            if (ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.HasCannon)
            {
              ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.RotationAngleYaw = camera.Yaw;

              if (camera.Pitch > 0.0f)
              {
                ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.RotationAnglePitch = 0.0f;
              }
              else
              {
                ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.RotationAnglePitch = camera.Pitch;
              }
            }
          }
          else
          {
            camera.UpdateCamera(camera.Position, cameraYaw, cameraPitch);
          }
        }

        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
          if (enableRotation == false)
          {
            enableRotation = true;
          }
        }
        else if (Mouse.GetState().LeftButton == ButtonState.Released)
        {
          enableRotation = false;
        }

        ScreenManager.GameObjectManager.Update(gameTime);

        CheckForCollision();
        CheckForGameOver();

        UpdateLightData();
      }
    }

    /// <summary>
    /// Lets the game respond to player input. Unlike the Update method,
    /// this will only be called when the gameplay screen is active.
    /// </summary>
    /// <param name="input">The input.</param>
    public override void HandleInput(InputState input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      if (input.PauseGame)
      {
        // If they pressed pause, bring up the pause menu screen.
        ////ScreenManager.AddScreen(new PauseMenuScreen());
      }
    }

    /// <summary>
    /// Draws the hud.
    /// </summary>
    public void DrawHud()
    {
      ScreenManager.SpriteBatch.Begin();
      DrawString(ScreenManager.Font, ScreenManager.PlayerSettings[activePlayer].Steinkugel.ToString(CultureInfo.InvariantCulture) + " " + ResourceLoader.GetString("StoneBallsText"), new Vector2(10, 10), Color.White);
      DrawString(ScreenManager.Font, ScreenManager.PlayerSettings[activePlayer].Eisenkugeln.ToString(CultureInfo.InvariantCulture) + " " + ResourceLoader.GetString("IronBallsText"), new Vector2(10, 40), Color.White);
      DrawString(ScreenManager.Font, ScreenManager.PlayerSettings[activePlayer].Powder.ToString(CultureInfo.InvariantCulture) + " " + ResourceLoader.GetString("PowderText"), new Vector2(10, 70), Color.White);
      ScreenManager.SpriteBatch.End();
    }

    /// <summary>
    /// Draws the gameplay screen.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public override void Draw(GameTime gameTime)
    {
      // This game has a blue background. Why? Because!
      // Render the scene to the shadow map
      GraphicsManager.GraphicsDevice.SetRenderTarget(renderTarget);
      GraphicsManager.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

      DrawScene("ShadowMap");

      GraphicsManager.GraphicsDevice.SetRenderTarget(null);

      shadowMap = renderTarget;

      GraphicsManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

      GraphicsManager.GraphicsDevice.BlendState = BlendState.Opaque;
      GraphicsManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

      GraphicsManager.GraphicsDevice.DepthStencilState = DepthStencilState.None;
      GraphicsManager.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;

      ScreenManager.GameObjectManager.SkyBox.Draw(gameTime, camera.ViewMatrix, camera.ProjMatrix);

      DrawScene("ShadowedScene");

      if (!ScreenManager.GameObjectManager.ExplosionBillboard.IsPaused)
      {
        ScreenManager.GameObjectManager.ExplosionBillboard.Camera = camera;
        ScreenManager.GameObjectManager.ExplosionBillboard.Draw(gameTime, ScreenManager.ShaderManager.BasicEffect, GraphicsManager.GraphicsDevice);
      }

      ////this.DrawShadowMapToScreen();

      // If the game is transitioning on or off, fade it out to black.
      if (TransitionPosition > 0)
      {
        ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
      }

      if (ScreenManager.GameSettings.GameStyle == GameType.Klassisch)
      {
        DrawHud();
      }

      shadowMap = null;

      base.Draw(gameTime);
    }

    /// <summary>
    /// Updates the light data.
    /// </summary>
    private void UpdateLightData()
    {
      ambientPower = 0.5f;

      lightPos = new Vector3(-18, 5, -2);
      lightPower = 1.0f;

      var lightsView = Matrix.CreateLookAt(lightPos, new Vector3(0, 0, 0), new Vector3(0, 1, 0));
      var lightsProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1f, 5f, 100f);
      ////lightsProjection = Matrix.CreateOrthographic(250, 100, 5f, 100f);

      lightsViewProjectionMatrix = lightsView * lightsProjection;
    }

    /// <summary>
    /// Draws the scene.
    /// </summary>
    /// <param name="technique">The technique.</param>
    private void DrawScene(string technique)
    {
      effect.CurrentTechnique = effect.Techniques[technique];
      effect.Parameters["xWorldViewProjection"].SetValue(Matrix.Identity * camera.ViewMatrix * camera.ProjMatrix);
      effect.Parameters["xWorld"].SetValue(Matrix.Identity);
      effect.Parameters["xLightPos"].SetValue(lightPos);
      effect.Parameters["xLightPower"].SetValue(lightPower);
      effect.Parameters["xAmbient"].SetValue(ambientPower);
      effect.Parameters["xLightsWorldViewProjection"].SetValue(Matrix.Identity * lightsViewProjectionMatrix);
      effect.Parameters["xShadowMap"].SetValue(shadowMap);

      ScreenManager.GameObjectManager.Terrain.Draw(camera.ViewMatrix, camera.ProjMatrix, effect, GraphicsManager.GraphicsDevice);

      foreach (var ps in ScreenManager.PlayerSettings.Where(ps => ps.IsActive))
      {
        for (int i = 0; i < ps.Castle.Towers.Count; i++)
        {
          ScreenManager.GameObjectManager.Castles[ps.Castle.CastleType].SelectTower(i);
          ScreenManager.GameObjectManager.Castles[ps.Castle.CastleType].CurrentTower.DrawCannon = ps.Castle.Towers[i].HasCannon;
          ScreenManager.GameObjectManager.Castles[ps.Castle.CastleType].CurrentTower.TowerCannon = ps.Castle.Towers[i].TowerCannon;
        }

        ScreenManager.GameObjectManager.Castles[ps.Castle.CastleType].Position = ps.Castle.StartPos;
        ScreenManager.GameObjectManager.Castles[ps.Castle.CastleType].Draw(
            Matrix.CreateTranslation(ps.Castle.StartPos),
            camera.ViewMatrix,
            camera.ProjMatrix,
            lightsViewProjectionMatrix,
            effect,
            ScreenManager.ContentManager,
            GraphicsManager.GraphicsDevice);
      }

      foreach (var cannonBall in ScreenManager.GameObjectManager.ActiveCannonballs)
      {
        if (cannonBall.Alive)
        {
          cannonBall.Draw(
              camera.ViewMatrix,
              camera.ProjMatrix,
              lightsViewProjectionMatrix,
              effect);
        }
      }
    }

    /// <summary>
    /// Draws the shadow map to screen.
    /// </summary>
    public void DrawShadowMapToScreen()
    {
      ScreenManager.SpriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
      ScreenManager.SpriteBatch.Draw(shadowMap, new Rectangle(0, 0, 128, 128), Color.White);
      ScreenManager.SpriteBatch.End();

      GraphicsManager.GraphicsDevice.Textures[0] = null;
      GraphicsManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
    }

    /// <summary>
    /// Checks for collision.
    /// </summary>
    private void CheckForCollision()
    {
      // Check for each enemy player, each tower: if it has a cannon, whether one of the bullets hits the cannon
      for (var i = 0; i < 4; i++)
      {
        if ((i != activePlayer) && ScreenManager.PlayerSettings[i].IsActive)
        {
          foreach (var t in ScreenManager.PlayerSettings[i].Castle.Towers)
          {
            if (t.HasCannon)
            {
              // Get the position of the cannon
              var cannonPos = new Vector3(
                  ScreenManager.GameObjectManager.Castles[ScreenManager.PlayerSettings[i].Castle.CastleType].Towers[t.ID - 1].X,
                  ScreenManager.GameObjectManager.Castles[ScreenManager.PlayerSettings[i].Castle.CastleType].Towers[t.ID - 1].CoverHeight + 0.5f,
                  ScreenManager.GameObjectManager.Castles[ScreenManager.PlayerSettings[i].Castle.CastleType].Towers[t.ID - 1].Y);

              cannonPos = Vector3.Transform(cannonPos, Matrix.CreateTranslation(ScreenManager.PlayerSettings[i].Castle.StartPos));

              // Now, use a rect as Bounding Volume
              var canBox = new BoundingBox(cannonPos - new Vector3(1.0f, 0.5f, 1.0f), cannonPos + new Vector3(1.0f, 0.5f, 1.0f));

              foreach (var c in ScreenManager.GameObjectManager.ActiveCannonballs)
              {
                if (c.Alive)
                {
                  var bs = new BoundingSphere(c.Position, 1.0f);

                  if (canBox.Intersects(bs))
                  {
                    ScreenManager.PlayerSettings[i].Castle.Towers[t.ID - 1].HasCannon = false;
                    cannonPos.Y = cannonPos.Y - 0.5f;
                    ScreenManager.GameObjectManager.ExplosionBillboard.Position = cannonPos;
                    ScreenManager.GameObjectManager.ExplosionBillboard.Play();
                    ScreenManager.AudioManager.PlayExplosionSound();
                    c.Alive = false;
                  }
                }
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Checks for game over.
    /// </summary>
    private void CheckForGameOver()
    {
      // Check for each enemy player, each tower: if it has a cannon, whether one of the bullets hits the cannon
      for (var i = 0; i < 4; i++)
      {
        if ((i != activePlayer) && ScreenManager.PlayerSettings[i].IsActive)
        {
          if (ScreenManager.PlayerSettings[i].NumCannons() == 0)
          {
            winnerId = activePlayer;

            // Reset castle objects
            foreach (var ps in ScreenManager.PlayerSettings)
            {
              if (ps.IsActive)
              {
                for (var j = 0; j < ps.Castle.Towers.Count; j++)
                {
                  ScreenManager.GameObjectManager.Castles[ps.Castle.CastleType].SelectTower(j);
                  ScreenManager.GameObjectManager.Castles[ps.Castle.CastleType].CurrentTower.DrawCannon = ScreenManager.GameObjectManager.Castles[ps.Castle.CastleType].CurrentTower.HasCannon;
                }
              }
            }

            var mainMenuScreen = new MainMenuScreen(GraphicsManager, ScreenManager);
            ScreenManager.AddScreen(mainMenuScreen);
            ScreenManager.AudioManager.PlayMenuBackgroundMusic();
            ExitScreen();
          }
        }
      }
    }

    private void InitGameLogic()
    {
      // Init gamelogics settings
      activePlayer = 0;
      cannonFired = false;
    }

    #endregion

    #region InputHandlers

    /// <summary>
    /// Event handler for reacting on the FFRight Action.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnFfRightDown(object sender, EventArgs e)
    {
      if (camera.State == CameraState.CastleView)
      {
        cameraYaw = 0.05f;
      }
      else
      {
        cameraYaw = -0.05f;
      }

      if (camera.State == CameraState.Cannon)
      {
        ScreenManager.AudioManager.PlayCannonRotateSound();
      }

      cameraPitch = 0.0f;
      btnRight.Enabled = false;
      btnCamDown.Enabled = false;
      btnCamUp.Enabled = false;
      btnSelectCannon.Enabled = false;
      btnSelectTower.Enabled = false;
      btnSelectCastle.Enabled = false;
      btnLeft.Enabled = false;
      btnFfLeft.Enabled = false;
      doCameraMove = true;
      ////camera.UpdateCamera(camera.Position, 0.05f, 0.0f);
    }

    /// <summary>
    /// Event handler for reacting on the FFRight Action.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnFfRightUp(object sender, EventArgs e)
    {
      doCameraMove = false;
      btnRight.Enabled = true;
      btnCamDown.Enabled = true;
      btnCamUp.Enabled = true;
      btnSelectCannon.Enabled = true;
      btnSelectTower.Enabled = true;
      btnSelectCastle.Enabled = true;
      btnLeft.Enabled = true;
      btnFfLeft.Enabled = true;
      ////camera.UpdateCamera(camera.Position, 0.05f, 0.0f);
      ScreenManager.AudioManager.StopCannonRotateSound();
    }

    /// <summary>
    /// Performs the on mouse enter.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="ButtonEventArgs"/> instance containing the event data.</param>
    private void PerformOnMouseEnter(object sender, ButtonEventArgs e)
    {
      lblHoverText.Text = e.MouseOverText;
    }

    /// <summary>
    /// Event handler for reacting on the Right Action.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnRightDown(object sender, EventArgs e)
    {
      if (camera.State == CameraState.CastleView)
      {
        cameraYaw = 0.025f;
      }
      else
      {
        cameraYaw = -0.025f;
      }

      if (camera.State == CameraState.Cannon)
      {
        ScreenManager.AudioManager.PlayCannonRotateSound();
      }

      cameraPitch = 0.0f;
      doCameraMove = true;

      ////camera.UpdateCamera(camera.Position, 0.025f, 0.0f);
    }

    /// <summary>
    /// Event handler for reacting on the Right Action.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnRightUp(object sender, EventArgs e)
    {
      doCameraMove = false;
      ////camera.UpdateCamera(camera.Position, 0.025f, 0.0f);
      ScreenManager.AudioManager.StopCannonRotateSound();
    }

    /// <summary>
    /// Event handler for reacting on the FFLeft Action.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnFfLeftDown(object sender, EventArgs e)
    {
      if (camera.State == CameraState.CastleView)
      {
        cameraYaw = -0.05f;
      }
      else
      {
        cameraYaw = 0.05f;
      }

      if (camera.State == CameraState.Cannon)
      {
        ScreenManager.AudioManager.PlayCannonRotateSound();
      }

      cameraPitch = 0.0f;
      doCameraMove = true;
      ////camera.UpdateCamera(camera.Position, -0.05f, 0.0f);
    }

    /// <summary>
    /// Event handler for reacting on the FFLeft Action.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnFfLeftUp(object sender, EventArgs e)
    {
      doCameraMove = false;
      ScreenManager.AudioManager.StopCannonRotateSound();
    }

    /// <summary>
    /// Event handler for reacting on the Left Action.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnLeftDown(object sender, EventArgs e)
    {
      if (camera.State == CameraState.CastleView)
      {
        cameraYaw = -0.025f;
      }
      else
      {
        cameraYaw = 0.025f;
      }

      if (camera.State == CameraState.Cannon)
      {
        ScreenManager.AudioManager.PlayCannonRotateSound();
      }

      cameraPitch = 0.0f;
      doCameraMove = true;
    }

    /// <summary>
    /// Event handler for reacting on the Left Action.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnLeftUp(object sender, EventArgs e)
    {
      doCameraMove = false;
      ScreenManager.AudioManager.StopCannonRotateSound();
    }

    /// <summary>
    /// Event handler for reacting on the Up Up Action.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnUpUp(object sender, EventArgs e)
    {
      doCameraMove = false;
      ////camera.UpdateCamera(camera.Position, 0.0f, -VertikalRotationSpeed);
    }

    /// <summary>
    /// Event handler for reacting on the Up Down Action.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnUpDown(object sender, EventArgs e)
    {
      cameraYaw = -0.0f;
      if (camera.State == CameraState.CastleView)
      {
        cameraPitch = -VertikalRotationSpeed;
      }
      else
      {
        cameraPitch = -VertikalRotationSpeed;
      }

      doCameraMove = true;
    }

    /// <summary>
    /// Event handler for reacting on the Up Action.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnDownDown(object sender, EventArgs e)
    {
      cameraYaw = -0.0f;

      if (camera.State == CameraState.CastleView)
      {
        cameraPitch = VertikalRotationSpeed;
      }
      else if (camera.State == CameraState.Cannon)
      {
        cameraPitch = VertikalRotationSpeed;
      }
      else
      {
        cameraPitch = VertikalRotationSpeed;
      }

      doCameraMove = true;
    }

    /// <summary>
    /// Performs the on down up.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnDownUp(object sender, EventArgs e)
    {
      doCameraMove = false;
      ////camera.UpdateCamera(camera.Position, 0.0f, VertikalRotationSpeed);
    }

    /// <summary>
    /// TODO: Store viewing directions and positions
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnCastleViewUp(object sender, EventArgs e)
    {
      lock (this)
      {
        SetCastleCameraButtonStateAndView();
      }
    }

    private void SetCastleCameraButtonStateAndView()
    {
      ScreenManager.PlayerSettings[activePlayer].CurrentCameraState = CameraState.CastleView;

      btnSelectCannon.Visible = false;
      btnSelectCannon.Enabled = false;
      btnSwitchTower.Visible = false;
      btnSwitchTower.Enabled = false;
      btnSelectTower.Visible = true;
      btnSelectTower.Enabled = true;
      btnSelectCannon.Visible = false;
      btnSelectCannon.Enabled = false;
      btnSelectCastle.Visible = false;
      btnSelectCastle.Enabled = false;
      btnLessPowder.Enabled = false;
      btnLessPowder.Visible = false;
      btnMorePowder.Enabled = false;
      btnMorePowder.Visible = false;
      btnFire.Enabled = false;
      btnFire.Visible = false;
      btnSwitchIron.Enabled = false;
      btnSwitchIron.Visible = false;
      btnSwitchStone.Enabled = false;
      btnSwitchStone.Visible = false;

      ////if (camera.State != CameraState.CastleView)
      ////{

      var trans = Matrix.Invert(Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));

      // we came from towerview, store current towers stuff
      if (camera.State == CameraState.Tower)
      {
        ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.CameraPosition = Vector3.Transform(camera.Position, trans);
        ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.ViewDirection = camera.ViewDirection;
      }

      if (camera.State == CameraState.Cannon)
      {
        ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.CameraPosition = Vector3.Transform(camera.Position, trans);
        ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.ViewDirection = Vector3.Transform(camera.ViewDirection, trans);
      }

      camera.State = CameraState.CastleView;

      if (ScreenManager.GameSettings.AnimatedCamera)
      {
        Vector3 tempTrans = Vector3.Transform(
            ScreenManager.PlayerSettings[activePlayer].Castle.CameraPosition,
            Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));

        DisableInput();

        camera.InitBezier(
            camera.Position,
            camera.Position + camera.ViewDirection,
            tempTrans,
            tempTrans + ScreenManager.PlayerSettings[activePlayer].Castle.StartPos,
            CameraState.CastleView);
      }

      camera.Position = Vector3.Transform(
          ScreenManager.PlayerSettings[activePlayer].Castle.CameraPosition,
          Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));

      camera.ViewDirection = ScreenManager.PlayerSettings[activePlayer].Castle.StartPos;

      // Force drawing with new positions
      camera.UpdateCamera(camera.Position, 0.0f, 0.0f);
      ////}
    }

    private void SetTowerCameraButtonStatesAndView()
    {
      ScreenManager.PlayerSettings[activePlayer].CurrentCameraState = CameraState.Tower;

      btnSelectCastle.Visible = true;
      btnSelectCastle.Enabled = true;
      btnSelectTower.Visible = false;
      btnSelectTower.Enabled = false;
      btnSwitchTower.Visible = true;
      btnSwitchTower.Enabled = true;
      btnLessPowder.Enabled = false;
      btnLessPowder.Visible = false;
      btnMorePowder.Enabled = false;
      btnMorePowder.Visible = false;
      btnFire.Enabled = false;
      btnFire.Visible = false;
      btnSwitchIron.Enabled = false;
      btnSwitchIron.Visible = false;
      btnSwitchStone.Enabled = false;
      btnSwitchStone.Visible = false;

      if (ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.HasCannon)
      {
        btnSelectCannon.Visible = true;
        btnSelectCannon.Enabled = true;
      }

      Matrix trans = Matrix.Invert(Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));

      if (camera.State == CameraState.CastleView)
      {
        ScreenManager.PlayerSettings[activePlayer].Castle.CameraPosition = Vector3.Transform(camera.Position, trans);
      }
      else if (camera.State == CameraState.Cannon)
      {
        ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.CameraPosition = Vector3.Transform(camera.Position, trans);
        ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.ViewDirection = Vector3.Transform(camera.ViewDirection, trans);
      }

      camera.State = CameraState.Tower;

      if (ScreenManager.GameSettings.AnimatedCamera)
      {
        Vector3 tempTrans = Vector3.Transform(
               ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.CameraPosition,
               Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));

        DisableInput();

        camera.InitBezier(
            camera.Position,
            camera.Position + camera.ViewDirection,
            tempTrans,
            tempTrans + ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.ViewDirection,
            CameraState.Tower);
      }

      camera.Position = Vector3.Transform(
          ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.CameraPosition,
          Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));

      /*camera.ViewDirection = new Vector3(
      //    castle.CurrentTower.X,
      //    10.0f,//castle.CurrentTower. TODO: use tower height here
      //    castle.CurrentTower.Y);*/
      camera.ViewDirection = ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.ViewDirection;

      // Force drawing with new positions
      camera.UpdateCamera(camera.Position, 0.0f, 0.0f);
    }

    /// <summary>
    /// Performs the on tower view up.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnTowerViewUp(object sender, EventArgs e)
    {
      lock (this)
      {
        if (camera.State != CameraState.Tower)
        {
          SetTowerCameraButtonStatesAndView();
        }
      }
    }

    /// <summary>
    /// Performs the on tower switch up.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnTowerSwitchUp(object sender, EventArgs e)
    {
      var trans = Matrix.Invert(Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));

      ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.CameraPosition = Vector3.Transform(camera.Position, trans);
      ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.ViewDirection = camera.ViewDirection;
      ScreenManager.PlayerSettings[activePlayer].Castle.SwitchTower();

      if (ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.HasCannon)
      {
        btnSelectCannon.Visible = true;
        btnSelectCannon.Enabled = true;
      }
      else
      {
        btnSelectCannon.Visible = false;
        btnSelectCannon.Enabled = false;
      }

      if (ScreenManager.GameSettings.AnimatedCamera)
      {
        var tempTrans = Vector3.Transform(
               ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.CameraPosition,
               Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));

        DisableInput();

        camera.InitBezier(
            camera.Position,
            camera.Position + camera.ViewDirection,
            tempTrans,
            tempTrans + ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.ViewDirection,
            CameraState.Tower);
      }

      camera.Position = Vector3.Transform(
          ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.CameraPosition,
          Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));

      camera.ViewDirection = ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.ViewDirection;

      // Force drawing with new positions
      camera.UpdateCamera(camera.Position, 0.0f, 0.0f);
    }

    /// <summary>
    /// Performs the on cannon view up.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnCannonViewUp(object sender, EventArgs e)
    {
      if (camera.State != CameraState.Animated)
      {
        SetCannonCameraButtonStatesAndView();
      }
    }

    private void SetCannonCameraButtonStatesAndView()
    {
      ScreenManager.PlayerSettings[activePlayer].CurrentCameraState = CameraState.Cannon;

      var trans = Matrix.Invert(
          Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));

      ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.CameraPosition =
          Vector3.Transform(camera.Position, trans);
      ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.ViewDirection =
          camera.ViewDirection;

      btnSelectCastle.Visible = true;
      btnSelectCastle.Enabled = true;
      btnSelectTower.Visible = true;
      btnSelectTower.Enabled = true;
      btnSwitchTower.Visible = false;
      btnSwitchTower.Enabled = false;
      btnSelectCannon.Visible = false;
      btnSelectCannon.Enabled = false;
      btnLessPowder.Enabled = true;
      btnLessPowder.Visible = true;
      btnMorePowder.Enabled = true;
      btnMorePowder.Visible = true;
      btnFire.Enabled = true;
      btnFire.Visible = true;

      if (ScreenManager.PlayerSettings[activePlayer].CannonballType == CannonballType.Stone)
      {
        btnSwitchIron.Enabled = true;
        btnSwitchIron.Visible = true;
        btnSwitchStone.Enabled = false;
        btnSwitchStone.Visible = false;
      }
      else
      {
        btnSwitchIron.Enabled = false;
        btnSwitchIron.Visible = false;
        btnSwitchStone.Enabled = true;
        btnSwitchStone.Visible = true;
      }

      if (ScreenManager.GameSettings.AnimatedCamera)
      {
        var tempTrans = Vector3.Transform(
            ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.CameraPosition,
            Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));

        var tempView = Vector3.Transform(
            ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.ViewDirection,
            Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));

        DisableInput();

        camera.InitBezier(
            camera.Position,
            camera.Position + camera.ViewDirection,
            tempTrans,
            tempView,
            CameraState.Cannon);
      }

      camera.TowerCenter = Vector3.Transform(
          new Vector3(
              ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.X,
              ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.Height,
              ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.Y),
          Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));

      camera.Position = Vector3.Transform(
          ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.CameraPosition,
          Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));

      camera.ViewDirection = Vector3.Transform(
          ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.ViewDirection,
          Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));

      camera.Yaw =
          ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.RotationAngleYaw;

      camera.Pitch =
          ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.RotationAnglePitch;

      camera.UpdateCamera(camera.Position, 0.0f, 0.0f);
    }

    /// <summary>
    /// Performs the on player switch down.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnPlayerSwitchDown(object sender, EventArgs e)
    {
      cannonFired = false;

      Calculation();

      // Save current players camera state
      ScreenManager.PlayerSettings[activePlayer].CurrentCameraState = camera.State;

      Matrix trans = Matrix.Invert(Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));

      if (camera.State == CameraState.CastleView)
      {
        ScreenManager.PlayerSettings[activePlayer].Castle.CameraPosition = Vector3.Transform(camera.Position, trans);
      }

      if (camera.State == CameraState.Tower)
      {
        ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.CameraPosition = Vector3.Transform(camera.Position, trans);
        ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.ViewDirection = camera.ViewDirection;
      }

      if (camera.State == CameraState.Cannon)
      {
        ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.CameraPosition = Vector3.Transform(camera.Position, trans);
        ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.ViewDirection = Vector3.Transform(camera.ViewDirection, trans);
      }

      // Switch the player
      activePlayer = activePlayer + 1;

      if (activePlayer == ScreenManager.GameSettings.NumPlayers)
      {
        activePlayer = 0;
      }

      camera.State = ScreenManager.PlayerSettings[activePlayer].CurrentCameraState;

      if (camera.State == CameraState.CastleView)
      {
        SetCastleCameraButtonStateAndView();
        /*
        SetCastleCameraButtonStateAndView();
        camera.Position = Vector3.Transform(
            BallerburgGame.PlayerSettings[activePlayer].Castle.CameraPosition,
            Matrix.CreateTranslation(BallerburgGame.PlayerSettings[activePlayer].Castle.StartPos));

        camera.ViewDirection = BallerburgGame.PlayerSettings[activePlayer].Castle.StartPos;

        // Force drawing with new positions
        camera.UpdateCamera(camera.Position, 0.0f, 0.0f);
         * */
      }
      else if (camera.State == CameraState.Tower)
      {
        SetTowerCameraButtonStatesAndView();
      }
      else if (camera.State == CameraState.Cannon)
      {
        // Cannon has not been destroyed in other players turn? If yes
        // set the tower view
        if (ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.HasCannon)
        {
          SetCannonCameraButtonStatesAndView();
        }
        else
        {
          ScreenManager.PlayerSettings[activePlayer].CurrentCameraState = CameraState.Tower;
          SetTowerCameraButtonStatesAndView();
        }
      }
    }

    /// <summary>
    /// Performs the on mouse leave.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnMouseLeave(object sender, EventArgs e)
    {
      lblHoverText.Text = string.Empty;
    }

    /// <summary>
    /// Performs the on fire down.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnFireDown(object sender, EventArgs e)
    {
      if (ScreenManager.GameSettings.GameStyle == GameType.Klassisch && !cannonFired)
      {
        if ((ScreenManager.PlayerSettings[activePlayer].CannonballType == CannonballType.Iron) && (ScreenManager.PlayerSettings[activePlayer].Eisenkugeln == 0))
        {
          lblInfoText.Text = "Zuwenig Eisenkugeln";
          showTextCounter = 0;
          return;
        }

        if ((ScreenManager.PlayerSettings[activePlayer].CannonballType == CannonballType.Stone) && (ScreenManager.PlayerSettings[activePlayer].Steinkugel == 0))
        {
          lblInfoText.Text = "Zuwenig Steinkugeln";
          showTextCounter = 0;
          return;
        }

        if (ScreenManager.PlayerSettings[activePlayer].Powder < ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.PowderLoad)
        {
          lblInfoText.Text = "Zuwenig Pulver";
          showTextCounter = 0;
          return;
        }

        if (((ScreenManager.PlayerSettings[activePlayer].CannonballType == CannonballType.Iron) && (ScreenManager.PlayerSettings[activePlayer].Eisenkugeln > 0)) || ((ScreenManager.PlayerSettings[activePlayer].CannonballType == CannonballType.Stone) && (ScreenManager.PlayerSettings[activePlayer].Steinkugel > 0)))
        {
          // Subtract 1 cannonball
          if (ScreenManager.PlayerSettings[activePlayer].CannonballType == CannonballType.Iron)
          {
            ScreenManager.PlayerSettings[activePlayer].Eisenkugeln -= 1;
          }

          if (ScreenManager.PlayerSettings[activePlayer].CannonballType == CannonballType.Stone)
          {
            ScreenManager.PlayerSettings[activePlayer].Steinkugel -= 1;
          }

          // Subtract Powder
          ScreenManager.PlayerSettings[activePlayer].Powder -= ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.PowderLoad;

          foreach (
              var cannonBall in
                  ScreenManager.GameObjectManager.ActiveCannonballs.Where(cannonBall => !cannonBall.Alive))
          {
            ScreenManager.AudioManager.PlayFireSound();
            cannonBall.VelocityDirection =
                Vector3.Normalize(
                    ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.TubeDirection);
            cannonBall.Position = Vector3.Transform(
                ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.BulletStartPosition,
                Matrix.CreateTranslation(ScreenManager.PlayerSettings[activePlayer].Castle.StartPos));
            cannonBall.Alive = true;
            break;
          }

          cannonFired = true;
        }
      }
    }

    /// <summary>
    /// Performs the on dollar down.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnMoneyDown(object sender, EventArgs e)
    {
      ScreenState = ScreenState.Hidden;
      ScreenManager.AddScreen(new EinstellungenScreen(GraphicsManager, ScreenManager, activePlayer));
    }

    /// <summary>
    /// Performs the on powder plus.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnPowderPlus(object sender, EventArgs e)
    {
      if (ScreenManager.PlayerSettings[activePlayer].Powder < (ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.PowderLoad + PowderPlusMinusAmount))
      {
        lblInfoText.Text = "Zuwenig Pulver";
        showTextCounter = 0;
      }
      else
      {
        btnMorePowder.Enabled = true;
        btnMorePowder.Visible = true;
        ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.PowderLoad += PowderPlusMinusAmount;
        lblInfoText.Text = "Pulver: " + ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.PowderLoad + "/1000";
        showTextCounter = 0;

        if (ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.PowderLoad >= MaxPowderAmount)
        {
          btnMorePowder.Enabled = false;
          btnMorePowder.Visible = false;
        }

        if (ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.PowderLoad > MinPowderAmount)
        {
          btnLessPowder.Enabled = true;
          btnLessPowder.Visible = true;
        }
      }
    }

    /// <summary>
    /// Performs the on powder minus.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnPowderMinus(object sender, EventArgs e)
    {
      ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.PowderLoad -= PowderPlusMinusAmount;
      lblInfoText.Text = "Pulver: " + ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.PowderLoad + "/1000";
      showTextCounter = 0;

      if (ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.PowderLoad <= MinPowderAmount)
      {
        btnLessPowder.Enabled = false;
        btnLessPowder.Visible = false;
      }

      if (ScreenManager.PlayerSettings[activePlayer].Powder >= ScreenManager.PlayerSettings[activePlayer].Castle.CurrentTower.TowerCannon.PowderLoad)
      {
        btnMorePowder.Enabled = true;
        btnMorePowder.Visible = true;
      }
    }

    private void PerformOnCannonBallSwitch(object sender, EventArgs e)
    {
      if (ScreenManager.PlayerSettings[activePlayer].CannonballType == CannonballType.Iron)
      {
        ScreenManager.PlayerSettings[activePlayer].CannonballType = CannonballType.Stone;
        btnSwitchIron.Enabled = true;
        btnSwitchIron.Visible = true;
        btnSwitchStone.Enabled = false;
        btnSwitchStone.Visible = false;
      }
      else
      {
        ScreenManager.PlayerSettings[activePlayer].CannonballType = CannonballType.Iron;
        btnSwitchIron.Enabled = false;
        btnSwitchIron.Visible = false;
        btnSwitchStone.Enabled = true;
        btnSwitchStone.Visible = true;
      }
    }

    private void PerformOnActivate(object sender, EventArgs e)
    {
      if (ScreenManager.PlayerSettings[activePlayer].CurrentCameraState == CameraState.Tower)
      {
        SetTowerCameraButtonStatesAndView();
      }
    }

    #endregion

    #region Gamelogic

    /// <summary>
    /// Update Money, Population, Prices
    /// </summary>
    public void Calculation()
    {
      var j = ScreenManager.PlayerSettings[activePlayer].Verzinsung;
      short[] pmi = new short[6] { 98, 347, 302, 102, 30, 29 };   /* Preisgrenzen */
      short[] pma = new short[6] { 302, 707, 498, 200, 89, 91 };
      short[] psp = new short[6] { 10, 50, 50, 20, 10, 10 };     /* max. Preisschwankung */

      Random rnd = new Random();
      ScreenManager.PlayerSettings[this.activePlayer].Money +=
          (ScreenManager.PlayerSettings[this.activePlayer].Population * (j > 65 ? 130 - j : j)) / (150 - rnd.Next(32767) % 50);

      ScreenManager.PlayerSettings[this.activePlayer].Population = ((ScreenManager.PlayerSettings[this.activePlayer].Population * (95 + rnd.Next(32767) % 11)) / 100) + (((21 - j + rnd.Next(32767) % 9) * (8 + rnd.Next(32767) % 5)) / 20);

      if (ScreenManager.PlayerSettings[this.activePlayer].Population < 0)
      {
        ScreenManager.PlayerSettings[this.activePlayer].Population = 0;
        ////end=n+49;
      }

      // Original Ballerburg Code: Türme hinzurechnen
      ////for ( j=0;j<5;j++ ) ge[n]+=(40+rand()%31)*(ft[n][j].x>-1);

      // Original Ballerburg Code: Preise für die nächste Runde anpassen

      ////for ( j=0;j<6;j++ )
      ////{
      ////    p[j]+=psp[j]*(rand()%99)/98-psp[j]/2;
      ////    p[j]=Max(p[j],pmi[j]);
      ////    p[j]=Min(p[j],pma[j]);
      ////}
    }

    /// <summary>
    /// Init the game - logically
    /// </summary>
    public void InitNewGame()
    {
      // Init the game - sound
      ScreenManager.AudioManager.PlayGameBackgroundMusic();

      var rnd = new Random();

      activePlayer = 0;

      // TODO: Money Value must be initialized when player is created
      ////BallerburgGame.PlayerSettings[this.activePlayer].Money = 5000;
      ////BallerburgGame.PlayerSettings[this.activePlayer].Population = 500;

      for (var i = 0; i < 4; i++)
      {
        if (ScreenManager.PlayerSettings[i].IsActive)
        {
          ScreenManager.PlayerSettings[i].Money = 5000;
          ScreenManager.PlayerSettings[i].Population = 500;
          ScreenManager.PlayerSettings[i].Verzinsung = 10;
          ScreenManager.PlayerSettings[i].Eisenkugeln = 5;
          ScreenManager.PlayerSettings[i].Steinkugel = 10;
          ScreenManager.PlayerSettings[i].Powder = 5000;
          ScreenManager.PlayerSettings[i].CannonballType = CannonballType.Stone;

          foreach (var tower in ScreenManager.PlayerSettings[i].Castle.Towers)
          {
            if (tower.HasCannon)
            {
              tower.TowerCannon.PowderLoad = 500;
            }
          }
        }
      }

      SetCastleCameraButtonStateAndView();

      // Reset all objects
      foreach (var cannonBall in ScreenManager.GameObjectManager.ActiveCannonballs)
      {
        if (cannonBall.Alive)
        {
          cannonBall.Alive = false;
        }
      }

      ScreenManager.GameObjectManager.ExplosionBillboard.Pause();
      ScreenManager.GameObjectManager.ExplosionBillboard.Reset();

      ScreenManager.GameSettings.AnimatedCamera = true;
    }

    #endregion

    // A simple helper to draw shadowed text.
    private void DrawString(SpriteFont font, string text, Vector2 position, Color color)
    {
      ScreenManager.SpriteBatch.DrawString(font, text, new Vector2(position.X + 1, position.Y + 1), Color.Black);
      ScreenManager.SpriteBatch.DrawString(font, text, position, color);
    }

    // A simple helper to draw shadowed text.
    private void DrawString(SpriteFont font, string text, Vector2 position, Color color, float fontScale)
    {
      ScreenManager.SpriteBatch.DrawString(
          font,
          text,
          new Vector2(position.X + 1, position.Y + 1),
          Color.Black,
          0,
          new Vector2(0, font.LineSpacing / 2),
          fontScale,
          SpriteEffects.None,
          0);
      ScreenManager.SpriteBatch.DrawString(
          font,
          text,
          position,
          color,
          0,
          new Vector2(0, font.LineSpacing / 2),
          fontScale,
          SpriteEffects.None,
          0);
    }

    #region IDisposable

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        renderTarget.Dispose();
        renderTarget = null;
        hud.Dispose();
        hud = null;
      }
    }

    #endregion
  }
}
