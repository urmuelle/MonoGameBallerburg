// <copyright file="CastleImporter.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
namespace CastleProcessor
{
    using System;
    using System.ComponentModel;
    using System.Xml;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content.Pipeline;

    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    /// <para>This should be part of a Content Pipeline Extension Library project.</para>
    /// </summary>
    [ContentImporter(".castle", DisplayName = "Castle Importer", DefaultProcessor = "CastleProcessor")]
    public class CastleImporter : ContentImporter<CastleContent>
    {
        /// <summary>
        /// Imports the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="context">The context.</param>
        /// <returns>A castle content object</returns>
        public override CastleContent Import(string filename, ContentImporterContext context)
        {
            var content = new CastleContent();

            // XML-Datei laden
            var document = new XmlDocument();
            document.Load(filename);

            // CastleEffect-Element abspeichern
            XmlNode root = document.DocumentElement;

            // Alle untergeordneten Nodes auslesen
            foreach (XmlNode node in root.ChildNodes)
            {
                this.ReadXmlNode(content, node);
            }

            return content;
        }

        /// <summary>
        /// Reads the XML node.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="node">The node.</param>
        protected virtual void ReadXmlNode(CastleContent content, XmlNode node)
        {
            SingleConverter floatConverter = new SingleConverter();
            BooleanConverter boolConverter = new BooleanConverter();
            StringConverter stringConverter = new StringConverter();
            Int32Converter integerConverter = new Int32Converter();

            switch (node.Name.ToLower())
            {
                case "name":
                    content.Name = (string)stringConverter.ConvertFromInvariantString(node.Attributes.GetNamedItem("value").InnerText);
                    break;

                case "price":
                    content.Price = (int)integerConverter.ConvertFromInvariantString(node.Attributes.GetNamedItem("value").InnerText);
                    break;

                case "tower":
                    CastleContent.TowerInformation info = new CastleContent.TowerInformation();

                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        this.ReadTowerNode(info, childNode);                        
                    }

                    info.HasCannon = Convert.ToBoolean(node.Attributes.GetNamedItem("hasCannon").Value);
                    info.ID = Convert.ToInt32(node.Attributes.GetNamedItem("id").Value);
                    content.Towers.Add(info);
                    break;

                case "wall":
                    CastleContent.WallInformation wallInfo = new CastleContent.WallInformation();

                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        this.ReadWallNode(wallInfo, childNode);
                    }

                    wallInfo.From = (int)integerConverter.ConvertFromInvariantString(node.Attributes.GetNamedItem("from").Value);
                    wallInfo.To = (int)integerConverter.ConvertFromInvariantString(node.Attributes.GetNamedItem("to").Value);
                    wallInfo.Height = (float)floatConverter.ConvertFromInvariantString(node.Attributes.GetNamedItem("height").Value);
                    content.Walls.Add(wallInfo);
                    break;
            }
        }

        /// <summary>
        /// Reads the tower node.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="node">The node.</param>
        protected virtual void ReadTowerNode(CastleContent.TowerInformation info, XmlNode node)
        {
            SingleConverter floatConverter = new SingleConverter();
            Int32Converter integerConverter = new Int32Converter();            

            switch (node.Name.ToLower())
            {
                case "position":
                    var x = (int)integerConverter.ConvertFrom(node.Attributes.GetNamedItem("x").InnerText);
                    var y = (int)integerConverter.ConvertFrom(node.Attributes.GetNamedItem("y").InnerText);
                    info.Center = new Vector2(x, y);
                    break;

                case "height":
                    // ConvertFromInvariantString() erwartet die Fließkommawerte in englischer Notation: 1.0
                    // ConvertFrom() verwendet die aktuelle Spracheinstellung des Threads. Auf deutschen Systemen wird also: 1,0 verlangt
                    info.Height = (float)floatConverter.ConvertFromInvariantString(node.Attributes.GetNamedItem("value").InnerText);
                    break;

                case "corners":
                    info.Corners = (int)integerConverter.ConvertFrom(node.Attributes.GetNamedItem("value").InnerText);
                    break;

                case "radius":
                    info.Radius = (float)floatConverter.ConvertFromInvariantString(node.Attributes.GetNamedItem("value").InnerText);
                    break;

                case "coverradius":
                    info.CoverRadius = (float)floatConverter.ConvertFromInvariantString(node.Attributes.GetNamedItem("value").InnerText);
                    break;

                case "coverheight":
                    info.CoverHeight = (float)floatConverter.ConvertFromInvariantString(node.Attributes.GetNamedItem("value").InnerText);
                    break;

                case "walltexture":
                    info.WallTexture = node.InnerText;
                    break;

                case "covertexture":
                    info.CoverTexture = node.InnerText;
                    break;
            }
        }

        /// <summary>
        /// Reads the wall node.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="node">The node.</param>
        protected virtual void ReadWallNode(CastleContent.WallInformation info, XmlNode node)
        {
            switch (node.Name.ToLower())
            {
                case "walltexture":
                    info.WallTexture = node.InnerText;
                    break;
            }
        }
    }
}
