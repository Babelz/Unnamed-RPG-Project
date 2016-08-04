using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine
{
    public sealed class FrameCounter
    {
        #region Constants
        public const int MaxSamples = 100;
        #endregion

        #region Fields
        private Queue<float> samples = new Queue<float>();
        #endregion

        #region Properties
        public long TotalFrames
        {
            get; private set;
        }
        public float TotalSeconds
        {
            get; private set;
        }
        public float AverageFramesPerSecond
        {
            get; private set;
        }
        public float CurrentFramesPerSecond
        {
            get; private set;
        }
        #endregion

        public FrameCounter()
        {
        }

        public void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            CurrentFramesPerSecond = 1.0f / deltaTime;

            samples.Enqueue(CurrentFramesPerSecond);

            if (samples.Count > MaxSamples)
            {
                samples.Dequeue();
                AverageFramesPerSecond = samples.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += deltaTime;
        }
    }
}
