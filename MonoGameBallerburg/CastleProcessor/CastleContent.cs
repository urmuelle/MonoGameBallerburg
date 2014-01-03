// <copyright file="CastleContent.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace CastleProcessor
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Content.Pipeline;
    using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

    /// <summary>
    /// Class representing a ballerburg castle
    /// </summary>
    public class CastleContent
    {
        #region variables

        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
            Justification = "Reviewed. Suppression is OK here.")]
        [ContentSerializerIgnore]
        public ExternalReference<TextureContent> CoverTexture = null;

        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
            Justification = "Reviewed. Suppression is OK here.")]
        [ContentSerializerIgnore]
        public ExternalReference<TextureContent> WallTexture = null;

        private List<TowerInformation> towers = new List<TowerInformation>();

        private List<WallInformation> walls = new List<WallInformation>();

        private string name;

        private int price;

        private int id;

        #endregion

        #region properties

        /// <summary>
        /// Gets the towers.
        /// </summary>
        public List<TowerInformation> Towers
        {
            get
            {
                return this.towers;
            }
        }

        /// <summary>
        /// Gets the walls.
        /// </summary>        
        public List<WallInformation> Walls
        {
            get
            {
                return this.walls;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public int Price
        {
            get
            {
                return this.price;
            }

            set
            {
                this.price = value;
            }
        }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
        public int ID
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        #endregion

        #region classes

        /// <summary>
        /// Class representing the logical structure of a tower
        /// </summary>
        public class TowerInformation
        {
            #region variables

            private Vector2 center;

            private float height;

            private int corners;

            private float radius;

            private float coverRadius;

            private float coverHeight;

            private bool hasCannon;

            private int id;

            private ExternalReference<TextureContent> wallTextureReference;

            private ExternalReference<TextureContent> coverTextureReference;

            private string wallTexture;

            private string coverTexture;

            #endregion

            #region properties

            /// <summary>
            /// Gets or sets the center.
            /// </summary>
            /// <value>
            /// The center.
            /// </value>
            public Vector2 Center
            {
                get
                {
                    return this.center;
                }

                set
                {
                    this.center = value;
                }
            }

            /// <summary>
            /// Gets or sets the height.
            /// </summary>
            /// <value>
            /// The height.
            /// </value>
            public float Height
            {
                get
                {
                    return this.height;
                }

                set
                {
                    this.height = value;
                }
            }

            /// <summary>
            /// Gets or sets the corners.
            /// </summary>
            /// <value>
            /// The corners.
            /// </value>
            public int Corners
            {
                get
                {
                    return this.corners;
                }

                set
                {
                    this.corners = value;
                }
            }

            /// <summary>
            /// Gets or sets the radius.
            /// </summary>
            /// <value>
            /// The radius.
            /// </value>
            public float Radius
            {
                get
                {
                    return this.radius;
                }

                set
                {
                    this.radius = value;
                }
            }

            /// <summary>
            /// Gets or sets the cover radius.
            /// </summary>
            /// <value>
            /// The cover radius.
            /// </value>
            public float CoverRadius
            {
                get
                {
                    return this.coverRadius;
                }

                set
                {
                    this.coverRadius = value;
                }
            }

            /// <summary>
            /// Gets or sets the height of the cover.
            /// </summary>
            /// <value>
            /// The height of the cover.
            /// </value>
            public float CoverHeight
            {
                get
                {
                    return this.coverHeight;
                }

                set
                {
                    this.coverHeight = value;
                }
            }

            /// <summary>
            /// Gets or sets a value indicating whether this instance has cannon.
            /// </summary>
            /// <value>
            ///     <c>true</c> if this instance has cannon; otherwise, <c>false</c>.
            /// </value>
            public bool HasCannon
            {
                get
                {
                    return this.hasCannon;
                }

                set
                {
                    this.hasCannon = value;
                }
            }

            /// <summary>
            /// Gets or sets the ID.
            /// </summary>
            /// <value>
            /// The ID.
            /// </value>
            public int ID
            {
                get
                {
                    return this.id;
                }

                set
                {
                    this.id = value;
                }
            }

            /// <summary>
            /// Gets or sets the cover reference.
            /// </summary>
            /// <value>
            /// The cover reference.
            /// </value>
            public ExternalReference<TextureContent> CoverReference
            {
                get
                {
                    return this.coverTextureReference;
                }

                set
                {
                    this.coverTextureReference = value;
                }
            }

            /// <summary>
            /// Gets or sets the wall reference.
            /// </summary>
            /// <value>
            /// The wall reference.
            /// </value>
            public ExternalReference<TextureContent> WallReference
            {
                get
                {
                    return this.wallTextureReference;
                }

                set
                {
                    this.wallTextureReference = value;
                }
            }

            /// <summary>
            /// Gets or sets the cover texture.
            /// </summary>
            /// <value>
            /// The cover texture.
            /// </value>
            public string CoverTexture
            {
                get
                {
                    return this.coverTexture;
                }

                set
                {
                    this.coverTexture = value;
                }
            }

            /// <summary>
            /// Gets or sets the wall texture.
            /// </summary>
            /// <value>
            /// The wall texture.
            /// </value>
            public string WallTexture
            {
                get
                {
                    return this.wallTexture;
                }

                set
                {
                    this.wallTexture = value;
                }
            }

            #endregion
        }

        /// <summary>
        /// Class representing the logical description of a wall
        /// </summary>
        public class WallInformation
        {
            private int from;

            private int to;

            private float height;

            private ExternalReference<TextureContent> wallTextureReference;

            private string wallTexture;

            /// <summary>
            /// Gets or sets from.
            /// </summary>
            /// <value>
            /// The id of the tower, this wall starts.
            /// </value>
            public int From
            {
                get
                {
                    return this.from;
                }

                set
                {
                    this.from = value;
                }
            }

            /// <summary>
            /// Gets or sets to.
            /// </summary>
            /// <value>
            /// The id of the tower, this wall end.
            /// </value>
            public int To
            {
                get
                {
                    return this.to;
                }

                set
                {
                    this.to = value;
                }
            }

            /// <summary>
            /// Gets or sets the height.
            /// </summary>
            /// <value>
            /// The height.
            /// </value>
            public float Height
            {
                get
                {
                    return this.height;
                }

                set
                {
                    this.height = value;
                }
            }

            /// <summary>
            /// Gets or sets the wall reference.
            /// </summary>
            /// <value>
            /// The wall reference.
            /// </value>
            public ExternalReference<TextureContent> WallReference
            {
                get
                {
                    return this.wallTextureReference;
                }

                set
                {
                    this.wallTextureReference = value;
                }
            }

            /// <summary>
            /// Gets or sets the wall texture.
            /// </summary>
            /// <value>
            /// The wall texture.
            /// </value>
            public string WallTexture
            {
                get
                {
                    return this.wallTexture;
                }

                set
                {
                    this.wallTexture = value;
                }
            }
        }

        #endregion
    }
}