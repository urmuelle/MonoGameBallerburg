// <copyright file="CastleContentReader.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg
{
    using Graphic;
    using Microsoft.Xna.Framework.Content;

    public class CastleContentReader : ContentTypeReader<Castle>
    {
        protected override Castle Read(ContentReader input, Castle existingInstance)
        {
            return new Castle(input);
        }
    }
}
