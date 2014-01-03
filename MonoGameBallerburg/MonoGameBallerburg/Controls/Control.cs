// <copyright file="Control.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Controls
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The controls states
    /// </summary>
    public enum States
    {
        /// <summary>
        /// The entry is no well defined state yet
        /// </summary>
        None = 0,

        /// <summary>
        /// The entry is hidden
        /// </summary>
        Hidden = 1,

        /// <summary>
        /// The entry is visible
        /// </summary>
        Visible = 2,

        /// <summary>
        /// Mouse is over the entry
        /// </summary>
        MouseOver = 3,

        /// <summary>
        /// The entry is pressed with mouse
        /// </summary>
        Pressed = 4,

        /// <summary>
        /// The entry is inactive
        /// </summary>
        Inactive = 5
    }

    /// <summary>
    /// Base class for all controls used in the ballerbug game
    /// </summary>
    public abstract class Control
    {
        #region Variables        

        private static Color controlColor;
        private static Color controlDarkColor;
        private static Color controlHiColor;

        private GameScreen owner;
        private States state = States.Visible;       
        private GraphicsDevice graphicsDevice;
        private ContentManager content;
        private Vector2 position = Vector2.Zero;
        private int id;

        private string caption;
        private bool enabled = true;
        private bool visible = true;
        ////private bool isActive = false;
        private int tabIndex = 0;
        private int width = 0;
        private int height = 0;
        
        #endregion

        #region Events

        public event EventHandler<EventArgs> ControlActivated;

        public event EventHandler<EventArgs> ControlDeactived;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color of the control.
        /// </summary>
        /// <value>
        /// The color of the control.
        /// </value>
        public static Color ControlColor
        {
            get
            {
                return controlColor;
            }

            set
            {
                controlColor = value;
                controlDarkColor = Color.Lerp(Color.Black, value, 0.4f);
                controlHiColor = Color.Lerp(value, Color.White, 0.4f);
            }
        }

        /// <summary>
        /// Gets the color of the this control.
        /// </summary>
        /// <value>
        /// The color of the this control.
        /// </value>
        public Color ThisControlColor
        {
            get
            {
                if (this.Enabled)
                {
                // TODO: Implement HasFocus Function
                    ////if (XnaGUIManager.HasFocus(this))
                    ////    return controlHiColor;
                    return controlColor;
                }

                return controlDarkColor;
            }
        }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        public GameScreen Owner
        {
            get 
            { 
                return this.owner; 
            }

            set
            {
                this.owner = value;
            }
        }

        /// <summary>
        /// Gets or sets the device.
        /// </summary>
        /// <value>
        /// The device.
        /// </value>
        public GraphicsDevice Device
        {
            get { return this.graphicsDevice; }
            set { this.graphicsDevice = value; }
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public ContentManager Content
        {
            get { return this.content; }
            set { this.content = value; }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>
        /// The caption.
        /// </value>
        public string Caption
        {
            get { return this.caption; }
            set { this.caption = value; }
        }

        /// <summary>
        /// Gets or sets the index of the tab.
        /// </summary>
        /// <value>
        /// The index of the tab.
        /// </value>
        public int TabIndex
        {
            get { return this.tabIndex; }
            set { this.tabIndex = value; }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width
        {
            get { return this.width; }
            set { this.width = value; }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height
        {
            get { return this.height; }
            set { this.height = value; }
        }

        public bool Enabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        /*
        public bool IsActive
        {
            get { return this.isActive; }
            protected set { this.isActive = value; }
        }
         * */

        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        public States State
        {
            get { return this.state; }
            set { this.state = value; }
        }

        public int Id
        {
            get { return this.id; }
            protected set { this.id = value; }
        }

        public Rectangle Rectangle { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Aktiviert das Control
        /// </summary>
        public virtual void Activate()
        {
            this.Enabled = true;

            if (this.ControlActivated != null)
            {
                this.ControlActivated(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Deaktiviert das Control
        /// </summary>
        public virtual void Deactivate()
        {
            this.Enabled = false;

            if (this.ControlDeactived != null)
            {
                this.ControlDeactived(this, EventArgs.Empty);
            }
        }

        /*
        /// <summary>
        /// Prüft, ob der Cursor über dem Steuerelement ist
        /// </summary>
        /// <param name="oCursor">Cusor-Objekt, dass die aktuelle Position repräsentiert</param>
        /// <returns>True, wenn sich der Cursor über dem Control befindet</returns>
        public virtual bool CursorIntersects(Cursor oCursor, bool bAutoActivation)
        {
            if ((oCursor.X > this.Location.X && oCursor.X < (this.Location.X + this.Width)) &&
                (oCursor.Y > this.Location.Y && oCursor.Y < (this.Location.Y + this.Height)))
                if (bAutoActivation)
                {
                    //Cursor befindet sich über dem Control --> Control aktivieren
                    this.Activate();
                    return true;
                }
                else //Eine automatische Aktivierung ist nicht erwünscht --> Lediglich den erfolgreichen Test signalisieren
                    return true;

            //Der Cursor befindet sich nicht über dem Element
            return false;
        }
        
        /// <summary>
        /// Implementieren Sie an dieser Stelle die Logik, um auf Benutzereingaben zu reagieren
        /// </summary>
        /// <param name="oInput"></param>
        public virtual void HandleInput(InputManager oInput) { }

        /// <summary>
        /// Implementieren Sie an dieser Stelle die Logik, um auf Benutzereingaben zu reagieren. Per Default wird ein
        /// Intersection-Test mit dem Cursor durchgeführt.
        /// </summary>
        /// <param name="oInput">InputManager mit dem Status der Benutzereingaben</param>
        /// <param name="oCursor">Cursor-Objekt</param>
        public virtual void HandleInput(InputManager oInput, Cursor oCursor)
        {
            //Verharrt der Cursor an seiner Position (Delta = 0) oder wenn das
            //Control bereits aktiv ist, dann findet kein Intersection-Test statt.
            //Hintergrund: 
            //Wird der Cursor nicht bewegt und der Benutzer aktiviert z.B. über die Tastatur
            //ein anderes Element, so würde ein Intersection-Test unter Umständen dazu führen,
            //dass das Element unter dem Cursor wieder aktiv wird. Somit währe keine Steuerung
            //per Tastatur möglich.
            if (!this.IsActive && (oInput.DeltaX != 0 || oInput.DeltaY != 0))
                CursorIntersects(oCursor, true);
        }
        */

        /// <summary>
        /// Implementieren Sie diese Methode, um grafische Ressourcen zu laden
        /// </summary>
        public virtual void LoadContent()
        {
        }

        /// <summary>
        /// Implementieren Sie diese Methode, um "Aufräumarbeiten" bezüglich grafischer Ressourcen durchzuführen
        /// </summary>
        public virtual void UnloadContent()
        {
        }

        /// <summary>
        /// Implementiert die Logik des Controls
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Implementiert den Render-Prozess des Controls
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public abstract void Draw(GameTime gameTime);

        public bool Contains(Point point)
        {
            Rectangle rect = Rectangle;
            return rect.Contains(point);
        }

        #endregion
    }
}
