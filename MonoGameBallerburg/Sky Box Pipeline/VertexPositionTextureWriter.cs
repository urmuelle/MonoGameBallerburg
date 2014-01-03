// -----------------------------------------------------------------------
// <copyright file="VertexPositionTextureWriter.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
// -----------------------------------------------------------------------
namespace SkyBoxProcessor
{
  using System;
  using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
  using Microsoft.Xna.Framework.Graphics;

  /// <summary>
  /// A Writer for Vertices with Position and Texture information
  /// </summary>
  [ContentTypeWriter]
  [CLSCompliant(false)]
  public class VertexPositionTextureWriter : ContentTypeWriter<VertexPositionTexture>
  {
    /// <summary>
    /// Gets the assembly qualified name of the runtime loader for this type.
    /// </summary>
    /// <param name="targetPlatform">Name of the platform.</param>
    /// <returns>The runtime type name as a string</returns>
    public override string GetRuntimeReader(Microsoft.Xna.Framework.Content.Pipeline.TargetPlatform targetPlatform)
    {
      return "MonoGameBallerburg.VertexPositionTextureReader, MonoGameBallerburg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
    }

    /// <summary>
    /// Gets the runtime type of this assembly
    /// </summary>
    /// <param name="targetPlatform">The target platform.</param>
    /// <returns>The runtime type name as a string</returns>
    public override string GetRuntimeType(Microsoft.Xna.Framework.Content.Pipeline.TargetPlatform targetPlatform)
    {
      return typeof(VertexPositionTexture).AssemblyQualifiedName;
    }

    /// <summary>
    /// Writes the specified output.
    /// </summary>
    /// <param name="output">The output.</param>
    /// <param name="value">The value.</param>
    protected override void Write(ContentWriter output, VertexPositionTexture value)
    {
      output.Write(value.Position);
      output.Write(value.TextureCoordinate);
    }
  }
}
