// -----------------------------------------------------------------------
// <copyright file="SkyBoxImporter.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
// -----------------------------------------------------------------------
namespace SkyBoxProcessor
{
  using System;
  using Microsoft.Xna.Framework.Content.Pipeline;
  using TImport = SkyBoxContent;
  using System.Xml;
  using System.ComponentModel;

  /// <summary>
  /// An importer class for skybox objects
  /// </summary>
  [ContentImporter(".sky", DisplayName = "Sky Box Importer", DefaultProcessor = "SkyBoxProcessor")]
  [CLSCompliant(false)]
  public class SkyBoxImporter : ContentImporter<TImport>
  {
    /// <summary>
    /// Imports the specified filename.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <param name="context">The context.</param>
    /// <returns>A new object of type timport</returns>
    public override TImport Import(string filename, ContentImporterContext context)
    {
      var content = new SkyBoxContent();

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
    protected virtual void ReadXmlNode(SkyBoxContent content, XmlNode node)
    {
      StringConverter stringConverter = new StringConverter();

      switch (node.Name.ToLower())
      {
        case "front":
          content.Front = node.InnerText;
          break;

        case "back":
          content.Back = node.InnerText;
          break;

        case "left":
          content.Left = node.InnerText;
          break;

        case "right":
          content.Right = node.InnerText;
          break;

        case "top":
          content.Top = node.InnerText;
          break;

        case "bottom":
          content.Bottom = node.InnerText;
          break;
      }
    }
  }
}
