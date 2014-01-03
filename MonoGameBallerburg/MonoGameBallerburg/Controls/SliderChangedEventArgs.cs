// -----------------------------------------------------------------------
// <copyright file="SliderChangedEventArgs.cs" company="Urs Müller">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace MonoGameBallerburg.Controls
{
    using System;

    /// <summary>
    /// A Slider Control
    /// </summary>
    public class SliderChangedEventArgs : EventArgs
    {
        private float value;

        /// <summary>
        /// Initializes a new instance of the <see cref="SliderChangedEventArgs"/> class.
        /// </summary>
        /// <param name="sliderValue">The slider value (range 0.0 - 1.0)</param>
        public SliderChangedEventArgs(float sliderValue)
        {
            this.value = sliderValue;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public float Value
        {
            get { return this.value; }
        }
    }
}