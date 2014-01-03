// <copyright file="Cannonball.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Graphic
{
  using Manager;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  /// <summary>
  /// Class for holding Cannonball Game Objects
  /// </summary>
  public class Cannonball
  {
    private readonly Texture2D cannonBallTexture;

    private readonly Model kugelMesh;

    private Vector3 velocity;

    private Vector3 position;

    private Vector3 initialPosition;

    private bool alive;

    private int totalTime;

    /// <summary>
    /// Initializes a new instance of the Cannonball class.
    /// </summary>
    /// <param name="contentManager">The content manager.</param>
    public Cannonball(IContentManager contentManager)
    {
      this.kugelMesh = contentManager.KugelMesh;
      this.cannonBallTexture = contentManager.CannonBallTexture;
      this.alive = false;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the cannonball is alive
    /// </summary>
    public bool Alive
    {
      get
      {
        return this.alive;
      }

      set
      {
        this.alive = value;

        if (value)
        {
          this.totalTime = 0;
        }
      }
    }

    /// <summary>
    /// Gets or sets the velocity direction.
    /// </summary>
    /// <value>
    /// The velocity direction.
    /// </value>
    public Vector3 VelocityDirection
    {
      get { return this.velocity; }
      set { this.velocity = value; }
    }

    /// <summary>
    /// Gets or sets the position.
    /// </summary>
    /// <value>
    /// The position.
    /// </value>
    public Vector3 Position
    {
      get
      {
        return this.position;
      }

      set
      {
        this.position = value;
        this.initialPosition = value;
      }
    }

    /// <summary>
    /// Gets the bounding volume.
    /// </summary>
    public BoundingSphere BoundingVolume
    {
      get { return this.kugelMesh.Meshes[0].BoundingSphere; }
    }

    /// <summary>
    /// Draws the cannonball.
    /// </summary>
    /// <param name="viewMatrix">The camera's view matrix</param>
    /// <param name="projectionMatrix">The camera's projection matrix</param>
    /// <param name="lightMatrix">The light matrix.</param>
    /// <param name="theEffect">The effect.</param>
    public void Draw(Matrix viewMatrix, Matrix projectionMatrix, Matrix lightMatrix, Effect theEffect)
    {
      var transforms = new Matrix[this.kugelMesh.Bones.Count];
      kugelMesh.CopyAbsoluteBoneTransformsTo(transforms);

      foreach (var mesh in kugelMesh.Meshes)
      {
        /*
            foreach (var meshPart in mesh.MeshParts)
            {
                meshPart.Effect = theEffect.Clone();
            }

            foreach (var effect in mesh.Effects)
            {                   
                var world = transforms[mesh.ParentBone.Index] * Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(position.X, position.Y, position.Z);

                effect.CurrentTechnique = effect.Techniques[theEffect.CurrentTechnique.Name];
                effect.Parameters["xWorldViewProjection"].SetValue(
                    world * viewMatrix * projectionMatrix);
                effect.Parameters["xTexture"].SetValue(cannonBallTexture);
                effect.Parameters["xWorld"].SetValue(world);
                effect.Parameters["xLightPos"].SetValue(theEffect.Parameters["xLightPos"].GetValueVector3());
                effect.Parameters["xLightPower"].SetValue(theEffect.Parameters["xLightPower"].GetValueSingle());
                effect.Parameters["xAmbient"].SetValue(theEffect.Parameters["xAmbient"].GetValueSingle());
                effect.Parameters["xLightsWorldViewProjection"].SetValue(world * lightMatrix);
                effect.Parameters["xShadowMap"].SetValue(theEffect.Parameters["xShadowMap"].GetValueTexture2D());
                    
                /*
                effect.Parameters["xAmbientIntensity"].SetValue(0.5f);
                effect.Parameters["xAmbientColor"].SetValue(new Vector3(1.0f, 1.0f, 1.0f));
                effect.Parameters["xDirectionalLight0Direction"].SetValue(new Vector3(-0.52f, -0.57f, -0.62f));
                effect.Parameters["xDirectionalLight0Color"].SetValue(new Vector3(1f, 0.96f, 0.81f));
                 * */
        /*
          }

          mesh.Draw();
         * */
        foreach (BasicEffect effect in mesh.Effects)
        {
          var world = transforms[mesh.ParentBone.Index] * Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(position.X, position.Y, position.Z);

          effect.EnableDefaultLighting();

          effect.World = world;
          effect.View = viewMatrix;
          effect.Projection = projectionMatrix;
        }

        mesh.Draw();
      }
    }

    /// <summary>
    /// Check for collision and update the Cannonballs position and velocity
    /// </summary>
    /// <param name="gameTime">The Game's GameTime Object holding the elapsed time</param>
    public void Update(GameTime gameTime)
    {
      this.totalTime += gameTime.ElapsedGameTime.Milliseconds;
      this.position = this.initialPosition + ((this.velocity * this.totalTime / 1000.0f) * 20.0f) - new Vector3(0.0f, (9.81f / 2.0f) * ((this.totalTime / 1000.0f) * (this.totalTime / 1000.0f)), 0.0f);
    }
  }
}
