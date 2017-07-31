// -----------------------------------------------------------------------
// <copyright file="CastleContentCompiler.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
// -----------------------------------------------------------------------
namespace CastleProcessor
{
    using Microsoft.Xna.Framework.Content.Pipeline;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
    using TWrite = CastleContent;

    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class CastleContentCompiler : ContentTypeWriter<TWrite>
    {
        /// <summary>
        /// Gets the assembly qualified name of the runtime loader for this type.
        /// </summary>
        /// <param name="targetPlatform">Name of the platform.</param>
        /// <returns>The type string</returns>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            // TODO: change this to the name of your ContentTypeReader
            // class which will be used to load this data.
            return "MonoGameBallerburg.CastleContentReader, MonoGameBallerburg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        /// <summary>
        /// Return the runtime type information of this assembly
        /// </summary>
        /// <param name="targetPlatform">The target platform.</param>
        /// <returns>the type string</returns>
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "MonoGameBallerburg.Graphic.Castle, MonoGameBallerburg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        /// <summary>
        /// Writes the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="value">The value.</param>
        protected override void Write(ContentWriter output, TWrite value)
        {
            output.Write(value.Name);
            output.Write(value.Price);

            CastleContent.TowerInformation[] towers = new CastleContent.TowerInformation[value.Towers.Count];
            value.Towers.CopyTo(towers);
            output.WriteObject<CastleContent.TowerInformation[]>(towers);
            CastleContent.WallInformation[] walls = new CastleContent.WallInformation[value.Walls.Count];
            value.Walls.CopyTo(walls);
            output.WriteObject<CastleContent.WallInformation[]>(walls);
        }
    }
}
