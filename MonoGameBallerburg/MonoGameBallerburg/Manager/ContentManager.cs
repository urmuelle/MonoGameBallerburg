// <copyright file="ContentManager.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Manager
{
  using Graphic;
  using Microsoft.Xna.Framework.Graphics;
  using XnaContentManager = Microsoft.Xna.Framework.Content.ContentManager;

  /// <summary>
  /// The global ContentManager that manages all content
  /// </summary>
  public class ContentManager : IContentManager
  {
    private XnaContentManager content;

    #region IContentManager Members

    /// <summary>
    /// Gets the menu font.
    /// </summary>
    public SpriteFont MenuFont { get; private set; }

    /// <summary>
    /// Gets the debug color texture.
    /// </summary>
    public Texture2D DebugColorTexture { get; private set; }

    /// <summary>
    /// Gets the texture of the background.
    /// </summary>
    /// <value>
    /// The texture of the background.
    /// </value>
    public Texture2D BackgroundTexture { get; private set; }

    /// <summary>
    /// Gets the menu texture.
    /// </summary>
    public Texture2D MenuTexture { get; private set; }

    /// <summary>
    /// Gets the game font.
    /// </summary>
    public SpriteFont GameFont { get; private set; }

    /// <summary>
    /// Gets the sky box.
    /// </summary>
    public Graphic.SkyBox SkyBox { get; private set; }

    /// <summary>
    /// Gets the gradient texture.
    /// </summary>
    public Texture2D GradientTexture { get; private set; }

    /// <summary>
    /// Gets the mouse pointer texture.
    /// </summary>
    public Texture2D MousePointerTexture { get; private set; }

    /// <summary>
    /// Gets the castle1.
    /// </summary>
    public Castle Castle1 { get; private set; }

    /// <summary>
    /// Gets the castle2.
    /// </summary>
    public Castle Castle2 { get; private set; }

    /// <summary>
    /// Gets the castle3.
    /// </summary>
    public Castle Castle3 { get; private set; }

    /// <summary>
    /// Gets the castle4.
    /// </summary>
    public Castle Castle4 { get; private set; }

    /// <summary>
    /// Gets the castle5.
    /// </summary>
    public Castle Castle5 { get; private set; }

    /// <summary>
    /// Gets the castle6.
    /// </summary>
    public Castle Castle6 { get; private set; }

    /// <summary>
    /// Gets the castle7.
    /// </summary>
    public Castle Castle7 { get; private set; }

    /// <summary>
    /// Gets the castle8.
    /// </summary>
    public Castle Castle8 { get; private set; }

    /// <summary>
    /// Gets the explosion frame01.
    /// </summary>
    public Texture2D ExplosionFrame01 { get; private set; }

    /// <summary>
    /// Gets the explosion frame02.
    /// </summary>
    public Texture2D ExplosionFrame02 { get; private set; }

    /// <summary>
    /// Gets the explosion frame03.
    /// </summary>
    public Texture2D ExplosionFrame03 { get; private set; }

    /// <summary>
    /// Gets the explosion frame04.
    /// </summary>
    public Texture2D ExplosionFrame04 { get; private set; }

    /// <summary>
    /// Gets the explosion frame05.
    /// </summary>
    public Texture2D ExplosionFrame05 { get; private set; }

    /// <summary>
    /// Gets the explosion frame06.
    /// </summary>
    public Texture2D ExplosionFrame06 { get; private set; }

    /// <summary>
    /// Gets the explosion frame07.
    /// </summary>
    public Texture2D ExplosionFrame07 { get; private set; }

    /// <summary>
    /// Gets the explosion frame08.
    /// </summary>
    public Texture2D ExplosionFrame08 { get; private set; }

    /// <summary>
    /// Gets the cannon texture.
    /// </summary>        
    public Texture2D CannonTexture { get; private set; }

    /// <summary>
    /// Gets the wall texture.
    /// </summary>
    public Texture2D WallTexture { get; private set; }

    /// <summary>
    /// Gets the cover texture.
    /// </summary>
    public Texture2D CoverTexture { get; private set; }

    /// <summary>
    /// Gets the kugel mesh.
    /// </summary>
    public Model KugelMesh { get; private set; }

    /// <summary>
    /// Gets the blank texture.
    /// </summary>
    public Texture2D BlankTexture { get; private set; }

    /// <summary>
    /// Gets the white texture.
    /// </summary>
    public Texture2D WhiteTexture { get; private set; }

    /// <summary>
    /// Gets the terrain texture.
    /// </summary>
    public Texture2D TerrainTexture { get; private set; }

    /// <summary>
    /// Gets the cursor texture.
    /// </summary>
    public Texture2D CursorTexture { get; private set; }

    /// <summary>
    /// Gets the cannon ball texture.
    /// </summary>
    public Texture2D CannonBallTexture { get; private set; }

    /// <summary>
    /// Unloads the content.
    /// </summary>
    public void UnloadContent()
    {
      if (null == this.content)
      {
        return;
      }

      this.content.Unload();
    }

    /// <summary>
    /// Loads the content.
    /// </summary>
    /// <param name="xnaContent">Content of the xna.</param>
    /// <param name="graphicsManager">The graphics manager.</param>
    public void LoadContent(XnaContentManager xnaContent, IBallerburgGraphicsManager graphicsManager)
    {
      if (null != this.content)
      {
        return;
      }

      this.content = xnaContent;
      this.content.RootDirectory = "Content";

      this.MenuFont = this.content.Load<SpriteFont>("menufont");
      this.DebugColorTexture = this.content.Load<Texture2D>("solidred");
      this.BackgroundTexture = this.content.Load<Texture2D>("Pic2");
      this.MenuTexture = this.content.Load<Texture2D>("Pic4");
      this.GameFont = this.content.Load<SpriteFont>("gamefont");
      this.SkyBox = this.content.Load<SkyBox>(@"Skybox\SkyBox");
      this.GradientTexture = this.content.Load<Texture2D>("Pic3");
      this.MousePointerTexture = this.content.Load<Texture2D>(@"Arrow3");
      this.Castle1 = this.content.Load<Castle>(@"Castles\Castle1");
      this.Castle1.Initialize(graphicsManager.GraphicsDevice);
      this.Castle2 = this.content.Load<Castle>(@"Castles\Castle2");
      this.Castle2.Initialize(graphicsManager.GraphicsDevice);
      this.Castle3 = this.content.Load<Castle>(@"Castles\Castle3");
      this.Castle3.Initialize(graphicsManager.GraphicsDevice);
      this.Castle4 = this.content.Load<Castle>(@"Castles\Castle4");
      this.Castle4.Initialize(graphicsManager.GraphicsDevice);
      this.Castle5 = this.content.Load<Castle>(@"Castles\Castle5");
      this.Castle5.Initialize(graphicsManager.GraphicsDevice);
      this.Castle6 = this.content.Load<Castle>(@"Castles\Castle6");
      this.Castle6.Initialize(graphicsManager.GraphicsDevice);
      this.Castle7 = this.content.Load<Castle>(@"Castles\Castle7");
      this.Castle7.Initialize(graphicsManager.GraphicsDevice);
      this.Castle8 = this.content.Load<Castle>(@"Castles\Castle8");
      this.Castle8.Initialize(graphicsManager.GraphicsDevice);
      this.ExplosionFrame01 = this.content.Load<Texture2D>(@"Explosion\Explosion_Frame01");
      this.ExplosionFrame02 = this.content.Load<Texture2D>(@"Explosion\Explosion_Frame02");
      this.ExplosionFrame03 = this.content.Load<Texture2D>(@"Explosion\Explosion_Frame03");
      this.ExplosionFrame04 = this.content.Load<Texture2D>(@"Explosion\Explosion_Frame04");
      this.ExplosionFrame05 = this.content.Load<Texture2D>(@"Explosion\Explosion_Frame05");
      this.ExplosionFrame06 = this.content.Load<Texture2D>(@"Explosion\Explosion_Frame06");
      this.ExplosionFrame07 = this.content.Load<Texture2D>(@"Explosion\Explosion_Frame07");
      this.ExplosionFrame08 = this.content.Load<Texture2D>(@"Explosion\Explosion_Frame08");
      this.CannonTexture = this.content.Load<Texture2D>("Tex8");
      this.KugelMesh = this.content.Load<Model>("SphereNew");
      this.BlankTexture = this.content.Load<Texture2D>("blank");
      this.WhiteTexture = this.content.Load<Texture2D>("WhiteRect");
      this.TerrainTexture = this.content.Load<Texture2D>("Tex1");
      this.CursorTexture = this.content.Load<Texture2D>(@"GUI\cursor");
      this.CannonBallTexture = this.content.Load<Texture2D>(@"SphereFinal");
    }

    #endregion
  }
}
