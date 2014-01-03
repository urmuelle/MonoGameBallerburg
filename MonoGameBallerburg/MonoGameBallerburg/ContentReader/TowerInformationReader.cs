// <copyright file="TowerInformationReader.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg
{
  using Graphic;
  using Microsoft.Xna.Framework.Content;

  /// <summary>
  /// Content Reader for the Content Pipeline Extension
  /// </summary>
  public class TowerInformationReader : ContentTypeReader<Tower>
  {
    /// <summary>
    /// Reads the specified input.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="existingInstance">The existing instance.</param>
    /// <returns>New Tower instance</returns>
    protected override Tower Read(ContentReader input, Tower existingInstance)
    {
      return new Tower(input);
    }
  }
}
