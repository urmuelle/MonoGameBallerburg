// <copyright file="VertexPositionTextureReader.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
namespace MonoGameBallerburg
{    
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class VertexPositionTextureReader : ContentTypeReader<VertexPositionTexture>
    {
        /// <summary>
        /// Reads the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="existingInstance">The existing instance.</param>
        /// <returns>A new VertexPositionTexture instance</returns>
        protected override VertexPositionTexture Read(ContentReader input, VertexPositionTexture existingInstance)
        {
            VertexPositionTexture vertex = new VertexPositionTexture();
            vertex.Position = input.ReadVector3();
            vertex.TextureCoordinate = input.ReadVector2();

            return vertex;
        }
    }
}
