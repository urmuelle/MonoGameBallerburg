// -----------------------------------------------------------------------
// <copyright file="SkyBoxImporter.cs" company="Urs M�ller">
//     Urs M�ller. All rights reserved.
// </copyright>
// <author>Urs M�ller</author>
// -----------------------------------------------------------------------
namespace SkyBoxProcessor
{
  using System;
  using Microsoft.Xna.Framework.Content.Pipeline;
  using TImport = System.String;

  /// <summary>
  /// An importer class for skybox objects
  /// </summary>
  [ContentImporter(".sky", DisplayName = "Sky Box Importer", DefaultProcessor = "SkyBoxProcessor")]
  [CLSCompliant(false)]
  public class SkyBoxImporter : ContentImporter<TImport>
  {
    /// <summary>
    /// Imports the specified filename.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <param name="context">The context.</param>
    /// <returns>A new object of type timport</returns>
    public override TImport Import(string filename, ContentImporterContext context)
    {
      // TODO: read the specified file into an instance of the imported type.
      throw new NotImplementedException();
    }
  }
}
