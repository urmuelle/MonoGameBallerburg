// <copyright file="Main.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace CastleProcessor
{
  using System.Xml;
  using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

  /// <summary>
  /// The "starter" class for the processor
  /// </summary>
  public static class MainClass
  {
    /// <summary>
    /// Mains this instance.
    /// </summary>
    public static void Main()
    {
      var content = new CastleContent();
      var settings = new XmlWriterSettings { Indent = true };

      using (var writer = XmlWriter.Create("test.xml", settings))
      {
        IntermediateSerializer.Serialize(writer, content, null);
      }
    }
  }
}