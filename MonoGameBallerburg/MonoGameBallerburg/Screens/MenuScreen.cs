// <copyright file="MenuScreen.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Screens
{
    using System;
    using System.Collections.Generic;
    using Controls;
    using Manager;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    public abstract class MenuScreen : GameScreen
    {
        #region Fields

        private readonly List<MenuEntry> menuEntries = new List<MenuEntry>();
        private readonly int selectedEntry = 0;
        private readonly string menuTitle;
        private IScreenManager screenManager;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuScreen"/> class.
        /// </summary>
        /// <param name="graphicsManager">The graphics manager.</param>
        /// <param name="screenManager">The screen manager.</param>
        /// <param name="menuTitle">The menu title.</param>
        protected MenuScreen(IBallerburgGraphicsManager graphicsManager, IScreenManager screenManager, string menuTitle)
            : base(graphicsManager)
        {
            this.menuTitle = menuTitle;
            this.screenManager = screenManager;
            TransitionOnTime = TimeSpan.FromSeconds(0);
            TransitionOffTime = TimeSpan.FromSeconds(0);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }

        protected IGameSettingsManager GameSettings
        {
            get { return screenManager.GameSettings; }
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        /// <param name="input">The input.</param>
        public override void HandleInput(InputState input)
        {            
            /*
            if (ScreenManager.GameMousePointer.MouseButtonLeftClicked)
            {
                if (selectedEntry > -1)
                    OnSelectEntry(selectedEntry);
            }
             * */
        }        

        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the menu.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="otherScreenHasFocus">if set to <c>true</c> [other screen has focus].</param>
        /// <param name="coveredByOtherScreen">if set to <c>true</c> [covered by other screen].</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (!IsActive)
            {
                for (var i = 0; i < menuEntries.Count; i++)
                {
                    menuEntries[i].Enabled = false;
                    menuEntries[i].Visible = false;                    
                }

                foreach (var c in ControlsContainer)
                {
                    c.Enabled = false;
                    c.Visible = false;
                    c.Deactivate();
                }
            }
            else
            {
                for (var i = 0; i < menuEntries.Count; i++)
                {
                    menuEntries[i].Enabled = true;
                    menuEntries[i].Visible = true;
                }

                foreach (var c in ControlsContainer)
                {
                    c.Enabled = true;
                    c.Visible = true;
                    c.Activate();
                }
            }

            /*
            if (!otherScreenHasFocus)
            {
                for (int i = 0; i < menuEntries.Count; i++)
                {
                    MenuEntry menuEntry = menuEntries[i];

                    menuEntry.IsActive = false;
                }
            }
            */
            /*
            if (!(otherScreenHasFocus))
            {

                selectedEntry = -1;

                // Draw each menu entry in turn.
                for (int i = 0; i < menuEntries.Count; i++)
                {
                    MenuEntry menuEntry = menuEntries[i];

                    bool isSelected = menuEntry.IsSelected(this, ScreenManager.GameMousePointer.MousePosition);

                    if (isSelected && !(ScreenManager.GameMousePointer.MouseButtonLeftPressed))
                        selectedEntry = menuEntry.Id;
                }
            }
             * */
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            /*
            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = menuEntry.IsSelected(this, ScreenManager.GameMousePointer.MousePosition);

                if (isSelected && !(ScreenManager.GameMousePointer.MouseButtonLeftPressed)) { }
                //selectedEntry = menuEntry.Id;
                else
                {
                    //selectedEntry = -1;
                    isSelected = false;
                }

                menuEntry.Draw(this, ScreenManager.GameMousePointer, isSelected, gameTime);
            }
             * */

            if (ScreenState == ScreenState.Active)
            {
                DrawTitle();
            }
        }

        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        /// <param name="entryIndex">Index of the entry.</param>
        protected virtual void OnSelectEntry(int entryIndex)
        {
            menuEntries[selectedEntry].OnSelectEntry();
        }

        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel()
        {
            ScreenManager.AudioManager.PlayKlickSound();
            ExitScreen();
        }

        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void OnCancel(object sender, EventArgs e)
        {
            ScreenManager.AudioManager.PlayKlickSound();
            OnCancel();
        }

        /// <summary>
        /// Draws the title.
        /// </summary>
        private void DrawTitle()
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            var font = ScreenManager.Font;

            // Draw the menu title.
            var titlePosition = 1.25f * (font.MeasureString(menuTitle) / 2);
            titlePosition.X += 20;
            var titleOrigin = font.MeasureString(menuTitle) / 2;
            var titleColor = Color.Blue;
            const float TitleScale = 1.25f;

            spriteBatch.Begin();

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0, titleOrigin, TitleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        #endregion
    }
}
