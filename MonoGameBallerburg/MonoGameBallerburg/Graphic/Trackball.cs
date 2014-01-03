// <copyright file="Trackball.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Graphic
{
  using System;
  using Microsoft.Xna.Framework;

  /// <summary>
  ///   Class for trackball usage
  /// </summary>
  public class Trackball
  {
    private int width, height;
    private Vector3 startProjection, currentProjection;
    private Quaternion startRotation, currentRotation;

    /// <summary>
    ///   Initializes a new instance of the <see cref="Trackball" /> class.
    /// </summary>
    public Trackball()
    {
      width = 0;
      height = 0;
      startProjection = Vector3.Zero;
      currentProjection = Vector3.Zero;
      startRotation = Quaternion.Identity;
      currentRotation = Quaternion.Identity;
    }

    /// <summary>
    ///   Gets the quaternion.
    /// </summary>
    /// <value> The Quaternion </value>
    public Quaternion Quaternion
    {
      get
      {
        var theRotation = new Quaternion(
            currentRotation.X,
            currentRotation.Y,
            currentRotation.Z,
            currentRotation.W);

        theRotation.Normalize();
        return theRotation;
      }
    }

    /// <summary>
    ///   Gets the matrix.
    /// </summary>
    /// <value> The rotation Matrix from the quaternion </value>
    public Matrix Matrix
    {
      get
      {
        var quaternionRotation = Quaternion;
        var theMatrix = Matrix.CreateFromQuaternion(quaternionRotation);
        return theMatrix;
      }
    }

    /// <summary>
    ///   Gives the screen coordinates to the trackball
    /// </summary>
    /// <param name="width"> The width. </param>
    /// <param name="height"> The height. </param>
    public void Resize(int width, int height)
    {
      this.width = width;
      this.height = height;
    }

    /// <summary>
    ///   Scale Screencoordinates to Unit sphere coordinates, i.e. Sphere with radius 1, gives rect 0..2, 0..2
    /// </summary>
    /// <param name="mouseX"> The mouse X. </param>
    /// <param name="mouseY"> The mouse Y. </param>
    /// <returns> The Screen NDC </returns>
    public Vector2 ScreenToNdc(int mouseX, int mouseY)
    {
      mouseY = height - mouseY;

      var x = ((2.0f * mouseX) - width) / width;
      var y = ((2.0f * mouseY) - height) / height;

      return new Vector2(x, y);
    }

    /// <summary>
    ///   Project the mouse coordinates onto the sphere
    /// </summary>
    /// <param name="ndc"> The NDC part. </param>
    /// <returns> The projected vector </returns>
    public Vector3 ProjectToSphere(Vector2 ndc)
    {
      var theVector = new Vector3(ndc.X, ndc.Y, 0);
      var theLength = ndc.Length();

      // If the mouse coordinate lies outside the sphere
      // choose the closest point on the sphere by 
      // setting z to zero and renomalizing
      if (theLength >= 1.0f)
      {
        theVector.X /= (float)Math.Sqrt(theLength);
        theVector.Y /= (float)Math.Sqrt(theLength);
      }
      else
      {
        theVector.Z = 1.0f - theLength;
      }

      return theVector;
    }

    /// <summary>
    ///   Gets the quaternion from projections.
    /// </summary>
    /// <param name="from"> From Vector. </param>
    /// <param name="to"> To Vector. </param>
    /// <returns> The quaternion </returns>
    public Quaternion GetQuaternionFromProjections(Vector3 from, Vector3 to)
    {
      var axis = Vector3.Cross(from, to);
      return new Quaternion(axis.X, axis.Y, axis.Z, Vector3.Dot(from, to));
    }

    /// <summary>
    ///   Begins the specified x.
    /// </summary>
    /// <param name="x"> The x coordinate. </param>
    /// <param name="y"> The y coordinate. </param>
    public void Begin(int x, int y)
    {
      Vector2 ndc = ScreenToNdc(x, y);
      startProjection = ProjectToSphere(ndc);
      startRotation = currentRotation;
    }

    /// <summary>
    ///   Updates the specified x.
    /// </summary>
    /// <param name="x"> The x coordinate. </param>
    /// <param name="y"> The y coordinate. </param>
    public void Update(int x, int y)
    {
      var ndc = ScreenToNdc(x, y);
      currentProjection = ProjectToSphere(ndc);
      currentRotation = GetQuaternionFromProjections(startProjection, currentProjection) * startRotation;
    }
  }
}