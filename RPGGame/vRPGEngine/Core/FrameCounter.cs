using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Core
{
    public sealed class FrameCounter
    {
        #region Constants
        private const int MaxSamples = 128;
        #endregion

        #region Fields
        private readonly Queue<float> samples;
        #endregion

        #region Properties
        public long TotalFrames
        {
            get;
            private set;
        }
        public float TotalSeconds
        {
            get;
            private set;
        }
        public float AverageFramesPerSecond
        {
            get;
            private set;
        }
        public float CurrentFramesPerSecond
        {
            get;
            private set;
        }
        #endregion

        public FrameCounter()
        {
            samples = new Queue<float>();
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
