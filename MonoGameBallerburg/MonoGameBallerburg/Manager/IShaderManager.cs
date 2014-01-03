// <copyright file="IShaderManager.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Manager
{
  using Microsoft.Xna.Framework.Graphics;
  using XnaContentManager = Microsoft.Xna.Framework.Content.ContentManager;

  /// <summary>
  /// Interface for a manager, dealing with the various shaders
  /// </summary>
  public interface IShaderManager
  {
    /// <summary>
    /// Gets the effect.
    /// </summary>
    Effect TheEffect { get; }

    /// <summary>
    /// Gets the basic effect.
    /// </summary>
    BasicEffect BasicEffect { get; }

    /// <summary>
    /// Loads the content.
    /// </summary>
    /// <param name="xnaContent">Content of the xna.</param>
    /// <param name="graphicsDevice">The graphics device.</param>
    void LoadContent(XnaContentManager xnaContent, IBallerburgGraphicsManager graphicsDevice);

    /// <summary>
    /// Unloads the content.
    /// </summary>
    void UnloadContent();
  }
}
