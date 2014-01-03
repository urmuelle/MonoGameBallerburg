// <copyright file="CastleProcessor.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
namespace CastleProcessor
{
    using Microsoft.Xna.Framework.Content.Pipeline;
    using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentProcessor(DisplayName = "Castle Processor")]
    public class CastleProcessor : ContentProcessor<CastleContent, CastleContent>
    {
        /// <summary>
        /// The Process Method to return CasteContent Objects
        /// </summary>
        /// <param name="input">Castle Content Object</param>
        /// <param name="context">Processor Context</param>
        /// <returns>A new Castle Content Object</returns>
        public override CastleContent Process(CastleContent input, ContentProcessorContext context)
        {
            CastleContent content = input;

            foreach (CastleContent.TowerInformation towerInformation in content.Towers)
            {
                towerInformation.CoverReference = context.BuildAsset<TextureContent, TextureContent>(
                    new ExternalReference<TextureContent>(towerInformation.CoverTexture), "TextureProcessor");

                towerInformation.WallReference = context.BuildAsset<TextureContent, TextureContent>(
                    new ExternalReference<TextureContent>(towerInformation.WallTexture), "TextureProcessor");
            }

            foreach (CastleContent.WallInformation wallInformation in content.Walls)
            {
                wallInformation.WallReference = context.BuildAsset<TextureContent, TextureContent>(
                    new ExternalReference<TextureContent>(wallInformation.WallTexture), "TextureProcessor");
            }

            return content;
        }
    }
}