// -----------------------------------------------------------------------
// <copyright file="SkyBoxProcessor.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
// -----------------------------------------------------------------------
namespace SkyBoxProcessor
{
  using System;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Content.Pipeline;
  using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
  using Microsoft.Xna.Framework.Graphics;

  /// <summary>
  /// The processor for this skybox pipeline object
  /// </summary>
  [ContentProcessor(DisplayName = "Sky Box Processor")]
  [CLSCompliant(false)]
  public class SkyBoxProcessor : ContentProcessor<SkyBoxContent, SkyBoxContent>
  {
    /// <summary>
    /// Processes the specified input.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="context">The context.</param>
    /// <returns>a skyboccontent object</returns>
    public override SkyBoxContent Process(SkyBoxContent input, ContentProcessorContext context)
    {
      var content = input;

      // Vertices generieren
      var vertices = new VertexPositionTexture[36];
      this.CreateVertexData(vertices);
      content.Vertices = vertices;

      var parameters = new OpaqueDataDictionary();
      parameters.Add("GenerateMipmaps", true);

      // Verweise auf Texturen erstellen
      if (!string.IsNullOrEmpty(content.Front))
      {
        content.FrontTexture = context.BuildAsset<TextureContent, TextureContent>(
            new ExternalReference<TextureContent>(content.Front), "TextureProcessor", parameters, null, null);
      }

      if (!string.IsNullOrEmpty(content.Back))
      {
        content.BackTexture = context.BuildAsset<TextureContent, TextureContent>(
            new ExternalReference<TextureContent>(content.Back), "TextureProcessor", parameters, null, null);
      }

      if (!string.IsNullOrEmpty(content.Left))
      {
        content.LeftTexture = context.BuildAsset<TextureContent, TextureContent>(
            new ExternalReference<TextureContent>(content.Left), "TextureProcessor", parameters, null, null);
      }

      if (!string.IsNullOrEmpty(content.Right))
      {
        content.RightTexture = context.BuildAsset<TextureContent, TextureContent>(
            new ExternalReference<TextureContent>(content.Right), "TextureProcessor", parameters, null, null);
      }

      if (!string.IsNullOrEmpty(content.Top))
      {
        content.TopTexture = context.BuildAsset<TextureContent, TextureContent>(
            new ExternalReference<TextureContent>(content.Top), "TextureProcessor", parameters, null, null);
      }

      if (!string.IsNullOrEmpty(content.Bottom))
      {
        content.BottomTexture = context.BuildAsset<TextureContent, TextureContent>(
            new ExternalReference<TextureContent>(content.Bottom), "TextureProcessor", parameters, null, null);
      }

      return content;
    }

    /// <summary>
    /// Definiert die Vertices der sechs Seiten des Quaders "per Hand". Als Primitiven-Typ
    /// wird eine Dreiecksliste fungieren, weshalb pro Seite seches Vertices benötigt werden.
    /// </summary>
    /// <param name="vertices">The vertices.</param>
    protected virtual void CreateVertexData(VertexPositionTexture[] vertices)
    {
      // Vorderseite (wenn die Kamera entlang der negativen Z-Achse schaut)
      vertices[0].Position = new Vector3(-1.0f, -1.0f, -1.0f);
      vertices[0].TextureCoordinate = new Vector2(0.0f, 1.0f);

      vertices[1].Position = new Vector3(-1.0f, 1.0f, -1.0f);
      vertices[1].TextureCoordinate = new Vector2(0.0f, 0.0f);

      vertices[2].Position = new Vector3(1.0f, -1.0f, -1.0f);
      vertices[2].TextureCoordinate = new Vector2(1.0f, 1.0f);

      vertices[3].Position = new Vector3(-1.0f, 1.0f, -1.0f);
      vertices[3].TextureCoordinate = new Vector2(0.0f, 0.0f);

      vertices[4].Position = new Vector3(1.0f, 1.0f, -1.0f);
      vertices[4].TextureCoordinate = new Vector2(1.0f, 0.0f);

      vertices[5].Position = new Vector3(1.0f, -1.0f, -1.0f);
      vertices[5].TextureCoordinate = new Vector2(1.0f, 1.0f);

      // Rückseite (Kamera schaut entlang der positiven Z-Achse)
      vertices[6].Position = new Vector3(1.0f, -1.0f, 1.0f);
      vertices[6].TextureCoordinate = new Vector2(0.0f, 1.0f);

      vertices[7].Position = new Vector3(1.0f, 1.0f, 1.0f);
      vertices[7].TextureCoordinate = new Vector2(0.0f, 0.0f);

      vertices[8].Position = new Vector3(-1.0f, -1.0f, 1.0f);
      vertices[8].TextureCoordinate = new Vector2(1.0f, 1.0f);

      vertices[9].Position = new Vector3(1.0f, 1.0f, 1.0f);
      vertices[9].TextureCoordinate = new Vector2(0.0f, 0.0f);

      vertices[10].Position = new Vector3(-1.0f, 1.0f, 1.0f);
      vertices[10].TextureCoordinate = new Vector2(1.0f, 0.0f);

      vertices[11].Position = new Vector3(-1.0f, -1.0f, 1.0f);
      vertices[11].TextureCoordinate = new Vector2(1.0f, 1.0f);

      // Linke Seite (Kamera schaut entlang der negativen X-Achse)
      vertices[12].Position = new Vector3(-1.0f, -1.0f, 1.0f);
      vertices[12].TextureCoordinate = new Vector2(0.0f, 1.0f);

      vertices[13].Position = new Vector3(-1.0f, 1.0f, 1.0f);
      vertices[13].TextureCoordinate = new Vector2(0.0f, 0.0f);

      vertices[14].Position = new Vector3(-1.0f, -1.0f, -1.0f);
      vertices[14].TextureCoordinate = new Vector2(1.0f, 1.0f);

      vertices[15].Position = new Vector3(-1.0f, 1.0f, 1.0f);
      vertices[15].TextureCoordinate = new Vector2(0.0f, 0.0f);

      vertices[16].Position = new Vector3(-1.0f, 1.0f, -1.0f);
      vertices[16].TextureCoordinate = new Vector2(1.0f, 0.0f);

      vertices[17].Position = new Vector3(-1.0f, -1.0f, -1.0f);
      vertices[17].TextureCoordinate = new Vector2(1.0f, 1.0f);

      // Rechte Seite (Kamera schaut entlang der positiven X-Achse)
      vertices[18].Position = new Vector3(1.0f, -1.0f, -1.0f);
      vertices[18].TextureCoordinate = new Vector2(0.0f, 1.0f);

      vertices[19].Position = new Vector3(1.0f, 1.0f, -1.0f);
      vertices[19].TextureCoordinate = new Vector2(0.0f, 0.0f);

      vertices[20].Position = new Vector3(1.0f, -1.0f, 1.0f);
      vertices[20].TextureCoordinate = new Vector2(1.0f, 1.0f);

      vertices[21].Position = new Vector3(1.0f, 1.0f, -1.0f);
      vertices[21].TextureCoordinate = new Vector2(0.0f, 0.0f);

      vertices[22].Position = new Vector3(1.0f, 1.0f, 1.0f);
      vertices[22].TextureCoordinate = new Vector2(1.0f, 0.0f);

      vertices[23].Position = new Vector3(1.0f, -1.0f, 1.0f);
      vertices[23].TextureCoordinate = new Vector2(1.0f, 1.0f);

      // Obere Seite (Kamera schaut entlang der positiven Y-Achse)
      vertices[24].Position = new Vector3(-1.0f, 1.0f, -1.0f);
      vertices[24].TextureCoordinate = new Vector2(0.0f, 1.0f);

      vertices[25].Position = new Vector3(-1.0f, 1.0f, 1.0f);
      vertices[25].TextureCoordinate = new Vector2(0.0f, 0.0f);

      vertices[26].Position = new Vector3(1.0f, 1.0f, -1.0f);
      vertices[26].TextureCoordinate = new Vector2(1.0f, 1.0f);

      vertices[27].Position = new Vector3(-1.0f, 1.0f, 1.0f);
      vertices[27].TextureCoordinate = new Vector2(0.0f, 0.0f);

      vertices[28].Position = new Vector3(1.0f, 1.0f, 1.0f);
      vertices[28].TextureCoordinate = new Vector2(1.0f, 0.0f);

      vertices[29].Position = new Vector3(1.0f, 1.0f, -1.0f);
      vertices[29].TextureCoordinate = new Vector2(1.0f, 1.0f);

      // Untere Seite (Kamera schaut entlang der negativen Y-Achse)
      vertices[30].Position = new Vector3(-1.0f, -1.0f, -1.0f);
      vertices[30].TextureCoordinate = new Vector2(0.0f, 1.0f);

      vertices[31].Position = new Vector3(-1.0f, -1.0f, 1.0f);
      vertices[31].TextureCoordinate = new Vector2(0.0f, 0.0f);

      vertices[32].Position = new Vector3(1.0f, -1.0f, -1.0f);
      vertices[32].TextureCoordinate = new Vector2(1.0f, 1.0f);

      vertices[33].Position = new Vector3(-1.0f, -1.0f, 1.0f);
      vertices[33].TextureCoordinate = new Vector2(0.0f, 0.0f);

      vertices[34].Position = new Vector3(1.0f, -1.0f, 1.0f);
      vertices[34].TextureCoordinate = new Vector2(1.0f, 0.0f);

      vertices[35].Position = new Vector3(1.0f, -1.0f, -1.0f);
      vertices[35].TextureCoordinate = new Vector2(1.0f, 1.0f);
    }
  }
}