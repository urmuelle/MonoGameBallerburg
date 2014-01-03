// <copyright file="CannonSettings.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Gameplay
{
  using System;
  using Microsoft.Xna.Framework;

  /// <summary>
  /// Class containing all information for cannons
  /// </summary>
  public class CannonSettings
  {
    public const float MinElevation = (15.0f / 180.0f) * (float)Math.PI;
    public const float MaxElevation = (90.0f / 180.0f) * (float)Math.PI;
    public const float TubeLenght = 1.0f;

    // Information stored for the camera
    private Vector3 viewDirection;
    private Vector3 cameraPosition;

    // Information stored for the game
    private int powderLoad;

    private Vector3 tubeDirection;
    private Vector3 tubeStartDirection;
    private Vector3 cannonPosition;
    private Vector3 bulletStartPosition;
    private float rotationAngleYaw;
    private float rotationAnglePitch;

    /// <summary>
    /// Initializes a new instance of the <see cref="CannonSettings"/> class.
    /// </summary>
    /// <param name="x">The x position.</param>
    /// <param name="y">The y position.</param>
    /// <param name="z">The z position.</param>
    public CannonSettings(float x, float y, float z)
    {
      this.cannonPosition = new Vector3(x, y, z);

      this.tubeStartDirection = new Vector3(0.0f, 0.0f, 1.0f);
      this.TubeDirection = this.tubeStartDirection;
      this.tubeDirection.Normalize();

      this.rotationAngleYaw = 0.0f;
      this.rotationAnglePitch = 0.0f;

      this.bulletStartPosition = (TubeLenght * this.tubeDirection) + this.CannonPosition + new Vector3(0.0f, 0.5f, 0.0f);

      // Camera position and view direction for the cannon
      this.cameraPosition = new Vector3(x, y + 1.5f, z - 1.0f);
      this.viewDirection = this.cameraPosition + this.tubeDirection;
    }

    /// <summary>
    /// Gets or sets the rotation angle yaw.
    /// </summary>
    /// <value>
    /// The rotation angle yaw.
    /// </value>
    public float RotationAngleYaw
    {
      get { return this.rotationAngleYaw; }
      set { this.rotationAngleYaw = value; }
    }

    /// <summary>
    /// Gets or sets the rotation angle pitch.
    /// </summary>
    /// <value>
    /// The rotation angle pitch.
    /// </value>
    public float RotationAnglePitch
    {
      get { return this.rotationAnglePitch; }
      set { this.rotationAnglePitch = value; }
    }

    /// <summary>
    /// Gets or sets the view direction.
    /// </summary>
    /// <value>
    /// The view direction.
    /// </value>
    public Vector3 ViewDirection
    {
      get { return this.viewDirection; }
      set { this.viewDirection = value; }
    }

    /// <summary>
    /// Gets or sets the camera position.
    /// </summary>
    /// <value>
    /// The camera position.
    /// </value>
    public Vector3 CameraPosition
    {
      get { return this.cameraPosition; }
      set { this.cameraPosition = value; }
    }

    /// <summary>
    /// Gets or sets the powder load.
    /// </summary>
    /// <value>
    /// The powder load.
    /// </value>
    public int PowderLoad
    {
      get { return this.powderLoad; }
      set { this.powderLoad = value; }
    }

    /// <summary>
    /// Gets or sets the tube direction.
    /// </summary>
    /// <value>
    /// The tube direction.
    /// </value>
    public Vector3 TubeDirection
    {
      get
      {
        return this.tubeDirection;
      }

      set
      {
        this.tubeDirection = value;

        // Set the new bullet start position
        this.bulletStartPosition = (TubeLenght * Vector3.Normalize(this.tubeDirection)) + this.CannonPosition + new Vector3(0.0f, 0.5f, 0.0f);
      }
    }

    /// <summary>
    /// Gets or sets the cannon position.
    /// </summary>
    /// <value>
    /// The cannon position.
    /// </value>
    public Vector3 CannonPosition
    {
      get { return this.cannonPosition; }
      set { this.cannonPosition = value; }
    }

    /// <summary>
    /// Gets the initial Position if a cannonball is shot.
    /// </summary>
    public Vector3 BulletStartPosition
    {
      get { return this.bulletStartPosition; }
    }
  }
}
