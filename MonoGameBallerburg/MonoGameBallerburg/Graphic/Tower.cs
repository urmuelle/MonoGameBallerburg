// <copyright file="Tower.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Graphic
{
  using System;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Graphics;

  using MonoGameBallerburg.Manager;

  /// <summary>
  /// Class to draw and hold the state of Ballerburg - towers.
  /// </summary>
  public class Tower : IDisposable
  {
    // Position and Transform
    private readonly int x;
    private readonly int y;
    private readonly float height;
    private readonly int corners;
    private readonly float radius;
    private readonly float coverRadius;
    private readonly float coverHeight;
    private readonly bool hasCannon;

    /// <summary>
    /// The effect shader used for drawing
    /// </summary>
    private readonly Texture2D wallTexture;
    private readonly Texture2D coverTexture;

    // each Tower has a tower view.
    // The player can rotate arround the tower, this rotation must be
    // stored.
    private int id;

    // Information stored for the camera
    private Vector3 viewDirection;
    private Vector3 cameraPosition;

    // Cannoninformation
    private Cannon cannon;
    private bool drawCannon;

    /// <summary>
    /// Vertex and Index buffers used for drawing
    /// </summary>
    private VertexPositionNormalTexture[] geometry;
    private int numVertices;
    private short[] indices;
    private short[] coverIndices;
    private VertexBuffer vb;
    private IndexBuffer ib;
    private IndexBuffer coverIb;

    /// <summary>
    /// Initializes a new instance of the Tower class.
    /// </summary>
    /// <param name="reader">The Content Reader for Castle XML Files</param>
    public Tower(ContentReader reader)
    {
      id = reader.ReadInt32();
      hasCannon = reader.ReadBoolean();
      var position = reader.ReadObject<Vector2>();
      x = (int)position.X;
      y = (int)position.Y;
      height = reader.ReadSingle();
      corners = reader.ReadInt32();
      radius = reader.ReadSingle();
      coverRadius = reader.ReadSingle();
      coverHeight = reader.ReadSingle();
      viewDirection = new Vector3(x, coverHeight + 2, y + 1);

      wallTexture = reader.ReadExternalReference<Texture2D>();
      coverTexture = reader.ReadExternalReference<Texture2D>();

      ////InitTowerGraphics();

      if (HasCannon)
      {
        hasCannon = true;
        cannon = new Cannon(X, coverHeight, Y, null);
      }
    }

    /// <summary>
    /// Gets the X.
    /// </summary>
    public int X
    {
      get { return x; }
    }

    /// <summary>
    /// Gets the Y.
    /// </summary>
    public int Y
    {
      get { return y; }
    }

    /// <summary>
    /// Gets the height.
    /// </summary>
    public float Height
    {
      get { return height; }
    }

    /// <summary>
    /// Gets the height of the cover.
    /// </summary>
    /// <value>
    /// The height of the cover.
    /// </value>
    public float CoverHeight
    {
      get { return coverHeight; }
    }

    /// <summary>
    /// Gets the ID.
    /// </summary>
    public int ID
    {
      get { return id; }
    }

    /// <summary>
    /// Gets or sets the tower's viewing direction
    /// </summary>
    public Vector3 ViewDirection
    {
      get { return viewDirection; }
      set { viewDirection = value; }
    }

    /// <summary>
    /// Gets or sets the tower's camera position
    /// </summary>
    public Vector3 CameraPosition
    {
      get { return cameraPosition; }
      set { cameraPosition = value; }
    }

    /// <summary>
    /// Gets a value indicating whether this instance has cannon.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has cannon; otherwise, <c>false</c>.
    /// </value>
    public bool HasCannon
    {
      get { return hasCannon; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance has cannon.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has cannon; otherwise, <c>false</c>.
    /// </value>
    public bool DrawCannon
    {
      get { return drawCannon; }
      set { drawCannon = value; }
    }

    /// <summary>
    /// Gets or sets the tower cannon.
    /// </summary>
    /// <value>
    /// The tower cannon.
    /// </value>
    public Cannon TowerCannon
    {
      get { return cannon; }
      set { cannon = value; }
    }

    /// <summary>
    /// Draw the tower.
    /// </summary>
    /// <param name="world">The world.</param>
    /// <param name="viewMatrix">The Viewmatrix</param>
    /// <param name="projectionMatrix">The Projectoin Matrix</param>
    /// <param name="lightView">The light view.</param>
    /// <param name="shaderEffect">The shader effect.</param>
    /// <param name="contentManager">The content manager.</param>
    /// <param name="graphicsDevice">The graphics device.</param>
    public void Draw(Matrix world, Matrix viewMatrix, Matrix projectionMatrix, Matrix lightView, Effect shaderEffect, IContentManager contentManager, GraphicsDevice graphicsDevice)
    {
      int width = corners;

      int primitivePerStrip = width * 2;
      int vertexPerStrip = (width * 2) + 2;

      Matrix worldTranslation = Matrix.CreateTranslation(x, 0.0f, y) * world;

      shaderEffect.Parameters["xWorldViewProjection"].SetValue(worldTranslation * viewMatrix * projectionMatrix);
      shaderEffect.Parameters["xTexture"].SetValue(coverTexture);
      shaderEffect.Parameters["xWorld"].SetValue(worldTranslation);
      shaderEffect.Parameters["xLightsWorldViewProjection"].SetValue(worldTranslation * lightView);

      graphicsDevice.RasterizerState = RasterizerState.CullNone;
      graphicsDevice.BlendState = BlendState.Opaque;
      graphicsDevice.DepthStencilState = DepthStencilState.Default;

      foreach (EffectPass pass in shaderEffect.CurrentTechnique.Passes)
      {
        graphicsDevice.SetVertexBuffer(vb);
        graphicsDevice.Indices = coverIb;

        pass.Apply();

        graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 3 * (corners - 2), 0, corners - 2);
        graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, numVertices, 0, corners - 2);
      }

      shaderEffect.Parameters["xTexture"].SetValue(wallTexture);

      for (int s = 0; s < 3; ++s)
      {
        foreach (EffectPass pass in shaderEffect.CurrentTechnique.Passes)
        {
          graphicsDevice.SetVertexBuffer(vb);
          graphicsDevice.Indices = ib;

          pass.Apply();
          graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, numVertices, vertexPerStrip * s, primitivePerStrip);
        }
      }

      if (drawCannon)
      {
        cannon.Draw(world, viewMatrix, projectionMatrix, shaderEffect, contentManager, graphicsDevice);
      }
    }

    /// <summary>
    /// Draws the default.
    /// </summary>
    /// <param name="world">The world.</param>
    /// <param name="viewMatrix">The view matrix.</param>
    /// <param name="projectionMatrix">The projection matrix.</param>
    /// <param name="lightView">The light view.</param>
    /// <param name="shaderEffect">The shader effect.</param>
    /// <param name="contentManager">The content manager.</param>
    /// <param name="graphicsDevice">The graphics device.</param>
    public void DrawDefault(Matrix world, Matrix viewMatrix, Matrix projectionMatrix, Matrix lightView, Effect shaderEffect, IContentManager contentManager, GraphicsDevice graphicsDevice)
    {
      int width = corners;

      int primitivePerStrip = width * 2;
      int vertexPerStrip = (width * 2) + 2;

      Matrix worldTranslation = Matrix.CreateTranslation(x, 0.0f, y) * world;

      shaderEffect.Parameters["xWorldViewProjection"].SetValue(worldTranslation * viewMatrix * projectionMatrix);
      shaderEffect.Parameters["xTexture"].SetValue(coverTexture);
      shaderEffect.Parameters["xWorld"].SetValue(worldTranslation);
      shaderEffect.Parameters["xLightsWorldViewProjection"].SetValue(worldTranslation * lightView);

      graphicsDevice.RasterizerState = RasterizerState.CullNone;
      graphicsDevice.BlendState = BlendState.Opaque;
      graphicsDevice.DepthStencilState = DepthStencilState.Default;

      foreach (EffectPass pass in shaderEffect.CurrentTechnique.Passes)
      {
        graphicsDevice.SetVertexBuffer(vb);
        graphicsDevice.Indices = coverIb;

        pass.Apply();

        graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 3 * (corners - 2), 0, corners - 2);
        graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, numVertices, 0, corners - 2);
      }

      shaderEffect.Parameters["xTexture"].SetValue(wallTexture);

      for (int s = 0; s < 3; ++s)
      {
        foreach (EffectPass pass in shaderEffect.CurrentTechnique.Passes)
        {
          graphicsDevice.SetVertexBuffer(vb);
          graphicsDevice.Indices = ib;

          pass.Apply();
          graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, numVertices, vertexPerStrip * s, primitivePerStrip);
        }
      }

      if (hasCannon)
      {
        cannon.Draw(world, viewMatrix, projectionMatrix, shaderEffect, contentManager, graphicsDevice);
      }
    }

    /// <summary>
    /// Initialize all objects needed for drawing the tower, call the geometry setup procedures
    /// </summary>
    /// <param name="graphicsDevice">The graphics device.</param>
    public void InitGraphics(GraphicsDevice graphicsDevice)
    {
      Initialize();

      // Initialize all parts necessary for rendering            
      vb = new VertexBuffer(graphicsDevice, typeof(VertexPositionNormalTexture), geometry.Length, BufferUsage.WriteOnly);
      vb.SetData(geometry);
      BuildIndexBuffer();
      ib = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, sizeof(short) * indices.Length, BufferUsage.WriteOnly);
      coverIb = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, sizeof(short) * coverIndices.Length, BufferUsage.WriteOnly);
      ib.SetData(indices);
      coverIb.SetData(coverIndices);

      if (HasCannon)
      {
        cannon.InitGraphics(graphicsDevice);
      }
    }

    /// <summary>
    /// Initialize the tower's geometry
    /// </summary>
    private void Initialize()
    {
      int numCorners = 5 * corners;
      float zinnenHeight = 1.0f;

      numVertices = numCorners + 4;
      geometry = new VertexPositionNormalTexture[numCorners + 4];
      Vector3 normal = new Vector3(0.0f, 0.0f, -1.0f);

      // Calculate bottom vertices
      float angle = (2.0f * (float)Math.PI) / corners;

      float dheight = (coverHeight - height) / (coverHeight + zinnenHeight);
      for (int i = 0; i < corners; i++)
      {
        geometry[i] = new VertexPositionNormalTexture(
            new Vector3(radius * (float)Math.Cos(i * angle), 0.0f, radius * (float)Math.Sin(i * angle)),
            normal,
            new Vector2(i * (1.0f / corners), 1.0f));
        geometry[i + (corners + 1)] = new VertexPositionNormalTexture(
            new Vector3(radius * (float)Math.Cos(i * angle), height, radius * (float)Math.Sin(i * angle)),
            normal,
            new Vector2(i * (1.0f / corners), 0.3333f));
        geometry[i + (2 * (corners + 1))] = new VertexPositionNormalTexture(
            new Vector3(coverRadius * (float)Math.Cos(i * angle), coverHeight, coverRadius * (float)Math.Sin(i * angle)),
            normal,
            new Vector2(i * (1.0f / corners), 0.3333f - dheight));
        geometry[i + (3 * (corners + 1))] = new VertexPositionNormalTexture(
            new Vector3(coverRadius * (float)Math.Cos(i * angle), coverHeight + zinnenHeight, coverRadius * (float)Math.Sin(i * angle)),
            normal,
            new Vector2(i * (1.0f / corners), 0.0f));

        // Turmdach
        geometry[i + (4 * (corners + 1))] = new VertexPositionNormalTexture(
            new Vector3(coverRadius * (float)Math.Cos(i * angle), coverHeight, coverRadius * (float)Math.Sin(i * angle)),
            normal,
            new Vector2((float)(Math.Cos(i * angle) + 1) / 2.0f, (float)(Math.Sin(i * angle) + 1) / 2.0f));
      }

      geometry[corners] = new VertexPositionNormalTexture(
          new Vector3(radius * (float)Math.Cos(0 * angle), 0.0f, radius * (float)Math.Sin(0 * angle)),
          normal,
          new Vector2(1.0f, 1.0f));
      geometry[(2 * corners) + 1] = new VertexPositionNormalTexture(
          new Vector3(radius * (float)Math.Cos(0 * angle), height, radius * (float)Math.Sin(0 * angle)),
          normal,
          new Vector2(1, 0.3333f));
      geometry[(3 * corners) + 2] = new VertexPositionNormalTexture(
          new Vector3(coverRadius * (float)Math.Cos(0 * angle), coverHeight, coverRadius * (float)Math.Sin(0 * angle)),
          normal,
          new Vector2(1, 0.3333f - dheight));
      geometry[(4 * corners) + 3] = new VertexPositionNormalTexture(
          new Vector3(coverRadius * (float)Math.Cos(0 * angle), coverHeight + zinnenHeight, coverRadius * (float)Math.Sin(0 * angle)),
          normal,
          new Vector2(1.0f, 0.0f));
    }

    /// <summary>
    /// Build the indexbuffer used for indexing the vertices to draw the tower
    /// </summary>
    private void BuildIndexBuffer()
    {
      int width = corners;
      int depth = 4;

      int stripLength = (width + 1) * 2;
      int stripCount = depth - 1;

      indices = new short[stripLength * stripCount];

      int index = 0;

      for (int s = 0; s < stripCount; ++s)
      {
        for (int z = 0; z < width + 1; ++z)
        {
          indices[index] = (short)(z + ((width + 1) * s));

          ++index;

          indices[index] = (short)(z + ((width + 1) * s) + (width + 1));

          ++index;
        }
      }

      coverIndices = new short[(corners - 2) * 3];
      short cornercount = 0;
      index = 0;

      // Turmdach
      for (int i = 0; i < corners - 2; i++)
      {
        coverIndices[index] = (short)(4 * (corners + 1));
        ++index;
        coverIndices[index] = (short)((4 * (corners + 1)) + cornercount + 1);
        ++index;
        coverIndices[index] = (short)((4 * (corners + 1)) + cornercount + 2);
        ++index;
        ++cornercount;
      }
    }

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
        vb.Dispose();
        vb = null;
        ib.Dispose();
        ib = null;

        if (cannon != null)
        {
          cannon.Dispose();
          cannon = null;
        }

        coverIb.Dispose();
        coverIb = null;
      }
    }
  }
}
