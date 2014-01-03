// <copyright file="GraphikScreen.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Screens
{
    using System;
    using Controls;
    using Manager;
    using Microsoft.Xna.Framework;

    public class GraphikScreen : MenuScreen
    {
        private readonly MenuEntry zurueckMenuEntry;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphikScreen"/> class.
        /// </summary>
        /// <param name="graphicsManager">The graphics manager.</param>
        /// <param name="screenManager">The screen manager.</param>
        public GraphikScreen(IBallerburgGraphicsManager graphicsManager, IScreenManager screenManager)
            : base(graphicsManager, screenManager, "Ballerburg3D")
        {
            // Zurück button
            zurueckMenuEntry = new MenuEntry(this, ResourceLoader.GetString("BackText"), 0) { Position = new Vector2(500, 450) };
            zurueckMenuEntry.Selected += ZurueckMenuEntrySelected;
            ControlsContainer.Add(zurueckMenuEntry);
        }

        /// <summary>
        /// When the user presses this button, we go on to the messagebox screen
        /// asking for the gamestyle he wants to play
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ZurueckMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AudioManager.PlayKlickSound();
            ExitScreen();
        }

        /// <summary>
        /// TODO: Das muss mit Shader gemacht werden.
        /// </summary>
        private void AdjustGamma()
        {            
        }
    }
}
