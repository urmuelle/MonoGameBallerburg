// <copyright file="Wall.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Graphic
{
  using System;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Graphics;

  /// <summary>
  /// Class, for drawing and representing a castle wall
  /// </summary>
  public class Wall : IDisposable
  {
    /// <summary>
    /// The texture Object used for drawing
    /// </summary>
    private readonly Texture2D wallTexture;

    /// <summary>
    /// Vertex and Index buffers used for drawing 
    /// </summary>
    private VertexPositionNormalTexture[] geometry;

    /// <summary>
    /// Indices array
    /// </summary>
    private short[] indices;

    /// <summary>
    /// The VertexBuffer
    /// </summary>
    private VertexBuffer vb;

    /// <summary>
    /// The IndexBuffer
    /// </summary>
    private IndexBuffer ib;

    /// <summary>
    /// From Vector
    /// </summary> 
    private Vector3 startTower;

    /// <summary>
    /// Index to the from tower
    /// </summary> 
    private int fromIndex;

    /// <summary>
    /// The To Vector
    /// </summary> 
    private Vector3 endTower;

    /// <summary>
    /// Index to the to tower
    /// </summary> 
    private int toIndex;

    /// <summary>
    /// The Wall height
    /// </summary> 
    private float height;

    /// <summary>
    /// Initializes a new instance of the Wall class.
    /// Important: SetFromToVectors must be called afterwards!
    /// </summary>
    /// <param name="reader">The Content Type Reader.</param>
    public Wall(ContentReader reader)
    {
      fromIndex = reader.ReadInt32();
      toIndex = reader.ReadInt32();
      height = reader.ReadSingle();
      wallTexture = reader.ReadExternalReference<Texture2D>();
    }

    /// <summary>
    /// Gets the from index
    /// </summary>
    public int FromIndex
    {
      get { return fromIndex; }
    }

    /// <summary>
    /// Gets the to index
    /// </summary>
    public int ToIndex
    {
      get { return toIndex; }
    }

    /// <summary>
    /// Set the from and to vectors (according to the tower's indexes)
    /// </summary>
    /// <param name="from">From vector.</param>
    /// <param name="to">To vector.</param>
    public void SetFromToVectors(Vector3 from, Vector3 to)
    {
      startTower = from;
      endTower = to;
    }

    /// <summary>
    /// Draw the wall.
    /// </summary>
    /// <param name="world">The world.</param>
    /// <param name="viewMatrix">The view matrix</param>
    /// <param name="projectionMatrix">The projection matrix</param>
    /// <param name="lightView">The light view.</param>
    /// <param name="shaderEffect">The shader effect.</param>
    /// <param name="graphicsDevice">The graphics device.</param>
    public void Draw(Matrix world, Matrix viewMatrix, Matrix projectionMatrix, Matrix lightView, Effect shaderEffect, GraphicsDevice graphicsDevice)
    {
      shaderEffect.Parameters["xWorldViewProjection"].SetValue(world * viewMatrix * projectionMatrix);
      shaderEffect.Parameters["xTexture"].SetValue(wallTexture);
      shaderEffect.Parameters["xWorld"].SetValue(world);
      shaderEffect.Parameters["xLightsWorldViewProjection"].SetValue(world * lightView);

      graphicsDevice.RasterizerState = RasterizerState.CullNone;
      graphicsDevice.BlendState = BlendState.Opaque;      

      for (var s = 0; s < 3; ++s)
      {
        foreach (var pass in shaderEffect.CurrentTechnique.Passes)
        {
          pass.Apply();

          graphicsDevice.SetVertexBuffer(vb);
          graphicsDevice.Indices = ib;
          graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, 4, 0, 2);
        }
      }
    }

    /// <summary>
    /// Init Effects, vertex and indexbuffer
    /// </summary>
    /// <param name="graphicsDevice">The graphics device.</param>
    public void InitGraphics(GraphicsDevice graphicsDevice)
    {
      Initialize();

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

      geometry = new VertexPositionNormalTexture[4];
      geometry[0] = new VertexPositionNormalTexture(startTower, normal, new Vector2(0.0f, 1.0f));
      geometry[1] = new VertexPositionNormalTexture(new Vector3(startTower.X, startTower.Y + height, startTower.Z), normal, new Vector2(0.0f, 0.0f));
      geometry[2] = new VertexPositionNormalTexture(endTower, normal, new Vector2(1.0f, 1.0f));
      geometry[3] = new VertexPositionNormalTexture(new Vector3(endTower.X, endTower.Y + height, endTower.Z), normal, new Vector2(1.0f, 0.0f));
    }

    /// <summary>
    /// Build the indexbuffer used for indexing the vertices to draw the tower
    /// </summary>
    private void BuildIndexBuffer()
    {
      indices = new short[4];

      indices[0] = 0;
      indices[1] = 1;
      indices[2] = 2;
      indices[3] = 3;
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
      }
    }
  }
}
