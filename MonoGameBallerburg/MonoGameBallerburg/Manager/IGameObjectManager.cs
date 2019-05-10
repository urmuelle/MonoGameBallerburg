// <copyright file="IGameObjectManager.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Manager
{
  using System.Collections.ObjectModel;
  using Microsoft.Xna.Framework;
  using MonoGameBallerburg.Animation;
  using MonoGameBallerburg.Graphic;

  /// <summary>
  /// Interface for a game object manager.
  /// </summary>
  public interface IGameObjectManager
  {
    /// <summary>
    /// Gets the castles.
    /// </summary>
    Castle[] Castles
    {
      get;
    }

    /// <summary>
    /// Gets the terrain.
    /// </summary>
    Terrain Terrain
    {
      get;
    }

    /// <summary>
    /// Gets the explosion billboard.
    /// </summary>
    AnimatedBillboard ExplosionBillboard
    {
      get;
    }

    /// <summary>
    /// Gets the sky box.
    /// </summary>
    SkyBox SkyBox
    {
      get;
    }

    /// <summary>
    /// Gets the active cannonballs.
    /// </summary>
    Collection<Cannonball> ActiveCannonballs
    {
      get;
    }

    /// <summary>
    /// Loads the content.
    /// </summary>
    void LoadContent();

    /// <summary>
    /// Updates the specified game time.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    void Update(GameTime gameTime);

    /// <summary>
    /// Draws this instance.
    /// </summary>
    void Draw();
  }
}
