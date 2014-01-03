// <copyright file="SkyBoxContentReader.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
namespace MonoGameBallerburg
{
    using Graphic;
    using Microsoft.Xna.Framework.Content;

    public class SkyBoxContentReader : ContentTypeReader<SkyBox>
    {
        /// <summary>
        /// Reads the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="existingInstance">The existing instance.</param>
        /// <returns>A new SkyBox object</returns>
        protected override SkyBox Read(ContentReader input, SkyBox existingInstance)
        {
            return new SkyBox(input);
        }
    }
}
