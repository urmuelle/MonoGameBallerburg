// <copyright file="Program.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace MonoGameBallerburg
{
  using System;

#if WINDOWS || LINUX
  /// <summary>
  /// The main class.
  /// </summary>
  public static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
      using (var game = new BallerburgGame())
      {
        game.Run();
      }
    }
  }
#endif
}
