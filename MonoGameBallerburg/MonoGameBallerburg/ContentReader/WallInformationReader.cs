// <copyright file="WallInformationReader.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg
{
  using Graphic;
  using Microsoft.Xna.Framework.Content;

  public class WallInformationReader : ContentTypeReader<Wall>
  {
    /// <summary>
    /// Reads the specified input.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="existingInstance">The existing instance.</param>
    /// <returns>A new wall object</returns>
    protected override Wall Read(ContentReader input, Wall existingInstance)
    {
      return new Wall(input);
    }
  }
}
