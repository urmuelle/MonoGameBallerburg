// <copyright file="AnimationSequence.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Animation
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Class used for texture Animation Sequences
    /// </summary>
    public class AnimationSequence
    {
        private float rotation, scale, depth;
        private Vector2 origin;
        private int framecount;
        private List<AnimationFrame> frames;
        private int curFrame;
        private float totalElapsed;
        private bool paused;
        private bool doLoop;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationSequence"/> class.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="loop">if set to <c>true</c> [loop].</param>
        public AnimationSequence(Vector2 origin, float rotation, float scale, float depth, bool loop)
        {
            this.origin = origin;
            this.rotation = rotation;
            this.scale = scale;
            this.depth = depth;
            framecount = 0;
            doLoop = loop;
            frames = new List<AnimationFrame>();
        }

        /// <summary>
        /// Gets a value indicating whether this instance is paused.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is paused; otherwise, <c>false</c>.
        /// </value>
        public bool IsPaused
        {
            get { return paused; }
        }

        /// <summary>
        /// Adds the frame.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="time">The time passed.</param>
        public void AddFrame(Texture2D texture, float time)
        {
            framecount = framecount + 1;
            frames.Add(new AnimationFrame(texture, time));            
        }

        /// <summary>
        /// Prepares the animation.
        /// </summary>
        public void PrepareAnimation()
        {
            curFrame = 0;
            totalElapsed = 0;
            paused = false;
        }

        /// <summary>
        /// Updates the frame.
        /// </summary>
        /// <param name="elapsed">The elapsed.</param>
        public void UpdateFrame(float elapsed)
        {
            if (paused)
            {
                return;
            }

            totalElapsed += elapsed;
            if (totalElapsed > frames[curFrame].Time)
            {
                totalElapsed -= frames[curFrame].Time;
                curFrame++;

                if (doLoop)
                {
                    // Keep the Frame between 0 and the total frames, minus one.
                    curFrame = curFrame % framecount;
                }
                else
                {
                    if (curFrame == framecount)
                    {
                        Pause();
                        Reset();
                    }
                }
            }
        }

        /// <summary>
        /// Draws the frame.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="screenPos">The screen pos.</param>
        public void DrawFrame(SpriteBatch batch, Vector2 screenPos)
        {
            var frameWidth = frames[curFrame].Texture.Width;
            var sourcerect = new Rectangle(0, 0, frameWidth, frames[curFrame].Texture.Height);
            batch.Draw(frames[curFrame].Texture, screenPos, sourcerect, Color.White, rotation, origin, scale, SpriteEffects.None, depth);
        }        

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            curFrame = 0;
            totalElapsed = 0f;
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            Pause();
            Reset();
        }

        /// <summary>
        /// Plays this instance.
        /// </summary>
        public void Play()
        {
            paused = false;
        }

        /// <summary>
        /// Pauses this instance.
        /// </summary>
        public void Pause()
        {
            paused = true;
        }
    }
}
