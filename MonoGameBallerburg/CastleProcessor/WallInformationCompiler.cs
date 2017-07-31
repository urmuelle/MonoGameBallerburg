// <copyright file="WallInformationCompiler.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
namespace CastleProcessor
{
    using Microsoft.Xna.Framework.Content.Pipeline;
    using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
    using TWrite = CastleContent.WallInformation;

    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class WallInformationCompiler : ContentTypeWriter<TWrite>
    {
        /// <summary>
        /// Gets the assembly qualified name of the runtime loader for this type.
        /// </summary>
        /// <param name="targetPlatform">Name of the platform.</param>
        /// <returns>A string with the runtime type information</returns>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {            
            return "MonoGameBallerburg.WallInformationReader, MonoGameBallerburg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        /// <summary>
        /// Return a string containing the runtime type information
        /// </summary>
        /// <param name="targetPlatform">The target platform.</param>
        /// <returns>string with the runtime type information</returns>
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "MonoGameBallerburg.Graphic.Wall, MonoGameBallerburg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        /// <summary>
        /// Writes the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="value">The value.</param>
        protected override void Write(ContentWriter output, TWrite value)
        {
            output.Write(value.From);
            output.Write(value.To);
            output.Write(value.Height);
            output.WriteExternalReference<TextureContent>(value.WallReference);
        }
    }
}
