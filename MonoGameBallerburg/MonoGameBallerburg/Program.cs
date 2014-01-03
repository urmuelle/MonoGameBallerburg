// <copyright file="Program.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg
{
  using System;

  /// <summary>
  /// The main class.
  /// </summary>
  public static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    public static void Main()
    {
      var factory = new MonoGame.Framework.GameFrameworkViewSource<BallerburgGame>();
      Windows.ApplicationModel.Core.CoreApplication.Run(factory);
    }
  }
}
