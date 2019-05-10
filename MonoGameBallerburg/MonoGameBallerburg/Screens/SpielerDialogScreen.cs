// <copyright file="SpielerDialogScreen.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Screens
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;
  using MonoGameBallerburg.Controls;
  using MonoGameBallerburg.Gameplay;
  using MonoGameBallerburg.Graphic;
  using MonoGameBallerburg.Manager;

  /// <summary>
  /// The dialog to configure player settings.
  /// </summary>
  public class SpielerDialogScreen : MenuScreen, IDisposable
  {
    private const float CastleCameraStartPosX = 0.0f;
    private const float CastleCameraStartPosY = 25.0f;
    private const float CastleCameraStartPosZ = 50.0f;
    private const int NumCastles = 8;

    private MenuEntry zurueckMenuEntry;
    private ComboToggleButton computerMenuEntry;
    private MenuEntry nameLabel;
    private TextBox txtCastleName;
    private GameplayMenuItem btnNextCastle;
    private GameplayMenuItem btnPreviousCastle;

    private PlayerSettings playerSettings;
    private float castleYaw;
    private int castleType;

    // Camera
    private Camera camera;

    // Drawing
    private Texture2D castleTexture;
    private RenderTarget2D renderTarget;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpielerDialogScreen"/> class.
    /// </summary>
    /// <param name="graphicsManager">The GraphicsManager.</param>
    /// <param name="screenManager">The screen manager.</param>
    /// <param name="spielerNr">The spieler nr.</param>
    /// <param name="playerSettings">The player settings.</param>
    public SpielerDialogScreen(IBallerburgGraphicsManager graphicsManager, IScreenManager screenManager, int spielerNr, PlayerSettings playerSettings)
      : base(graphicsManager, screenManager, "Spieler" + spielerNr.ToString() + " Dialog")
    {
      this.playerSettings = playerSettings;

      // Zurück button
      zurueckMenuEntry = new MenuEntry(this, "Zurück", 0) { Position = new Vector2(500, 450) };
      zurueckMenuEntry.Selected += ZurueckMenuEntrySelected;

      castleType = screenManager.PlayerSettings[spielerNr - 1].Castle.CastleType;

      var selectedEntry = 0;

      if (screenManager.PlayerSettings[spielerNr - 1].PlayerType == PlayerType.Computer)
      {
        selectedEntry = 1;
      }

      var entries = new List<string> { "Aus", "An" };

      computerMenuEntry = new ComboToggleButton(this, "Computer", new Collection<string>(entries), selectedEntry, 0)
                              {
                                Position = new Vector2(10, 100),
                              };
      computerMenuEntry.Selected += ComputerMenuEntrySelected;

      txtCastleName = new TextBox(this, false) { Position = new Vector2(260, 270), ShowCursor = false };

      nameLabel = new MenuEntry(this, "Name", 0) { Position = new Vector2(10, 200) };
      nameLabel.Selected += NameMenuEntrySelected;

      ControlsContainer.Add(zurueckMenuEntry);
      ControlsContainer.Add(computerMenuEntry);
      ControlsContainer.Add(nameLabel);
      ControlsContainer.Add(txtCastleName);

      var pp = GraphicsManager.GraphicsDevice.PresentationParameters;
      renderTarget = new RenderTarget2D(GraphicsManager.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
    }

    /// <summary>
    /// Load graphics content for the screen.
    /// </summary>
    public override void LoadContent()
    {
      base.LoadContent();

      castleYaw = 0.0f;
      camera = new Camera(GraphicsManager);
      camera.UpdateCamera(
          new Vector3(
              ScreenManager.GameObjectManager.Castles[castleType].Center.X + CastleCameraStartPosX,
              ScreenManager.GameObjectManager.Castles[castleType].Center.Y + CastleCameraStartPosY,
              ScreenManager.GameObjectManager.Castles[castleType].Center.Z + CastleCameraStartPosZ),
          0.0f,
          0.0f);

      btnNextCastle = new GameplayMenuItem(this, new Rectangle(32, 1, 32, 32), new Rectangle(580, 280, 32, 32), 2);
      btnPreviousCastle = new GameplayMenuItem(this, new Rectangle(1, 1, 32, 32), new Rectangle(240, 280, 32, 32), 8);

      ControlsContainer.Add(btnNextCastle);
      ControlsContainer.Add(btnPreviousCastle);

      btnNextCastle.MouseDown += PerformOnBtnNextCastleDown;
      btnPreviousCastle.MouseDown += PerformOnBtnPreviousCastleDown;
    }

    /// <summary>
    /// Draws the menu.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public override void Draw(GameTime gameTime)
    {
      /*
      Viewport viewport = BallerburgGame.Instance.GraphicsDevice.Viewport;
      Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
      byte fade = TransitionAlpha;

      this.ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);

      this.ScreenManager.SpriteBatch.Draw(this.backgroundTexture, fullscreen, new Color(fade, fade, fade));

      this.ScreenManager.SpriteBatch.End();
      */

      // Draw this texture to screen
      base.Draw(gameTime);

      DrawCastleTextureToScreen();
    }

    /// <summary>
    /// Draws the castle texture.
    /// </summary>
    /// <param name="shaderManager">The shader manager.</param>
    private void DrawCastleTexture(IShaderManager shaderManager)
    {
      var effect = shaderManager.TheEffect;
      effect.CurrentTechnique = effect.Techniques["Simplest"];
      effect.Parameters["xAmbientIntensity"].SetValue(1.0f);
      effect.Parameters["xAmbientColor"].SetValue(new Vector3(1.0f, 1.0f, 1.0f));

      GraphicsManager.GraphicsDevice.SetRenderTarget(renderTarget);

      GraphicsManager.GraphicsDevice.Clear(Color.Transparent);

      var view = Matrix.CreateLookAt(new Vector3(0f, 8f, 20f), new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 0f));
      var projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1f, 5f, 100f);

      // Draw the castle to the rendertarget
      ScreenManager.GameObjectManager.Castles[castleType].DrawDefault(
          Matrix.Identity,
          Matrix.CreateRotationY(castleYaw) * view,
          projection,
          Matrix.Identity,
          effect,
          ScreenManager.ContentManager,
          GraphicsManager.GraphicsDevice);

      GraphicsManager.GraphicsDevice.SetRenderTarget(null);

      castleTexture = renderTarget;
    }

    /// <summary>
    /// Draws the shadow map to screen.
    /// </summary>
    public void DrawCastleTextureToScreen()
    {
      if (castleTexture != null)
      {
        ////this.ScreenManager.SpriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
        ScreenManager.SpriteBatch.Begin(
            SpriteSortMode.Immediate,
            BlendState.AlphaBlend,
            SamplerState.LinearClamp,
            DepthStencilState.Default,
            RasterizerState.CullNone);
        ScreenManager.SpriteBatch.Draw(castleTexture, new Rectangle(220, 10, 400, 400), Color.White);
        ScreenManager.SpriteBatch.End();
      }

      ////BallerburgGame.Instance.GraphicsDevice.Textures[0] = null;
      ////BallerburgGame.Instance.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
    }

    /// <summary>
    /// Updates the menu.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    /// <param name="otherScreenHasFocus">if set to <c>true</c> [other screen has focus].</param>
    /// <param name="coveredByOtherScreen">if set to <c>true</c> [covered by other screen].</param>
    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

      txtCastleName.Text = ScreenManager.GameObjectManager.Castles[castleType].Name;

      castleYaw += 0.05f;

      DrawCastleTexture(ScreenManager.ShaderManager);
    }

    /// <summary>
    /// When the user presses this button, we go on to the messagebox screen
    /// asking for the gamestyle he wants to play.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void ZurueckMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
      playerSettings.Castle.CastleType = castleType;
      ////MainMenuScreen menuScreen = new MainMenuScreen(this.GraphicsManager);
      ////ScreenManager.AddScreen(menuScreen);
      ExitScreen();
    }

    /// <summary>
    /// Names the menu entry selected.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void NameMenuEntrySelected(object sender, EventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();
      playerSettings.Castle.CastleType = castleType;
      var nameDialogScreen = new EnterNameDialogScreen(GraphicsManager, "Spieler Name", playerSettings);
      ScreenManager.AddScreen(nameDialogScreen);
      ScreenManager.RemoveScreen(this);
    }

    /// <summary>
    /// Set the players type: human or pc.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void ComputerMenuEntrySelected(object sender, ActionToggleButtonEventArgs e)
    {
      ScreenManager.AudioManager.PlayKlickSound();

      playerSettings.PlayerType = (PlayerType)e.SelectedIndex;

      /*
      if (BallerburgGame.PlayerSettings[this.spielerNr - 1].PlayerType == Ballerburg.Gameplay.PlayerType.Human)
      {
          BallerburgGame.PlayerSettings[this.spielerNr - 1].PlayerType = Ballerburg.Gameplay.PlayerType.Computer;
          //this.computerMenuEntry.StateText = "An";
      }
      else
      {
          BallerburgGame.PlayerSettings[this.spielerNr - 1].PlayerType = Ballerburg.Gameplay.PlayerType.Human;
          //this.computerMenuEntry.StateText = "Aus";
      }
       * */
    }

    /// <summary>
    /// Event handler for reacting on the btnNextCastleDown Action.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnBtnNextCastleDown(object sender, EventArgs e)
    {
      if (castleType < NumCastles - 1)
      {
        castleType++;
      }

      txtCastleName.Text = ScreenManager.GameObjectManager.Castles[castleType].Name;
    }

    /// <summary>
    /// Event handler for reacting on the btnPreviousCastleDown Action.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void PerformOnBtnPreviousCastleDown(object sender, EventArgs e)
    {
      if (castleType > 0)
      {
        castleType--;
      }

      txtCastleName.Text = ScreenManager.GameObjectManager.Castles[castleType].Name;
    }

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
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        renderTarget.Dispose();
        renderTarget = null;
      }
    }

    #endregion
  }
}
