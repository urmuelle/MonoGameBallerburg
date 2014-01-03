// <copyright file="GlobalSuppressions.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Ballerburg.Controls.ActionToggleButton.#ctor(Ballerburg.GameScreen,System.String,System.String,System.Int32)", Scope = "member", Target = "Ballerburg.Screens.MainMenuScreen.#.ctor(Ballerburg.Manager.IBallerburgGraphicsManager)", Justification = "OK")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Microsoft.Xna.Framework.Graphics.SpriteBatch.DrawString(Microsoft.Xna.Framework.Graphics.SpriteFont,System.String,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Color,System.Single,Microsoft.Xna.Framework.Vector2,System.Single,Microsoft.Xna.Framework.Graphics.SpriteEffects,System.Single)", Scope = "member", Target = "Ballerburg.Controls.OnOffToggleButton.#Draw(Microsoft.Xna.Framework.GameTime)", Justification = "OK")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Scope = "type", Target = "Ballerburg.Manager.BallerburgGraphicsManager", Justification = "OK")]
