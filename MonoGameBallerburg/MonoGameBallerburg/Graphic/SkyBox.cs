// <copyright file="SkyBox.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Graphic
{
  using System;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Graphics;

  public class SkyBox : IDisposable
  {
    #region Variables

    private GraphicsDevice graphicsDevice = null;
    private VertexBuffer vertexBuffer = null;
    private VertexDeclaration vertexDeclaration = null;
    private BasicEffect effect = null;

    private Texture2D frontTexture = null;
    private Texture2D backTexture = null;
    private Texture2D leftTexture = null;
    private Texture2D rightTexture = null;
    private Texture2D topTexture = null;
    private Texture2D bottomTexture = null;

    private int numVertices = 0;
    private int numPrimitives = 0;

    #endregion

    #region Constructor

    public SkyBox(ContentReader input)
    {
      // Graphics Device-Instanz abrufen
      this.GraphicsDevice = ((IGraphicsDeviceService)input.ContentManager.ServiceProvider.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice;

      // Vertices einlesen und Vertex Buffer initialisieren
      VertexPositionTexture[] vertices = input.ReadObject<VertexPositionTexture[]>();
      this.vertexBuffer = new VertexBuffer(this.GraphicsDevice, typeof(VertexPositionTexture), vertices.Length, BufferUsage.None);
      this.vertexBuffer.SetData<VertexPositionTexture>(vertices);

      // Anzahl der Vertices speichern
      this.numVertices = vertices.Length;

      // Division durch 3 ergibt die Anzahl der Primitive, da eine Dreiecksliste verwendet wird
      this.numPrimitives = this.numPrimitives / 3;

      // Vertex-Beschreibung erzeugen
      this.vertexDeclaration = new VertexDeclaration(VertexPositionTexture.VertexDeclaration.GetVertexElements());

      // BasicEffect-Instanz erzeugen
      this.effect = new BasicEffect(this.GraphicsDevice);

      // Texturen einlesen
      this.frontTexture = input.ReadExternalReference<Texture2D>();
      this.backTexture = input.ReadExternalReference<Texture2D>();
      this.leftTexture = input.ReadExternalReference<Texture2D>();
      this.rightTexture = input.ReadExternalReference<Texture2D>();
      this.topTexture = input.ReadExternalReference<Texture2D>();
      this.bottomTexture = input.ReadExternalReference<Texture2D>();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the graphics device.
    /// </summary>
    /// <value>
    /// The graphics device.
    /// </value>
    public GraphicsDevice GraphicsDevice
    {
      get { return this.graphicsDevice; }
      set { this.graphicsDevice = value; }
    }

    #endregion

    #region Methods

    public void Draw(GameTime gameTime, Matrix viewMatrix, Matrix projectionMatrix)
    {
      // Kopie der Sichtmatrix erstellen und die Translation
      // unberücksichtigt lassen, da der Himmel unendlich erscheinen soll
      // unabhängig davon, wo wir uns im Level befinden
      Matrix view = viewMatrix;
      view.Translation = Vector3.Zero;

      // Graphics Device zum Rendern der Primitive vorbereiten
      ////this.GraphicsDevice.VertexDeclaration = m_oVertexDeclaration;
      ////this.GraphicsDevice.Vertices[0].SetSource(m_oVertexBuffer, 0, VertexPositionTexture.SizeInBytes);
      this.GraphicsDevice.SetVertexBuffer(this.vertexBuffer);

      // Tiefentest deaktivieren und Schreibschutz des Z-Buffers aktivieren
      ////Beim Rendern der Sky Box sollen keine Tiefenwerte in den Z-Buffer geschrieben
      ////werden, denn der Himmel liegt immer hinter allen anderen Objekten
      ////this.GraphicsDevice.RenderState.DepthBufferEnable = false;
      ////this.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;
      ////this.GraphicsDevice.DepthStencilState.DepthBufferEnable = false;
      ////this.GraphicsDevice.DepthStencilState.DepthBufferWriteEnable = false;

      this.effect.World = Matrix.Identity;
      this.effect.View = view;
      this.effect.Projection = projectionMatrix;
      this.effect.TextureEnabled = true;

      // Render States setzen
      ////this.GraphicsDevice.RenderState.CullMode = CullMode.None;
      ////this.GraphicsDevice.RasterizerState.CullMode = CullMode.None;
      ////this.GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Mirror;
      ////this.GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Mirror;

      this.effect.Texture = this.frontTexture;
      this.effect.CurrentTechnique.Passes[0].Apply();
      this.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

      this.effect.Texture = this.backTexture;
      this.effect.CurrentTechnique.Passes[0].Apply();
      this.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 6, 2);

      this.effect.Texture = this.leftTexture;
      this.effect.CurrentTechnique.Passes[0].Apply();
      this.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 12, 2);

      this.effect.Texture = this.rightTexture;
      this.effect.CurrentTechnique.Passes[0].Apply();
      this.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 18, 2);

      this.effect.Texture = this.topTexture;
      this.effect.CurrentTechnique.Passes[0].Apply();
      this.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 24, 2);

      this.effect.Texture = this.bottomTexture;
      this.effect.CurrentTechnique.Passes[0].Apply();
      this.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 30, 2);

      ////m_oEffect.Texture = m_oLeftTexture;
      ////this.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 24, 2);

      ////m_oEffect.Texture = m_oRightTexture;
      ////this.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 36, 2);

      ////m_oEffect.Texture = m_oTopTexture;
      ////this.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 48, 2);

      ////m_oEffect.CurrentTechnique.Passes[0].End();

      // Tiefentest wieder aktivieren und Schreibschutz entfernen
      ////this.GraphicsDevice.DepthStencilState.DepthBufferEnable = true;
      ////this.GraphicsDevice.DepthStencilState.DepthBufferEnable = true;
    }

    #endregion

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
        this.vertexBuffer.Dispose();
        this.vertexBuffer = null;
        this.effect.Dispose();
        this.effect = null;
        this.vertexDeclaration.Dispose();
        this.vertexDeclaration = null;
      }
    }
  }
}
