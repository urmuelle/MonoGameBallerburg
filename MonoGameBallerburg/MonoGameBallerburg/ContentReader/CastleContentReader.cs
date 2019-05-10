// <copyright file="CastleContentReader.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg
{
  using Microsoft.Xna.Framework.Content;
  using MonoGameBallerburg.Graphic;

  public class CastleContentReader : ContentTypeReader<Castle>
  {
    protected override Castle Read(ContentReader input, Castle existingInstance)
    {
      return new Castle(input);
    }
  }
}
