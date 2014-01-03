// <copyright file="Cannon.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Graphic
{
  using System;
  using Manager;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  /// <summary>
  /// Class for drawing and holding state information about a cannon
  /// </summary>
  public class Cannon : IDisposable
  {
    #region Constants
    public const float MinElevation = (15.0f / 180.0f) * (float)Math.PI;
    public const float MaxElevation = (90.0f / 180.0f) * (float)Math.PI;
    public const float TubeLenght = 1.0f;
    #endregion

    /// <summary>
    /// Vertex and Index buffers used for drawing
    /// </summary>
    private VertexPositionNormalTexture[] geometry;
    private short[] indices;
    private VertexBuffer vb;
    private IndexBuffer ib;
    private Texture2D cannonTexture;

    /// <summary>
    /// Position and Orientation
    /// </summary>
    private Matrix worldMatrix;

    // Information stored for the camera
    private Vector3 viewDirection;
    private Vector3 cameraPosition;

    // Information stored for the game
    private int powderLoad;

    private Vector3 tubeDirection;
    private Vector3 tubeStartDirection;
    private Vector3 cannonPosition;
    private Vector3 bulletStartPosition;
    private float rotationAngleYaw;
    private float rotationAnglePitch;

    // Bounding Volume
    private BoundingSphere boundingVolumeSphere;

    /// <summary>
    /// Initializes a new instance of the Cannon class.
    /// </summary>
    /// <param name="x">Position on the x-Axis</param>
    /// <param name="y">Position on the y-Axis</param>
    /// <param name="z">Position on the z-Axis</param>
    /// <param name="graphicsDevice">The graphics device.</param>
    public Cannon(float x, float y, float z, IGraphicsDeviceManager graphicsDevice)
    {
      // Initialize Cannon and view values
      worldMatrix = Matrix.CreateTranslation(x, y, z);

      cannonPosition = new Vector3(x, y, z);

      tubeStartDirection = new Vector3(0.0f, 0.0f, 1.0f);
      TubeDirection = this.tubeStartDirection;
      tubeDirection.Normalize();

      rotationAngleYaw = 0.0f;
      rotationAnglePitch = 0.0f;

      bulletStartPosition = (TubeLenght * tubeDirection) + CannonPosition + new Vector3(0.0f, 0.5f, 0.0f);

      // Camera position and view direction for the cannon
      cameraPosition = new Vector3(x, y + 1.5f, z - 1.0f);
      viewDirection = cameraPosition + tubeDirection;

      // Initialize Bounding Volume
      boundingVolumeSphere = new BoundingSphere(new Vector3(0.0f, 0.5f, 0.0f), 0.75f);
    }

    /// <summary>
    /// Gets or sets the rotation angle yaw.
    /// </summary>
    /// <value>
    /// The rotation angle yaw.
    /// </value>
    public float RotationAngleYaw
    {
      get { return this.rotationAngleYaw; }
      set { this.rotationAngleYaw = value; }
    }

    /// <summary>
    /// Gets or sets the rotation angle pitch.
    /// </summary>
    /// <value>
    /// The rotation angle pitch.
    /// </value>
    public float RotationAnglePitch
    {
      get { return this.rotationAnglePitch; }
      set { this.rotationAnglePitch = value; }
    }

    /// <summary>
    /// Gets or sets the view direction.
    /// </summary>
    /// <value>
    /// The view direction.
    /// </value>
    public Vector3 ViewDirection
    {
      get { return this.viewDirection; }
      set { this.viewDirection = value; }
    }

    /// <summary>
    /// Gets or sets the camera position.
    /// </summary>
    /// <value>
    /// The camera position.
    /// </value>
    public Vector3 CameraPosition
    {
      get { return this.cameraPosition; }
      set { this.cameraPosition = value; }
    }

    /// <summary>
    /// Gets or sets the powder load.
    /// </summary>
    /// <value>
    /// The powder load.
    /// </value>
    public int PowderLoad
    {
      get { return this.powderLoad; }
      set { this.powderLoad = value; }
    }

    /// <summary>
    /// Gets or sets the tube direction.
    /// </summary>
    /// <value>
    /// The tube direction.
    /// </value>
    public Vector3 TubeDirection
    {
      get
      {
        return this.tubeDirection;
      }

      set
      {
        this.tubeDirection = value;

        // Set the new bullet start position
        this.bulletStartPosition = (TubeLenght * Vector3.Normalize(this.tubeDirection)) + this.CannonPosition + new Vector3(0.0f, 0.5f, 0.0f);
      }
    }

    /// <summary>
    /// Gets or sets the cannon position.
    /// </summary>
    /// <value>
    /// The cannon position.
    /// </value>
    public Vector3 CannonPosition
    {
      get { return this.cannonPosition; }
      set { this.cannonPosition = value; }
    }

    /// <summary>
    /// Gets the initial Position if a cannonball is shot.
    /// </summary>
    public Vector3 BulletStartPosition
    {
      get { return this.bulletStartPosition; }
    }

    /// <summary>
    /// Gets the bounding sphere absolute position.
    /// </summary>
    public BoundingSphere BoundingSphereAbsolutePosition
    {
      get
      {
        BoundingSphere bs = new BoundingSphere();
        Matrix trans = Matrix.CreateTranslation(this.cannonPosition);
        this.boundingVolumeSphere.Transform(ref trans, out bs);
        return bs;
      }
    }

    /// <summary>
    /// Draw the cannon.
    /// </summary>
    /// <param name="world">The world.</param>
    /// <param name="viewMatrix">The view matrix.</param>
    /// <param name="projectionMatrix">The projection matrix.</param>
    /// <param name="shaderEffect">The shader effect.</param>
    /// <param name="contentManager">The content manager.</param>
    /// <param name="graphicsDevice">The graphics device.</param>
    public void Draw(Matrix world, Matrix viewMatrix, Matrix projectionMatrix, Effect shaderEffect, IContentManager contentManager, GraphicsDevice graphicsDevice)
    {
      ////Game1.Instance.GraphicsDevice.DepthStencilState.DepthBufferEnable = true;
      ////Game1.Instance.GraphicsDevice.DepthStencilState.DepthBufferWriteEnable = true;
      ////Game1.Instance.GraphicsDevice.RasterizerState.FillMode = FillMode.Solid;
      ////Game1.Instance.GraphicsDevice.RasterizerState.CullMode = CullMode.None;
      ////Game1.Instance.GraphicsDevice.VertexDeclaration = this.vertexDeclaration;

      graphicsDevice.RasterizerState = RasterizerState.CullNone;

      this.cannonTexture = contentManager.CannonTexture;

      // Create translation move the tube to the origin
      Matrix toOrigin = Matrix.CreateTranslation(0.0f, -0.5f, 0.0f);

      // Create rotation matrices that rotates the tube to the cannontubedirection
      float alpha = (float)Math.Asin(this.tubeDirection.Y);

      // Limit the angle, in which the cannontube may be raised
      this.rotationAnglePitch = MathHelper.Clamp(this.rotationAnglePitch, -MathHelper.PiOver2, 0.0f);
      Matrix rotateX = Matrix.CreateFromYawPitchRoll(0.0f, this.rotationAnglePitch, 0.0f);
      Matrix rotateY = Matrix.CreateFromYawPitchRoll(this.rotationAngleYaw, 0.0f, 0.0f);

      Matrix worldTranslation = toOrigin * rotateX * rotateY * Matrix.Invert(toOrigin) * Matrix.CreateTranslation(this.cannonPosition.X, this.cannonPosition.Y, this.cannonPosition.Z) * world;

      shaderEffect.Parameters["xWorldViewProjection"].SetValue(worldTranslation * viewMatrix * projectionMatrix);
      shaderEffect.Parameters["xTexture"].SetValue(this.cannonTexture);

      this.TubeDirection = Vector3.Normalize(Vector3.Transform(this.tubeStartDirection, rotateX * rotateY));

      ////Game1.Instance.GraphicsDevice.RenderState.AlphaBlendEnable = false;
      graphicsDevice.BlendState = BlendState.Opaque;

      for (int s = 0; s < 3; ++s)
      {
        // Draw the wheels
        foreach (EffectPass pass in shaderEffect.CurrentTechnique.Passes)
        {
          graphicsDevice.SetVertexBuffer(this.vb);
          graphicsDevice.Indices = this.ib;

          pass.Apply();

          graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 66, 0, 12);
          graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 66, 36, 12);
        }
      }

      for (int s = 0; s < 3; ++s)
      {
        // Draw the Tube
        foreach (EffectPass pass in shaderEffect.CurrentTechnique.Passes)
        {
          ////pass.Begin();

          ////Game1.Instance.GraphicsDevice.Vertices[0].SetSource(this.vb, 0, VertexPositionNormalTexture.SizeInBytes);
          graphicsDevice.SetVertexBuffer(this.vb);
          graphicsDevice.Indices = this.ib;
          pass.Apply();
          graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, 66, 72, 24);
          graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 66, 98, 12);
        }
      }

      ////Game1.Instance.GraphicsDevice.DepthStencilState.DepthBufferEnable = false;
      ////Game1.Instance.GraphicsDevice.DepthStencilState.DepthBufferWriteEnable = false;
    }

    /// <summary>
    /// Inits the graphics.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device.</param>
    public void InitGraphics(GraphicsDevice graphicsDevice)
    {
      // Initialize Geometry
      Initialize();

      // Initialize all parts necessary for rendering
      ////cannonTexture = contentManager.CannonTexture;

      vb = new VertexBuffer(graphicsDevice, typeof(VertexPositionNormalTexture), geometry.Length, BufferUsage.WriteOnly);
      vb.SetData(geometry);
      BuildIndexBuffer();
      ib = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, sizeof(short) * indices.Length, BufferUsage.WriteOnly);
      ib.SetData(indices);
    }

    /// <summary>
    /// Generate the vertices for the wall to be drawn
    /// </summary>
    private void Initialize()
    {
      var normal = new Vector3(0.0f, 0.0f, -1.0f);

      this.geometry = new VertexPositionNormalTexture[66];

      var wheelCenterleft = new Vector3(-0.375f, 0.5f, 0.0f);
      var wheelCenterright = new Vector3(0.375f, 0.5f, 0.0f);

      // Generate the two wheels
      this.geometry[0] = new VertexPositionNormalTexture(wheelCenterleft, normal, new Vector2(0.25f, 0.25f));
      this.geometry[13] = new VertexPositionNormalTexture(wheelCenterright, normal, new Vector2(0.25f, 0.25f));

      double x;
      double y;
      for (int i = 0; i < 12; i++)
      {
        x = Math.Cos(i * 2 * Math.PI / 12);
        y = Math.Sin(i * 2 * Math.PI / 12);

        // Calculate Position Vectors
        var leftPos = new Vector3(-0.375f, (0.5f * (float)y) + 0.5f, 0.5f * (float)x);
        var rightPos = new Vector3(0.375f, (0.5f * (float)y) + 0.5f, 0.5f * (float)x);

        // Calculate Textur Positions
        var tpos = new Vector2(0.5f - (((float)x + 1.0f) / 4.0f), 0.5f - (((float)y + 1.0f) / 4.0f));
        this.geometry[i + 1] = new VertexPositionNormalTexture(leftPos, normal, tpos);
        this.geometry[i + 14] = new VertexPositionNormalTexture(rightPos, normal, tpos);
      }

      // Generate the cannon tube
      for (var i = 0; i < 12; i++)
      {
        x = Math.Cos(i * 2 * Math.PI / 12);
        y = Math.Sin(i * 2 * Math.PI / 12);

        // Calculate Position Vectors
        var leftPos = new Vector3(0.375f * (float)x, 0.5f + (0.375f * (float)y), 0.0f);
        var upperPos = new Vector3(0.375f * (float)x, 0.5f + (0.375f * (float)y), 1.0f);

        // Calculate Textur Positions
        var lowTexturePos = new Vector2(0.5f + ((i * 0.5f) / 12), 0.0f);
        var upperTexturePos = new Vector2(0.5f + ((i * 0.5f) / 12), 0.5f);
        var lowerTextureZpos = new Vector2(1.0f - (((float)x + 1.0f) / 4.0f), 0.5f - (((float)y + 1.0f) / 4.0f));

        this.geometry[i + 26] = new VertexPositionNormalTexture(leftPos, normal, lowTexturePos);
        this.geometry[i + 39] = new VertexPositionNormalTexture(upperPos, normal, upperTexturePos);
        this.geometry[i + 52] = new VertexPositionNormalTexture(leftPos, normal, lowerTextureZpos);
      }

      this.geometry[38] = new VertexPositionNormalTexture(new Vector3(0.375f, 0.5f, 0.0f), normal, new Vector2(1.0f, 0.0f));
      this.geometry[51] = new VertexPositionNormalTexture(new Vector3(0.375f, 0.5f, 1.0f), normal, new Vector2(1.0f, 0.5f));
      this.geometry[65] = new VertexPositionNormalTexture(new Vector3(0.0f, 0.5f, -0.25f), normal, new Vector2(0.75f, 0.25f));
    }

    /// <summary>
    /// Build the indexbuffer used for indexing the vertices to draw the cannon
    /// </summary>
    private void BuildIndexBuffer()
    {
      this.indices = new short[134];

      // Add the indices for the wheels - trianglelist
      for (var i = 0; i < 12; i++)
      {
        this.indices[i * 3] = 0;
        this.indices[(i * 3) + 1] = (short)(i + 1);
        this.indices[(i * 3) + 2] = (short)(i + 2);

        this.indices[(i * 3) + 36] = 13;
        this.indices[(i * 3) + 37] = (short)(i + 1 + 13);
        this.indices[(i * 3) + 38] = (short)(i + 2 + 13);
      }

      this.indices[35] = 1;
      this.indices[71] = 14;

      // Add the indices for the tube - trianglestrip
      var index = 72;

      for (var i = 0; i < 13; ++i)
      {
        this.indices[index] = (short)(i + 26);

        ++index;

        this.indices[index] = (short)(i + 39);

        ++index;
      }

      // Add the indices for the Verschluss - trianglelist
      index = 98;

      for (var i = 0; i < 12; i++)
      {
        this.indices[(i * 3) + index] = 65;
        this.indices[(i * 3) + index + 1] = (short)(i + 52);
        this.indices[(i * 3) + index + 2] = (short)(i + 52 + 1);
      }

      this.indices[133] = 52;
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
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        this.vb.Dispose();
        this.vb = null;
        this.ib.Dispose();
        this.ib = null;
      }
    }

    #endregion
  }
}
