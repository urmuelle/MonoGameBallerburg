// -----------------------------------------------------------------------
// <copyright file="ActionToggleButtonEventArgs.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
namespace MonoGameBallerburg.Controls
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ActionToggleButtonEventArgs : EventArgs
    {
        private int selectedIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionToggleButtonEventArgs"/> class.
        /// </summary>
        /// <param name="selectedIndex">Index of the selected.</param>
        public ActionToggleButtonEventArgs(int selectedIndex)
        {
            this.selectedIndex = selectedIndex;
        }

        /// <summary>
        /// Gets the center X.
        /// </summary>
        public int SelectedIndex
        {
            get { return this.selectedIndex; }
        }
    }
}
