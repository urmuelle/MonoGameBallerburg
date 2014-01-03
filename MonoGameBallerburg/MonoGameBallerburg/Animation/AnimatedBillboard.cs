// <copyright file="AnimatedBillboard.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Animation
{
  using System;
  using System.Collections.Generic;
  using Graphic;
  using Manager;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  /// <summary>
  /// Class used to represent an animated billboard
  /// </summary>
  public class AnimatedBillboard : IDisposable
  {
    private readonly List<AnimationFrame> frames;
    private readonly Vector3 direction = new Vector3(0, 0, -1);
    private readonly bool doLoop;
    private readonly IContentManager contentManager;
    private Matrix worldTransform;
    private VertexBuffer vertexBuffer;
    private Vector3 position = Vector3.Zero;
    private int framecount;
    private int frame;
    private float totalElapsed;
    private bool paused;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimatedBillboard"/> class.
    /// </summary>
    /// <param name="loop">if set to <c>true</c> [loop].</param>
    /// <param name="contentManager">The content manager.</param>
    public AnimatedBillboard(bool loop, IContentManager contentManager)
    {
      Camera = null;
      framecount = 0;
      doLoop = loop;
      frames = new List<AnimationFrame>();
      Position = new Vector3(0.0f, 0.0f, 0.0f);
      this.contentManager = contentManager;
    }

    /// <summary>
    /// Gets or sets the camera.
    /// </summary>
    /// <value>
    /// The camera.
    /// </value>
    public Camera Camera { get; set; }

    /// <summary>
    /// Gets or sets the position.
    /// </summary>
    /// <value>
    /// The position.
    /// </value>
    public Vector3 Position
    {
      get { return position; }
      set { position = value; }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is paused.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is paused; otherwise, <c>false</c>.
    /// </value>
    public bool IsPaused
    {
      get { return paused; }
    }

    /// <summary>
    /// Called when graphics resources need to be loaded. Override this method to load any component-specific graphics resources.
    /// </summary>
    public void LoadContent()
    {
      // Load Graphics
      AddFrame(contentManager.ExplosionFrame01, 0.05f);
      AddFrame(contentManager.ExplosionFrame02, 0.05f);
      AddFrame(contentManager.ExplosionFrame03, 0.05f);
      AddFrame(contentManager.ExplosionFrame04, 0.05f);
      AddFrame(contentManager.ExplosionFrame05, 0.05f);
      AddFrame(contentManager.ExplosionFrame06, 0.05f);
      AddFrame(contentManager.ExplosionFrame07, 0.05f);
      AddFrame(contentManager.ExplosionFrame08, 0.05f);

      framecount = 8;
    }

    /// <summary>
    /// Adds the frame.
    /// </summary>
    /// <param name="texture">The texture.</param>
    /// <param name="time">The time passed.</param>
    public void AddFrame(Texture2D texture, float time)
    {
      framecount = framecount + 1;
      frames.Add(new AnimationFrame(texture, time));
    }

    /// <summary>
    /// Allows the game component to update itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public void Update(GameTime gameTime)
    {
      if (Camera == null)
      {
        return;
      }

      // Transformation berechnen
      var translationMatrix = Matrix.CreateTranslation(position);

      // Richtungsvektor zur Kamera berechnen
      var cameraToBillboard = Camera.Position - position;
      cameraToBillboard.Y = 0;
      cameraToBillboard.Normalize();

      // Der Vektor zeigt nach oben bzw. nach unten, und beeinflusst damit die 
      // Richtung der Drehung. Er wird quasi als "Drehachse" verwendet.
      var upVector = Vector3.Cross(direction, cameraToBillboard);
      upVector.Normalize();

      // Winkel berechnen
      var angle = (float)Math.Acos(Vector3.Dot(direction, cameraToBillboard));
      var transform = Matrix.Multiply(Matrix.CreateFromAxisAngle(upVector, angle), translationMatrix);

      worldTransform = transform;

      // Verwenden Sie folgende Zuweisung, um die Hilfsmethode des
      // XNA Frameworks anstelle des obigen Quellcodes zu verwenden
      ////oTransform = Matrix.CreateBillboard(this.Position, this.Camera.Position, this.Camera.UpVector, this.Camera.Direction);
      UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    /// <summary>
    /// Updates the frame.
    /// </summary>
    /// <param name="elapsed">The elapsed.</param>
    public void UpdateFrame(float elapsed)
    {
      if (paused)
      {
        return;
      }

      totalElapsed += elapsed;
      if (totalElapsed > frames[frame].Time)
      {
        totalElapsed -= frames[frame].Time;
        frame++;
        if (doLoop)
        {
          // Keep the Frame between 0 and the total frames, minus one.
          frame = frame % framecount;
        }
        else
        {
          if (frame == framecount)
          {
            Pause();
            Reset();
          }
        }
      }
    }

    /// <summary>
    /// Draws the specified game time.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    /// <param name="effect">The effect.</param>
    /// <param name="graphicsDevice">The graphics device.</param>
    public void Draw(GameTime gameTime, BasicEffect effect, GraphicsDevice graphicsDevice)
    {
      // Dem Device das Vertex Format bekannt machen
      ////this.Game.GraphicsDevice.VertexDeclaration = m_oVertexDeclaration;

      // Vertex-Quelle angeben
      ////this.Game.GraphicsDevice.Vertices[0].SetSource(
      ////    m_oVertexBuffer, 0, VertexPositionTexture.SizeInBytes);
      graphicsDevice.SetVertexBuffer(vertexBuffer);
      ////Game1.Instance.GraphicsDevice.Indices = this.coverIb;

      ////m_oEffect.Texture = m_oTexture;
      effect.Texture = frames[frame].Texture;
      effect.World = worldTransform;
      effect.View = Camera.ViewMatrix;
      effect.Projection = Camera.ProjMatrix;

      ////m_oEffect.Begin();

      /*
      this.Game.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.Linear;
      this.Game.GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Linear;
      this.Game.GraphicsDevice.SamplerStates[0].MipFilter = TextureFilter.Linear;
      */

      ////this.Game.GraphicsDevice.SamplerStates[0].Filter = TextureFilter.Linear;

      graphicsDevice.BlendState = BlendState.AlphaBlend;
      graphicsDevice.DepthStencilState = DepthStencilState.Default;
      graphicsDevice.RasterizerState = RasterizerState.CullNone;
      graphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

      effect.CurrentTechnique.Passes[0].Apply();
      graphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
      ////m_oEffect.CurrentTechnique.Passes[0].End();
      ////m_oEffect.End();
      ////Debug.WriteLine("Frame: " + Frame.ToString());
    }

    /// <summary>
    /// Prepares the animation.
    /// </summary>
    public void PrepareAnimation()
    {
      frame = 0;
      totalElapsed = 0;
      paused = false;
    }

    /// <summary>
    /// Resets this instance.
    /// </summary>
    public void Reset()
    {
      frame = 0;
      totalElapsed = 0f;
    }

    /// <summary>
    /// Stops this instance.
    /// </summary>
    public void Stop()
    {
      Pause();
      Reset();
    }

    /// <summary>
    /// Plays this instance.
    /// </summary>
    public void Play()
    {
      paused = false;
    }

    /// <summary>
    /// Pauses this instance.
    /// </summary>
    public void Pause()
    {
      paused = true;
    }

    /// <summary>
    /// Inits the graphics.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device.</param>
    public void InitGraphics(GraphicsDevice graphicsDevice)
    {
      this.CreateVertexData(graphicsDevice);
    }

    /// <summary>
    /// Creates the vertex data.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device.</param>
    protected virtual void CreateVertexData(GraphicsDevice graphicsDevice)
    {
      // Vertex Buffer initialisieren
      vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), 4, BufferUsage.None);

      // Vertex-Daten definieren
      var verts = new VertexPositionTexture[4];
      verts[0].Position = new Vector3(-2, 0, 0);
      verts[0].TextureCoordinate = new Vector2(0, 1);

      verts[1].Position = new Vector3(-2, 4, 0);
      verts[1].TextureCoordinate = new Vector2(0, 0);

      verts[2].Position = new Vector3(2, 0, 0);
      verts[2].TextureCoordinate = new Vector2(1, 1);

      verts[3].Position = new Vector3(2, 4, 0);
      verts[3].TextureCoordinate = new Vector2(1, 0);

      // Vertex-Daten in den Buffer schreiben
      vertexBuffer.SetData(verts);
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
        vertexBuffer.Dispose();
      }
    }

    #endregion
  }
}