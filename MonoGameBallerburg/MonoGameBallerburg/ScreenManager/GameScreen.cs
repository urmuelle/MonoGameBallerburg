// <copyright file="GameScreen.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg
{
  using System;
  using System.Collections.ObjectModel;
  using System.Diagnostics.CodeAnalysis;
  using Controls;
  using Manager;
  using Microsoft.Xna.Framework;

  /// <summary>
  /// Enum describes the screen transition state.
  /// </summary>
  public enum ScreenState
  {
    /// <summary>
    /// Screen is appearing
    /// </summary>
    TransitionOn,

    /// <summary>
    /// Screen is shown
    /// </summary>
    Active,

    /// <summary>
    /// Screen is transitioning
    /// </summary>
    TransitionOff,

    /// <summary>
    /// Screen is hidden
    /// </summary>
    Hidden,
  }

  /// <summary>
  /// A screen is a single layer that has update and draw logic, and which
  /// can be combined with other layers to build up a complex menu system.
  /// For instance the main menu, the options menu, the "are you sure you
  /// want to quit" message box, and the main game itself are all implemented
  /// as screens.
  /// </summary>
  [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Imported from sample project")]
  public abstract class GameScreen
  {
    #region Fields

    private bool isPopup = false;
    private TimeSpan transitionOnTime = TimeSpan.Zero;
    private TimeSpan transitionOffTime = TimeSpan.Zero;
    ////private ScreenState screenState = ScreenState.Active;

    private float transitionPosition = 1;
    private bool isExiting = false;
    private bool otherScreenHasFocus;
    private ScreenManager screenManager;
    private ScreenState screenState = ScreenState.TransitionOn;

    private IBallerburgGraphicsManager graphicsManager;
    private Collection<Control> controls;

    //private Windows.ApplicationModel.Resources.ResourceLoader resourceLoader;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="GameScreen"/> class.
    /// </summary>
    /// <param name="graphicsManager">The graphics manager.</param>
    protected GameScreen(IBallerburgGraphicsManager graphicsManager)
    {
      this.graphicsManager = graphicsManager;

      controls = new Collection<Control>();
      //resourceLoader = new Windows.ApplicationModel.Resources.ResourceLoader();
      ////Viewport viewport = this.GraphicsManager.GraphicsDevice.Viewport;
    }

    #region Events

    public event EventHandler<EventArgs> ScreenActivated;

    public event EventHandler<EventArgs> ScreenDeactived;

    #endregion

    #region Properties

    /// <summary>
    /// Normally when one screen is brought up over the top of another,
    /// the first screen will transition off to make room for the new
    /// one. This property indicates whether the screen is only a small
    /// popup, in which case screens underneath it do not need to bother
    /// transitioning off.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is popup; otherwise, <c>false</c>.
    /// </value>
    public bool IsPopup
    {
      get { return isPopup; }
      protected set { isPopup = value; }
    }

    /// <summary>
    /// Indicates how long the screen takes to
    /// transition on when it is activated.
    /// </summary>
    public TimeSpan TransitionOnTime
    {
      get { return transitionOnTime; }
      protected set { transitionOnTime = value; }
    }

    /// <summary>
    /// Indicates how long the screen takes to
    /// transition off when it is deactivated.
    /// </summary>
    public TimeSpan TransitionOffTime
    {
      get { return transitionOffTime; }
      protected set { transitionOffTime = value; }
    }

    /// <summary>
    /// Gets the current position of the screen transition, ranging
    /// from zero (fully active, no transition) to one (transitioned
    /// fully off to nothing).
    /// </summary>
    /// <value>
    /// The transition position.
    /// </value>
    public float TransitionPosition
    {
      get { return transitionPosition; }
      protected set { transitionPosition = value; }
    }

    /// <summary>
    /// Gets the current alpha of the screen transition, ranging
    /// from 255 (fully active, no transition) to 0 (transitioned
    /// fully off to nothing).
    /// </summary>
    public byte TransitionAlpha
    {
      get { return (byte)(255 - (TransitionPosition * 255)); }
    }

    /// <summary>
    /// Gets the current screen transition state.
    /// </summary>
    /// <value>
    /// The state of the screen.
    /// </value>
    public ScreenState ScreenState
    {
      get { return screenState; }
      protected set { screenState = value; }
    }

    /// <summary>
    /// There are two possible reasons why a screen might be transitioning
    /// off. It could be temporarily going away to make room for another
    /// screen that is on top of it, or it could be going away for good.
    /// This property indicates whether the screen is exiting for real:
    /// if set, the screen will automatically remove itself as soon as the
    /// transition finishes.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is exiting; otherwise, <c>false</c>.
    /// </value>
    public bool IsExiting
    {
      get { return isExiting; }
      protected internal set { isExiting = value; }
    }

    /// <summary>
    /// Checks whether this screen is active and can respond to user input.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
    /// </value>
    public bool IsActive
    {
      get
      {
        return (!otherScreenHasFocus &&
               screenState == ScreenState.TransitionOn) ||
               screenState == ScreenState.Active;
      }
    }

    /// <summary>
    /// Gets the manager that this screen belongs to.
    /// </summary>
    public ScreenManager ScreenManager
    {
      get { return screenManager; }
      internal set { screenManager = value; }
    }

    /// <summary>
    /// Gets the graphics manager.
    /// </summary>
    public IBallerburgGraphicsManager GraphicsManager
    {
      get { return graphicsManager; }
    }

    /// <summary>
    /// Gets the controls container.
    /// </summary>
    public Collection<Control> ControlsContainer
    {
      get { return controls; }
    }

    /*
    public Windows.ApplicationModel.Resources.ResourceLoader ResourceLoader
    {
      get { return this.resourceLoader; }
    }
    */

    #endregion

    #region Initialization

    /// <summary>
    /// Load graphics content for the screen.
    /// </summary>
    public virtual void LoadContent()
    {
    }

    /// <summary>
    /// Unload content for the screen.
    /// </summary>
    public virtual void UnloadContent()
    {
    }

    #endregion

    #region Update and Draw

    /// <summary>
    /// Allows the screen to run logic, such as updating the transition position.
    /// Unlike HandleInput, this method is called regardless of whether the screen
    /// is active, hidden, or in the middle of a transition.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    /// <param name="otherScreenHasFocus">if set to <c>true</c> [other screen has focus].</param>
    /// <param name="coveredByOtherScreen">if set to <c>true</c> [covered by other screen].</param>
    public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
      this.otherScreenHasFocus = otherScreenHasFocus;

      if (isExiting)
      {
        // If the screen is going away to die, it should transition off.
        screenState = ScreenState.TransitionOff;

        if (!UpdateTransition(gameTime, transitionOffTime, 1))
        {
          // When the transition finishes, remove the screen.
          ScreenManager.RemoveScreen(this);
        }
      }
      else if (coveredByOtherScreen)
      {
        // If the screen is covered by another, it should transition off.
        if (UpdateTransition(gameTime, transitionOffTime, 1))
        {
          // Still busy transitioning.
          screenState = ScreenState.TransitionOff;
        }
        else
        {
          // Transition finished!
          screenState = ScreenState.Hidden;
        }
      }
      else
      {
        // Otherwise the screen should transition on and become active.
        if (UpdateTransition(gameTime, transitionOnTime, -1))
        {
          // Still busy transitioning.
          screenState = ScreenState.TransitionOn;
        }
        else
        {
          // Transition finished!

          // Otherwise the screen should transition on and become active.
          transitionPosition = 0;

          if ((screenState == ScreenState.Hidden) && (ScreenActivated != null))
          {
            ScreenActivated(this, EventArgs.Empty);
          }

          screenState = ScreenState.Active;

          foreach (Control c in controls)
          {
            c.Update(gameTime);
          }
        }
      }
    }

    /// <summary>
    /// Allows the screen to handle user input. Unlike Update, this method
    /// is only called when the screen is active, and not when some other
    /// screen has taken the focus.
    /// </summary>
    /// <param name="input">The input.</param>
    public virtual void HandleInput(InputState input)
    {
    }

    /// <summary>
    /// This is called when the screen should draw itself.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public virtual void Draw(GameTime gameTime)
    {
      if (ScreenState == ScreenState.Active)
      {
        foreach (Control c in controls)
        {
          if (c.Visible)
          {
            c.Draw(gameTime);
          }
        }
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
    /// instantly kills the screen, this method respects the transition timings
    /// and will give the screen a chance to gradually transition off.
    /// </summary>
    public void ExitScreen()
    {
      if (TransitionOffTime == TimeSpan.Zero)
      {
        // If the screen has a zero transition time, remove it immediately.
        ScreenManager.RemoveScreen(this);
      }
      else
      {
        // Otherwise flag that it should transition off and then exit.
        isExiting = true;
      }
    }

    /// <summary>
    /// Aktiviert das Control
    /// </summary>
    public virtual void Activate()
    {
      ////IsActive = true;

      if (ScreenActivated != null)
      {
        ScreenActivated(this, EventArgs.Empty);
      }
    }

    /// <summary>
    /// Deaktiviert das Control
    /// </summary>
    public virtual void Deactivate()
    {
      ////IsActive = false;

      if (ScreenActivated != null)
      {
        ScreenActivated(this, EventArgs.Empty);
      }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Helper for updating the screen transition position.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    /// <param name="time">The time passed.</param>
    /// <param name="direction">The direction.</param>
    /// <returns>True, if the screen is not transitioning any more</returns>
    protected bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
    {
      // How much should we move by?
      float transitionDelta;

      if (time == TimeSpan.Zero)
      {
        transitionDelta = 1;
      }
      else
      {
        transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);
      }

      // Update the transition position.
      transitionPosition += transitionDelta * direction;

      // Did we reach the end of the transition?
      if (((direction < 0) && (transitionPosition <= 0)) ||
          ((direction > 0) && (transitionPosition >= 1)))
      {
        transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
        return false;
      }

      // Otherwise we are still busy transitioning.
      return true;
    }

    #endregion
  }
}
