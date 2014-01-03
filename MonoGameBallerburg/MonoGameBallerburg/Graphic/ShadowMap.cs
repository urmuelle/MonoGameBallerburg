// <copyright file="ShadowMap.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Graphic
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /*
    public class ShadowMap
    {
        // The size of the shadow map
        // The larger the size the more detail we will have for our entire scene
        const int shadowMapWidthHeight = 2048;

        // Light direction
        Vector3 lightDir = new Vector3(-0.3333333f, 0.6666667f, 0.6666667f);
        BoundingFrustum cameraFrustum = new BoundingFrustum(Matrix.Identity);
        // The shadow map render target
        RenderTarget2D shadowRenderTarget;
        private SpriteBatch spriteBatch;        
        private ScreenManager screenManager;
        private Camera camera;

        public ShadowMap(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, ScreenManager screenManager, Camera camera)
        {
            this.spriteBatch = spriteBatch;            
            this.screenManager = screenManager;
            this.camera = camera;
            this.shadowRenderTarget = new RenderTarget2D(BallerburgGame.Instance.GraphicsDevice,
                                                    shadowMapWidthHeight,
                                                    shadowMapWidthHeight,
                                                    false,
                                                    SurfaceFormat.Single,
                                                    DepthFormat.Depth24);
        }

        /// <summary>
        /// Creates the WorldViewProjection matrix from the perspective of the 
        /// light using the cameras bounding frustum to determine what is visible 
        /// in the scene.
        /// </summary>
        /// <returns>The WorldViewProjection for the light</returns>
        public Matrix CreateLightViewProjectionMatrix()
        {
            // Matrix with that will rotate in points the direction of the light
            Matrix lightRotation = Matrix.CreateLookAt(Vector3.Zero,
                                                       -lightDir,
                                                       Vector3.Up);

            // Get the corners of the frustum
            Vector3[] frustumCorners = cameraFrustum.GetCorners();

            // Transform the positions of the corners into the direction of the light
            for (int i = 0; i < frustumCorners.Length; i++)
            {
                frustumCorners[i] = Vector3.Transform(frustumCorners[i], lightRotation);
            }

            // Find the smallest box around the points
            BoundingBox lightBox = BoundingBox.CreateFromPoints(frustumCorners);

            Vector3 boxSize = lightBox.Max - lightBox.Min;
            Vector3 halfBoxSize = boxSize * 0.5f;

            // The position of the light should be in the center of the back
            // pannel of the box. 
            Vector3 lightPosition = lightBox.Min + halfBoxSize;
            lightPosition.Z = lightBox.Min.Z;

            // We need the position back in world coordinates so we transform 
            // the light position by the inverse of the lights rotation
            lightPosition = Vector3.Transform(lightPosition,
                                              Matrix.Invert(lightRotation));

            // Create the view matrix for the light
            Matrix lightView = Matrix.CreateLookAt(lightPosition,
                                                   lightPosition - lightDir,
                                                   Vector3.Up);

            // Create the projection matrix for the light
            // The projection is orthographic since we are using a directional light
            Matrix lightProjection = Matrix.CreateOrthographic(boxSize.X, boxSize.Y,
                                                               -boxSize.Z, boxSize.Z);

            return lightView * lightProjection;
        }

        /// <summary>
        /// Renders the scene to the floating point render target then 
        /// sets the texture for use when drawing the scene.
        /// </summary>
        public void CreateShadowMap(Effect shaderEffect)
        {
            // Set our render target to our floating point render target
            BallerburgGame.Instance.GraphicsDevice.SetRenderTarget(shadowRenderTarget);

            // Clear the render target to white or all 1's
            // We set the clear to white since that represents the 
            // furthest the object could be away
            BallerburgGame.Instance.GraphicsDevice.Clear(Color.White);

            // Draw any occluders in our case that is all the scene except the ground plane

            foreach (Graphic.Cannonball cannonBall in this.screenManager.GameObjectManager.ActiveCannonballs)
            {
                if (cannonBall.Alive)
                {
                    cannonBall.Draw(new GameTime(), this.camera.ViewMatrix, this.camera.ProjMatrix);
                }
            }            
            
            foreach (Gameplay.PlayerSettings ps in BallerburgGame.PlayerSettings)
            {
                if (ps.IsActive)
                {
                    for (int i = 0; i < ps.Castle.Towers.Count; i++)
                    {
                        this.screenManager.GameObjectManager.Castles[ps.Castle.CastleType].SelectTower(i);
                        this.screenManager.GameObjectManager.Castles[ps.Castle.CastleType].CurrentTower.HasCannon = ps.Castle.Towers[i].HasCannon;
                        this.screenManager.GameObjectManager.Castles[ps.Castle.CastleType].CurrentTower.TowerCannon = ps.Castle.Towers[i].TowerCannon;
                    }

                    this.screenManager.GameObjectManager.Castles[ps.Castle.CastleType].Position = ps.Castle.StartPos;
                    this.screenManager.GameObjectManager.Castles[ps.Castle.CastleType].Draw(new GameTime(), this.camera.ViewMatrix, this.camera.ProjMatrix, shaderEffect);
                }
            }

            // Set render target back to the back buffer
            BallerburgGame.Instance.GraphicsDevice.SetRenderTarget(null);
        }

        /// <summary>
        /// Renders the scene using the shadow map to darken the shadow areas
        /// </summary>
        void DrawWithShadowMap()
        {
            BallerburgGame.Instance.GraphicsDevice.Clear(Color.CornflowerBlue);

            BallerburgGame.Instance.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;

            /*
            // Draw the grid
            world = Matrix.Identity;
            DrawModel(gridModel, false);

            // Draw the dude model
            world = Matrix.CreateRotationY(MathHelper.ToRadians(rotateDude));
            DrawModel(dudeModel, false);
             * 
        }

        /// <summary>
        /// Helper function to draw a model
        /// </summary>
        /// <param name="model">The model to draw</param>
        /// <param name="technique">The technique to use</param>
        void DrawModel(Model model, bool createShadowMap)
        {
            string techniqueName = createShadowMap ? "CreateShadowMap" : "DrawWithShadowMap";

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            /*
            // Loop over meshs in the model
            foreach (ModelMesh mesh in model.Meshes)
            {
                // Loop over effects in the mesh
                foreach (Effect effect in mesh.Effects)
                {
                    // Set the currest values for the effect
                    effect.CurrentTechnique = effect.Techniques[techniqueName];
                    effect.Parameters["World"].SetValue(world);
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["LightDirection"].SetValue(lightDir);
                    effect.Parameters["LightViewProj"].SetValue(lightViewProjection);

                    if (!createShadowMap)
                        effect.Parameters["ShadowMap"].SetValue(shadowRenderTarget);
                }
                // Draw the mesh
                mesh.Draw();
            }
             * 
        }

        /// <summary>
        /// Render the shadow map texture to the screen
        /// </summary>
        public void DrawShadowMapToScreen()
        {
            spriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
            spriteBatch.Draw(shadowRenderTarget, new Rectangle(0, 0, 128, 128), Color.White);
            spriteBatch.End();

            BallerburgGame.Instance.GraphicsDevice.Textures[0] = null;
            BallerburgGame.Instance.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }
    }
     * */
}