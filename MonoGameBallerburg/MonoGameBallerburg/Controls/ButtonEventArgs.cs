// <copyright file="ButtonEventArgs.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
namespace MonoGameBallerburg.Controls
{
    using System;

    /// <summary>
    /// Event Args for GamePlay Menu screen buttons
    /// </summary>
    public class ButtonEventArgs : EventArgs
    {
        private int centerX;
        private int centerY;
        private string mouseOverText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonEventArgs"/> class.
        /// </summary>
        /// <param name="centerX">The center X.</param>
        /// <param name="centerY">The center Y.</param>
        /// <param name="mouseOverText">The mouse over text.</param>
        public ButtonEventArgs(int centerX, int centerY, string mouseOverText)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            this.mouseOverText = mouseOverText;
        }

        /// <summary>
        /// Gets the center X.
        /// </summary>
        public int CenterX
        {
            get { return this.centerX; }
        }

        /// <summary>
        /// Gets the center Y.
        /// </summary>
        public int CenterY
        {
            get { return this.centerY; }
        }

        /// <summary>
        /// Gets the mouse over text.
        /// </summary>
        public string MouseOverText
        {
            get { return this.mouseOverText; }
        }
    }
}
