// -----------------------------------------------------------------------
// <copyright file="SkyBoxContentWriter.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
// -----------------------------------------------------------------------
namespace SkyBoxProcessor
{
  using System;
  using Microsoft.Xna.Framework.Content.Pipeline;
  using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
  using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
  using Microsoft.Xna.Framework.Graphics;

  /// <summary>
  /// A class used as writer for a skybox content element
  /// </summary>
  [ContentTypeWriter]
  [CLSCompliant(false)]
  public class SkyBoxContentWriter : ContentTypeWriter<SkyBoxContent>
  {
    /// <summary>
    /// Gets the assembly qualified name of the runtime loader for this type.
    /// </summary>
    /// <param name="targetPlatform">Name of the platform.</param>
    /// <returns>A string with the runtime description</returns>
    public override string GetRuntimeReader(TargetPlatform targetPlatform)
    {
      // Datentyp angeben, der zur Laufzeit diese Daten importiert
      return "MonoGameBallerburg.SkyBoxContentReader, MonoGameBallerburg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
    }

    /// <summary>
    /// Return the runtime type description as a string
    /// </summary>
    /// <param name="targetPlatform">The target platform.</param>
    /// <returns>A string with the runtime description</returns>
    public override string GetRuntimeType(Microsoft.Xna.Framework.Content.Pipeline.TargetPlatform targetPlatform)
    {
      return "MonoGameBallerburg.Graphic.SkyBox, MonoGameBallerburg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
    }

    /// <summary>
    /// Writes the specified output.
    /// </summary>
    /// <param name="output">The output.</param>
    /// <param name="value">The value.</param>
    protected override void Write(ContentWriter output, SkyBoxContent value)
    {
      // Vertices wegschreiben
      output.WriteObject<VertexPositionTexture[]>(value.Vertices);

      // Referenzen auf die Texturen wegschreiben
      output.WriteExternalReference<TextureContent>(value.FrontTexture);
      output.WriteExternalReference<TextureContent>(value.BackTexture);
      output.WriteExternalReference<TextureContent>(value.LeftTexture);
      output.WriteExternalReference<TextureContent>(value.RightTexture);
      output.WriteExternalReference<TextureContent>(value.TopTexture);
      output.WriteExternalReference<TextureContent>(value.BottomTexture);
    }
  }
}
