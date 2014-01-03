// <copyright file="Camera.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Graphic
{
  using System;
  using Manager;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  /// <summary>
  /// The camera may either be in Castle, Tower or Cannon view state
  /// </summary>
  public enum CameraState
  {
    /// <summary>
    /// Camera looks at castle
    /// </summary>
    CastleView,

    /// <summary>
    /// Camera is on top of tower
    /// </summary>
    Tower,

    /// <summary>
    /// Camera is behind cannon
    /// </summary>
    Cannon,

    /// <summary>
    /// Camera is controlled by a script, not by user
    /// </summary>
    Animated
  }

  /// <summary>
  /// The class representing the camera with its 3 states for the Game itself
  /// </summary>
  public class Camera
  {
    private const float ViewAngle = MathHelper.Pi / 4.0f;

    // Distance from the camera of the near and far clipping planes
    private const float NearClip = 0.5f;
    private const float FarClip = 2000.0f;

    private readonly IBallerburgGraphicsManager graphicsManager;

    private Vector3 towerCenter;

    // We have a view
    private Matrix view;

    // and a projection matrice with the camera
    private Matrix proj;
    private Viewport viewPort;

    // Position of the Camera in world space, for our view matrix
    private Vector3 cameraPosition;

    private Quaternion cameraOrientation;
    private float yaw;
    private float pitch;

    // The direction the camera points without rotation.
    private Vector3 castleCameraReference = new Vector3(0, 0, 0);

    // The camera state, avatar's center, first-person, third-person.
    private CameraState cameraState = CameraState.CastleView;

    // Variables for Bezier Curve movement
    private float bezTime = 1.0f;
    private Vector3 bezStartPosition;
    private Vector3 bezMidPosition;
    private Vector3 bezEndPosition;
    private Vector3 bezStartTarget;
    private Vector3 bezEndTarget;
    private CameraState endCameraState;

    /// <summary>
    /// Initializes a new instance of the <see cref="Camera"/> class.
    /// </summary>
    /// <param name="graphicsManager">The graphics manager.</param>
    public Camera(IBallerburgGraphicsManager graphicsManager)
    {
      this.graphicsManager = graphicsManager;

      viewPort = graphicsManager.GraphicsDevice.Viewport;
    }

    /// <summary>
    /// Gets or sets the tower center.
    /// </summary>
    /// <value>
    /// The tower center.
    /// </value>
    public Vector3 TowerCenter
    {
      get { return towerCenter; }
      set { towerCenter = value; }
    }

    /// <summary>
    /// Gets or sets the view direction.
    /// </summary>
    /// <value>
    /// The view direction.
    /// </value>
    public Vector3 ViewDirection
    {
      get { return castleCameraReference; }
      set { castleCameraReference = value; }
    }

    /// <summary>
    /// Gets or sets the yaw.
    /// </summary>
    /// <value>
    /// The yaw value.
    /// </value>
    public float Yaw
    {
      get { return yaw; }
      set { yaw = value; }
    }

    /// <summary>
    /// Gets or sets the pitch.
    /// </summary>
    /// <value>
    /// The pitch.
    /// </value>
    public float Pitch
    {
      get { return pitch; }
      set { pitch = value; }
    }

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    /// <value>
    /// The state.
    /// </value>
    public CameraState State
    {
      get { return cameraState; }
      set { cameraState = value; }
    }

    /// <summary>
    /// Gets the view matrix.
    /// </summary>
    /// <value>
    /// The view matrix.
    /// </value>
    public Matrix ViewMatrix
    {
      get
      {
        return view;
      }

      private set
      {
        view = value;
      }
    }

    /// <summary>
    /// Gets the proj matrix.
    /// </summary>
    /// <value>
    /// The proj matrix.
    /// </value>
    public Matrix ProjMatrix
    {
      get
      {
        return proj;
      }

      private set
      {
        proj = value;
      }
    }

    /// <summary>
    /// Gets or sets the position.
    /// </summary>
    /// <value>
    /// The position.
    /// </value>
    public Vector3 Position
    {
      get { return cameraPosition; }
      set { cameraPosition = value; }
    }

    /// <summary>
    /// The update function for the fixed camera state
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="camYaw">The yaw used.</param>
    /// <param name="camPitch">The pitch used.</param>
    public void UpdateCamera(Vector3 position, float camYaw, float camPitch)
    {
      // Calculate the camera's current position.
      cameraPosition = position;
      yaw += camYaw;
      pitch += camPitch;

      if (cameraState == CameraState.Cannon)
      {
        if (pitch > 0.0f)
        {
          pitch = 0.0f;
        }
      }

      Matrix translation;

      // Calculate the Matrix for rotation arround the Y axis
      var rotationMatrixY = Matrix.CreateRotationY(camYaw);

      // Calculate the Matrix for rotation arround the z axis
      // First, calculate the axis
      Matrix rotationMatrixZ;

      if (Math.Abs(pitch) < (Math.PI / 2.0f) - 0.2)
      {
        var verticalRotationAxis = Vector3.Normalize(Vector3.Cross(Vector3.Normalize(castleCameraReference - position), Vector3.Up));
        rotationMatrixZ = Matrix.CreateFromAxisAngle(verticalRotationAxis, camPitch);
      }
      else
      {
        rotationMatrixZ = Matrix.Identity;
        pitch = Math.Sign(pitch) * (float)((Math.PI / 2.0f) - 0.2);
      }

      // In castle view, we rotate arround the camera's referencepoint
      if (cameraState == CameraState.CastleView)
      {
        translation = Matrix.CreateTranslation(Vector3.Negate(castleCameraReference));

        cameraPosition = Vector3.Transform(cameraPosition, translation * rotationMatrixY * rotationMatrixZ * Matrix.Invert(translation));
      }
      else if (cameraState == CameraState.Tower)
      {
        // In Tower and cannon view, we rotate arround the position
        translation = Matrix.CreateTranslation(Vector3.Negate(cameraPosition));

        // Create a vector pointing the direction the camera is facing.
        castleCameraReference = Vector3.Transform(
            castleCameraReference,
            translation * rotationMatrixY * rotationMatrixZ * Matrix.Invert(translation));
      }
      else
      {
        // camerastate == 2
        var translationCenter = Matrix.CreateTranslation(Vector3.Negate(towerCenter));

        cameraPosition = Vector3.Transform(
            cameraPosition,
            translationCenter * rotationMatrixY * Matrix.Invert(translationCenter));
        castleCameraReference = Vector3.Transform(
            castleCameraReference,
            translationCenter * rotationMatrixY * Matrix.Invert(translationCenter));
      }

      // Set up view matrix and projection matrix.
      view = Matrix.CreateLookAt(cameraPosition, castleCameraReference, new Vector3(0.0f, 1.0f, 0.0f));

      var aspectRatio = viewPort.Width / (float)viewPort.Height;

      proj = Matrix.CreatePerspectiveFieldOfView(ViewAngle, aspectRatio, NearClip, FarClip);
    }

    /// <summary>
    /// The update function for the first person camera
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="position">The position.</param>
    /// <param name="camYaw">The cam yaw.</param>
    /// <param name="camPitch">The cam pitch.</param>
    /// <param name="roll">The roll angle.</param>
    public void UpdateCameraFirstPerson(Vector3 offset, Vector3 position, float camYaw, float camPitch, float roll)
    {
      var rotationMatrixX = Matrix.CreateRotationX(camPitch);
      var rotationMatrixY = Matrix.CreateRotationY(camYaw);
      var rotationMatrixZ = Matrix.Identity;

      var rotationMatrix = rotationMatrixX * rotationMatrixY * rotationMatrixZ;

      Quaternion.CreateFromRotationMatrix(ref rotationMatrix, out cameraOrientation);

      cameraOrientation.Normalize();

      // Calculate the camera's current position.
      cameraPosition = position + offset;

      Update();
    }

    /// <summary>
    /// The update function for the third person camera
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="camYaw">The cam yaw.</param>
    public void UpdateCameraThirdPerson(Vector3 position, float camYaw)
    {
      // Set up view matrix and projection matrix
      view = Matrix.CreateLookAt(cameraPosition, position, new Vector3(0.0f, 1.0f, 0.0f));

      viewPort = graphicsManager.GraphicsDevice.Viewport;

      var aspectRatio = (float)viewPort.Width / viewPort.Height;

      proj = Matrix.CreatePerspectiveFieldOfView(ViewAngle, aspectRatio, NearClip, FarClip);
    }

    /// <summary>
    /// Rotates the camera around axis.
    /// </summary>
    /// <param name="axis">The axis to rotate.</param>
    /// <param name="angle">The angle to be rotated.</param>
    public void RotateCameraAroundAxis(Vector3 axis, float angle)
    {
      axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(cameraOrientation));
      cameraOrientation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, angle) * cameraOrientation);
    }

    /// <summary>
    /// Rotates the specified axis.
    /// </summary>
    /// <param name="axis">The axis used.</param>
    /// <param name="angle">The angle used.</param>
    public void Rotate(Vector3 axis, float angle)
    {
      axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(cameraOrientation));
      cameraOrientation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, angle) * cameraOrientation);
    }

    /// <summary>
    /// Translates the specified distance.
    /// </summary>
    /// <param name="distance">The distance.</param>
    public void Translate(Vector3 distance)
    {
      cameraPosition += Vector3.Transform(distance, Matrix.CreateFromQuaternion(cameraOrientation));
      Update();
    }

    /// <summary>
    /// Revolves the specified target.
    /// </summary>
    /// <param name="target">The target use.</param>
    /// <param name="axis">The axis used.</param>
    /// <param name="angle">The angle used.</param>
    public void Revolve(Vector3 target, Vector3 axis, float angle)
    {
      Rotate(axis, angle);
      axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(cameraOrientation));
      var rotate = Quaternion.CreateFromAxisAngle(axis, angle);
      cameraPosition = Vector3.Transform(target - cameraPosition, Matrix.CreateFromQuaternion(rotate));
      Update();
    }

    /// <summary>
    /// Inits the bezier.
    /// </summary>
    /// <param name="startPosition">The start position.</param>
    /// <param name="startTarget">The start target.</param>
    /// <param name="endPosition">The end position.</param>
    /// <param name="endTarget">The end target.</param>
    /// <param name="endState">The end state.</param>
    public void InitBezier(Vector3 startPosition, Vector3 startTarget, Vector3 endPosition, Vector3 endTarget, CameraState endState)
    {
      bezStartPosition = startPosition;
      bezEndPosition = endPosition;

      bezMidPosition = (bezStartPosition + bezEndPosition) / 2.0f;

      var cameraDirection = endPosition - startPosition;
      var targDirection = endTarget - startTarget;
      var upVector = Vector3.Cross(new Vector3(targDirection.X, 0, targDirection.Z), new Vector3(cameraDirection.X, 0, cameraDirection.Z));
      var perpDirection = Vector3.Cross(upVector, cameraDirection);

      if (perpDirection == new Vector3())
      {
        perpDirection = new Vector3(0, 1, 0);
      }

      perpDirection.Normalize();

      var midShiftDirecton = new Vector3(0, 1, 0) + perpDirection;
      bezMidPosition += cameraDirection.Length() * midShiftDirecton;

      bezStartTarget = startTarget;
      bezEndTarget = endTarget;

      bezTime = 0.0f;

      endCameraState = endState;
      cameraState = CameraState.Animated;
    }

    /// <summary>
    /// Updates the bezier.
    /// </summary>
    public void UpdateBezier()
    {
      bezTime += 0.01f;

      if (bezTime > 1.0f)
      {
        if (cameraState == CameraState.Animated)
        {
          cameraState = endCameraState;
        }

        return;
      }

      var smoothValue = MathHelper.SmoothStep(0, 1, bezTime);
      var newCamPos = Bezier(bezStartPosition, bezMidPosition, bezEndPosition, smoothValue);
      var newCamTarget = Vector3.Lerp(bezStartTarget, bezEndTarget, smoothValue);

      float updownRot;
      float leftrightRot;
      AnglesFromDirection(newCamTarget - newCamPos, out updownRot, out leftrightRot);

      UpdateCamera(newCamPos, 0.0f, 0.0f);
    }

    /// <summary>
    /// Beziers the specified start point.
    /// </summary>
    /// <param name="startPoint">The start point.</param>
    /// <param name="midPoint">The mid point.</param>
    /// <param name="endPoint">The end point.</param>
    /// <param name="time">The time value.</param>
    /// <returns>The calculated Bezier Vector</returns>
    private static Vector3 Bezier(Vector3 startPoint, Vector3 midPoint, Vector3 endPoint, float time)
    {
      var invTime = 1.0f - time;
      var timePow = (float)Math.Pow(time, 2);
      var invTimePow = (float)Math.Pow(invTime, 2);

      var result = startPoint * invTimePow;
      result += 2 * midPoint * time * invTime;
      result += endPoint * timePow;

      return result;
    }

    /// <summary>
    /// Angleses from direction.
    /// </summary>
    /// <param name="direction">The direction.</param>
    /// <param name="updownAngle">The updown angle.</param>
    /// <param name="leftrightAngle">The leftright angle.</param>
    private static void AnglesFromDirection(Vector3 direction, out float updownAngle, out float leftrightAngle)
    {
      var floorProjection = new Vector3(direction.X, 0, direction.Z);
      var directionLength = floorProjection.Length();
      updownAngle = (float)Math.Atan2(direction.Y, directionLength);
      leftrightAngle = -(float)Math.Atan2(direction.X, -direction.Z);
    }

    /// <summary>
    /// Generic update function
    /// </summary>
    private void Update()
    {
      viewPort = graphicsManager.GraphicsDevice.Viewport;
      var aspectRatio = viewPort.Width / (float)viewPort.Height;

      proj = Matrix.CreatePerspectiveFieldOfView(ViewAngle, aspectRatio, NearClip, FarClip);
    }
  }
}
