// <copyright file="Terrain.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Graphic
{
  using System;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  using MonoGameBallerburg.Manager;

  /// <summary>
  /// Class used to represent ballerburgs terrain.
  /// </summary>
  public class Terrain : IDisposable
  {
    private readonly Texture2D texture = null;

    private VertexBuffer vertexBuffer = null;

    // Vertices generieren
    private VertexPositionNormalTexture[] vertices;

    /// <summary>
    /// Initializes a new instance of the <see cref="Terrain"/> class.
    /// </summary>
    /// <param name="contentManager">The content manager.</param>
    /// <param name="graphicsDevice">The graphics device.</param>
    public Terrain(IContentManager contentManager, IBallerburgGraphicsManager graphicsDevice)
    {
      this.texture = contentManager.TerrainTexture;
      this.InitTerrainGraphics(graphicsDevice);
    }

    /// <summary>
    /// Draws the specified game time.
    /// </summary>
    /// <param name="viewMatrix">The view matrix.</param>
    /// <param name="projectionMatrix">The projection matrix.</param>
    /// <param name="shaderEffect">The shader effect.</param>
    /// <param name="graphicsDevice">The graphics device.</param>
    public void Draw(Matrix viewMatrix, Matrix projectionMatrix, Effect shaderEffect, GraphicsDevice graphicsDevice)
    {
      // Kopie der Sichtmatrix erstellen und die Translation
      // unberücksichtigt lassen, da der Himmel unendlich erscheinen soll
      // unabhängig davon, wo wir uns im Level befinden
      Matrix view = viewMatrix;
      ////oView.Translation = Vector3.Zero;

      // Graphics Device zum Rendern der Primitive vorbereiten
      ////Game1.Instance.GraphicsDevice.VertexDeclaration = this.vertexDeclaration;
      graphicsDevice.SetVertexBuffer(vertexBuffer);

      // Tiefentest deaktivieren und Schreibschutz des Z-Buffers aktivieren
      // Beim Rendern der Sky Box sollen keine Tiefenwerte in den Z-Buffer geschrieben
      // werden, denn der Himmel liegt immer hinter allen anderen Objekten
      ////Game1.Instance.GraphicsDevice.DepthStencilState.DepthBufferEnable = true;
      ////Game1.Instance.GraphicsDevice.DepthStencilState.DepthBufferWriteEnable = true;

      graphicsDevice.DepthStencilState = DepthStencilState.Default;

      ////shaderEffect.CurrentTechnique = shaderEffect.Techniques["Simplest"];
      shaderEffect.Parameters["xWorldViewProjection"].SetValue(Matrix.Identity * viewMatrix * projectionMatrix);
      shaderEffect.Parameters["xWorld"].SetValue(Matrix.Identity);
      shaderEffect.Parameters["xTexture"].SetValue(this.texture);

      // Render States setzen
      graphicsDevice.RasterizerState = RasterizerState.CullNone;
      ////Game1.Instance.GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Mirror;
      ////Game1.Instance.GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Mirror;
      ////effect.Begin();

      SamplerState samplerState = graphicsDevice.SamplerStates[0];
      graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

      foreach (var pass in shaderEffect.CurrentTechnique.Passes)
      {
        pass.Apply();
        graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
      }

      ////effect.End();

      ////m_oEffect.Texture = m_oLeftTexture;
      ////this.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 24, 2);

      ////m_oEffect.Texture = m_oRightTexture;
      ////this.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 36, 2);

      ////m_oEffect.Texture = m_oTopTexture;
      ////this.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 48, 2);

      ////m_oEffect.CurrentTechnique.Passes[0].End();

      // Tiefentest wieder aktivieren und Schreibschutz entfernen
      ////Game1.Instance.GraphicsDevice.DepthStencilState.DepthBufferEnable = true;
      ////Game1.Instance.GraphicsDevice.DepthStencilState.DepthBufferWriteEnable = true;
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
        this.vertexBuffer.Dispose();
        this.vertexBuffer = null;
      }
    }

    #endregion

    /// <summary>
    /// Inits the terrain graphics.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device.</param>
    private void InitTerrainGraphics(IBallerburgGraphicsManager graphicsDevice)
    {
      this.vertices = new VertexPositionNormalTexture[6];

      // Untere Seite (Kamera schaut entlang der negativen Y-Achse)
      this.vertices[0].Position = new Vector3(-1000.0f, 0.0f, -1000.0f);
      this.vertices[0].TextureCoordinate = new Vector2(0.0f, 100.0f);
      this.vertices[0].Normal = new Vector3(0.0f, 1.0f, 0.0f);

      this.vertices[1].Position = new Vector3(-1000.0f, 0.0f, 1000.0f);
      this.vertices[1].TextureCoordinate = new Vector2(0.0f, 0.0f);
      this.vertices[1].Normal = new Vector3(0.0f, 1.0f, 0.0f);

      this.vertices[2].Position = new Vector3(1000.0f, 0.0f, -1000.0f);
      this.vertices[2].TextureCoordinate = new Vector2(100.0f, 100.0f);
      this.vertices[2].Normal = new Vector3(0.0f, 1.0f, 0.0f);

      this.vertices[3].Position = new Vector3(-1000.0f, 0.0f, 1000.0f);
      this.vertices[3].TextureCoordinate = new Vector2(0.0f, 0.0f);
      this.vertices[3].Normal = new Vector3(0.0f, 1.0f, 0.0f);

      this.vertices[4].Position = new Vector3(1000.0f, 0.0f, 1000.0f);
      this.vertices[4].TextureCoordinate = new Vector2(100.0f, 0.0f);
      this.vertices[4].Normal = new Vector3(0.0f, 1.0f, 0.0f);

      this.vertices[5].Position = new Vector3(1000.0f, 0.0f, -1000.0f);
      this.vertices[5].TextureCoordinate = new Vector2(100.0f, 100.0f);
      this.vertices[5].Normal = new Vector3(0.0f, 1.0f, 0.0f);

      this.vertexBuffer = new VertexBuffer(graphicsDevice.GraphicsDevice, typeof(VertexPositionNormalTexture), this.vertices.Length, BufferUsage.None);
      this.vertexBuffer.SetData(this.vertices);

      ////this.texture = BallerburgGame.ContentManager.TerrainTexture;
    }
  }
}
