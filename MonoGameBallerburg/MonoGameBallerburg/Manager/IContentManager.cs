// <copyright file="IContentManager.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Manager
{
  using System.Collections.Generic;
  using Graphic;
  using Microsoft.Xna.Framework.Audio;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Media;
  using XnaContentManager = Microsoft.Xna.Framework.Content.ContentManager;

  /// <summary>
  /// Interface for the content manager
  /// </summary>
  public interface IContentManager
  {
    SpriteFont MenuFont { get; }

    Texture2D DebugColorTexture { get; }

    Texture2D BackgroundTexture { get; }

    Texture2D MenuTexture { get; }

    SpriteFont GameFont { get; }

    SkyBox SkyBox { get; }

    Texture2D GradientTexture { get; }

    Texture2D MousePointerTexture { get; }

    Castle Castle1 { get; }

    Castle Castle2 { get; }

    Castle Castle3 { get; }

    Castle Castle4 { get; }

    Castle Castle5 { get; }

    Castle Castle6 { get; }

    Castle Castle7 { get; }

    Castle Castle8 { get; }

    Texture2D ExplosionFrame01 { get; }

    Texture2D ExplosionFrame02 { get; }

    Texture2D ExplosionFrame03 { get; }

    Texture2D ExplosionFrame04 { get; }

    Texture2D ExplosionFrame05 { get; }

    Texture2D ExplosionFrame06 { get; }

    Texture2D ExplosionFrame07 { get; }

    Texture2D ExplosionFrame08 { get; }

    Texture2D CannonTexture { get; }

    Texture2D WallTexture { get; }

    Texture2D CoverTexture { get; }

    Model KugelMesh { get; }

    Texture2D BlankTexture { get; }

    Texture2D WhiteTexture { get; }

    Texture2D TerrainTexture { get; }

    Texture2D CursorTexture { get; }

    Texture2D CannonBallTexture { get; }

    Dictionary<string, Song> BackgroundMusicTracks { get; }

    void LoadContent(XnaContentManager xnaContent, IBallerburgGraphicsManager graphicsManager);

    void UnloadContent();
  }
}
