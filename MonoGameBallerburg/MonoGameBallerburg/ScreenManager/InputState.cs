// <copyright file="InputState.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>
namespace MonoGameBallerburg
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Helper for reading input from keyboard and gamepad. This class tracks both
    /// the current and previous state of both input devices, and implements query
    /// properties for high level input actions such as "move up through the menu"
    /// or "pause the game".
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Imported from sample project")]
    public class InputState
    {
        #region Fields

        public const int MaxInputs = 4;

        public readonly KeyboardState[] CurrentKeyboardStates;
        public readonly GamePadState[] CurrentGamePadStates;

        public readonly KeyboardState[] LastKeyboardStates;
        public readonly GamePadState[] LastGamePadStates;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="InputState"/> class.
        /// </summary>
        public InputState()
        {
            // prepare keyboard (obsolete)
            this.CurrentKeyboardStates = new KeyboardState[MaxInputs];
            this.CurrentGamePadStates = new GamePadState[MaxInputs];

            this.LastKeyboardStates = new KeyboardState[MaxInputs];
            this.LastGamePadStates = new GamePadState[MaxInputs];

            // prepare mouse - if needed any time
        }

        #endregion

        #region Properties

        /// <summary>
        /// Checks for a "menu up" input action, from any player,
        /// on either keyboard or gamepad.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [menu up]; otherwise, <c>false</c>.
        /// </value>
        public bool MenuUp
        {
            get
            {
                return this.IsNewKeyPress(Keys.Up) ||
                       this.IsNewButtonPress(Buttons.DPadUp) ||
                       this.IsNewButtonPress(Buttons.LeftThumbstickUp);
            }
        }

        /// <summary>
        /// Checks for a "menu down" input action, from any player,
        /// on either keyboard or gamepad.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [menu down]; otherwise, <c>false</c>.
        /// </value>
        public bool MenuDown
        {
            get
            {
                return this.IsNewKeyPress(Keys.Down) ||
                       this.IsNewButtonPress(Buttons.DPadDown) ||
                       this.IsNewButtonPress(Buttons.LeftThumbstickDown);
            }
        }

        /// <summary>
        /// Checks for a "menu select" input action, from any player,
        /// on either keyboard or gamepad.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [menu select]; otherwise, <c>false</c>.
        /// </value>
        public bool MenuSelect
        {
            get
            {
                return this.IsNewKeyPress(Keys.Space) ||
                       this.IsNewKeyPress(Keys.Enter) ||
                       this.IsNewButtonPress(Buttons.A) ||
                       this.IsNewButtonPress(Buttons.Start);
            }
        }

        /// <summary>
        /// Checks for a "menu cancel" input action, from any player,
        /// on either keyboard or gamepad.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [menu cancel]; otherwise, <c>false</c>.
        /// </value>
        public bool MenuCancel
        {
            get
            {
                return this.IsNewKeyPress(Keys.Escape) ||
                       this.IsNewButtonPress(Buttons.B) ||
                       this.IsNewButtonPress(Buttons.Back);
            }
        }

        /// <summary>        
        /// Checks for a "pause the game" input action, from any player,
        /// on either keyboard or gamepad.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [pause game]; otherwise, <c>false</c>.
        /// </value>
        public bool PauseGame
        {
            get
            {
                return this.IsNewKeyPress(Keys.Escape) ||
                       this.IsNewButtonPress(Buttons.Back) ||
                       this.IsNewButtonPress(Buttons.Start);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the latest state of the keyboard and gamepad.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < MaxInputs; i++)
            {
                this.LastKeyboardStates[i] = this.CurrentKeyboardStates[i];
                this.LastGamePadStates[i] = this.CurrentGamePadStates[i];

                this.CurrentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
                this.CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);
            }
        }

        /// <summary>
        /// Helper for checking if a key was newly pressed during this update,
        /// by any player.
        /// </summary>
        /// <param name="key">The accroding key.</param>
        /// <returns>
        ///   <c>true</c> if [is new key press] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNewKeyPress(Keys key)
        {
            for (int i = 0; i < MaxInputs; i++)
            {
                if (this.IsNewKeyPress(key, (PlayerIndex)i))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Helper for checking if a key was newly pressed during this update,
        /// by the specified player.
        /// </summary>
        /// <param name="key">The according key.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns>
        ///   <c>true</c> if [is new key press] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNewKeyPress(Keys key, PlayerIndex playerIndex)
        {
            return this.CurrentKeyboardStates[(int)playerIndex].IsKeyDown(key) &&
                    this.LastKeyboardStates[(int)playerIndex].IsKeyUp(key);
        }

        /// <summary>
        /// Helper for checking if a button was newly pressed during this update,
        /// by any player.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <returns>
        ///   <c>true</c> if [is new button press] [the specified button]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNewButtonPress(Buttons button)
        {
            for (int i = 0; i < MaxInputs; i++)
            {
                if (this.IsNewButtonPress(button, (PlayerIndex)i))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Helper for checking if a button was newly pressed during this update,
        /// by the specified player.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns>
        ///   <c>true</c> if [is new button press] [the specified button]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNewButtonPress(Buttons button, PlayerIndex playerIndex)
        {
            return this.CurrentGamePadStates[(int)playerIndex].IsButtonDown(button) &&
                    this.LastGamePadStates[(int)playerIndex].IsButtonUp(button);
        }

        /// <summary>
        /// Checks for a "menu select" input action from the specified player.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns>
        ///   <c>true</c> if [is menu select] [the specified player index]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMenuSelect(PlayerIndex playerIndex)
        {
            return this.IsNewKeyPress(Keys.Space, playerIndex) ||
                   this.IsNewKeyPress(Keys.Enter, playerIndex) ||
                   this.IsNewButtonPress(Buttons.A, playerIndex) ||
                   this.IsNewButtonPress(Buttons.Start, playerIndex);
        }

        /// <summary>
        /// Checks for a "menu cancel" input action from the specified player.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns>
        ///   <c>true</c> if [is menu cancel] [the specified player index]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMenuCancel(PlayerIndex playerIndex)
        {
            return this.IsNewKeyPress(Keys.Escape, playerIndex) ||
                   this.IsNewButtonPress(Buttons.B, playerIndex) ||
                   this.IsNewButtonPress(Buttons.Back, playerIndex);
        }

        #endregion
    }
}
