// <copyright file="IGameSettingsManager.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Manager
{
    using Gameplay;

    public interface IGameSettingsManager
    {
        /// <summary>
        /// Gets or sets the num players.
        /// </summary>
        /// <value>
        /// The num players.
        /// </value>
        int NumPlayers { get; set; }

        /// <summary>
        /// Gets or sets the game art.
        /// </summary>
        /// <value>
        /// The game art.
        /// </value>
        GameType GameStyle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether switching the camera is animated (bezier)
        /// </summary>
        /// <value>
        /// The flag value.
        /// </value>
        bool AnimatedCamera { get; set; }
    }
}