﻿// -----------------------------------------------------------------------
// <copyright file="TowerInformationCompiler.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
// -----------------------------------------------------------------------
namespace CastleProcessor
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content.Pipeline;
    using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
    using TWrite = CastleContent.TowerInformation;

    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class TowerInformationCompiler : ContentTypeWriter<TWrite>
    {
        /// <summary>
        /// Gets the assembly qualified name of the runtime loader for this type.
        /// </summary>
        /// <param name="targetPlatform">Name of the platform.</param>
        /// <returns>string with the runtime type information.</returns>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            // class which will be used to load this data.
            return "MonoGameBallerburg.TowerInformationReader, MonoGameBallerburg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        /// <summary>
        /// Return a string with the runtime type information.
        /// </summary>
        /// <param name="targetPlatform">The target platform.</param>
        /// <returns>string with the runtime type information.</returns>
        public override string GetRuntimeType(Microsoft.Xna.Framework.Content.Pipeline.TargetPlatform targetPlatform)
        {
            return "MonoGameBallerburg.Graphic.Tower, MonoGameBallerburg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        /// <summary>
        /// Writes the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="value">The value.</param>
        protected override void Write(ContentWriter output, TWrite value)
        {
            output.Write(value.ID);
            output.Write(value.HasCannon);
            output.WriteObject<Vector2>(value.Center);
            output.Write(value.Height);
            output.Write(value.Corners);
            output.Write(value.Radius);
            output.Write(value.CoverRadius);
            output.Write(value.CoverHeight);

            // Referenzen auf die Texturen wegschreiben
            output.WriteExternalReference<TextureContent>(value.WallReference);
            output.WriteExternalReference<TextureContent>(value.CoverReference);
        }
    }
}
