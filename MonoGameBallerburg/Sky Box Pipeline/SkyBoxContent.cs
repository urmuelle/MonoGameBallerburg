// -----------------------------------------------------------------------
// <copyright file="SkyBoxContent.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
// -----------------------------------------------------------------------

namespace SkyBoxProcessor
{
  using System;
  using System.Diagnostics.CodeAnalysis;

  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Content.Pipeline;
  using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
  using Microsoft.Xna.Framework.Graphics;

  /// <summary>
  /// The serialized content of a Skybox object
  /// </summary>
  public class SkyBoxContent
  {
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
        Justification = "Reviewed. Suppression is OK here.")]
    [ContentSerializerIgnore]
    [CLSCompliant(false)]
    public ExternalReference<TextureContent> Texture = null;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
        Justification = "Reviewed. Suppression is OK here.")]
    [ContentSerializerIgnore]
    [CLSCompliant(false)]
    public VertexPositionTexture[] Vertices = null;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
        Justification = "Reviewed. Suppression is OK here.")]
    [ContentSerializerIgnore]
    [CLSCompliant(false)]
    public ExternalReference<TextureContent> FrontTexture;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
        Justification = "Reviewed. Suppression is OK here.")]
    [ContentSerializerIgnore]
    [CLSCompliant(false)]
    public ExternalReference<TextureContent> BackTexture;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
        Justification = "Reviewed. Suppression is OK here.")]
    [ContentSerializerIgnore]
    [CLSCompliant(false)]
    public ExternalReference<TextureContent> LeftTexture;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
        Justification = "Reviewed. Suppression is OK here.")]
    [ContentSerializerIgnore]
    [CLSCompliant(false)]
    public ExternalReference<TextureContent> RightTexture;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
        Justification = "Reviewed. Suppression is OK here.")]
    [ContentSerializerIgnore]
    [CLSCompliant(false)]
    public ExternalReference<TextureContent> TopTexture;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
        Justification = "Reviewed. Suppression is OK here.")]
    [ContentSerializerIgnore]
    [CLSCompliant(false)]
    public ExternalReference<TextureContent> BottomTexture;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
        Justification = "Reviewed. Suppression is OK here.")]
    public string Front = string.Empty;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
        Justification = "Reviewed. Suppression is OK here.")]
    public string Back = string.Empty;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
        Justification = "Reviewed. Suppression is OK here.")]
    public string Left = string.Empty;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
        Justification = "Reviewed. Suppression is OK here.")]
    public string Right = string.Empty;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
        Justification = "Reviewed. Suppression is OK here.")]
    public string Top = string.Empty;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
        Justification = "Reviewed. Suppression is OK here.")]
    public string Bottom = string.Empty;
  }
}