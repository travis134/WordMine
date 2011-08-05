using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WordMine
{
    class SpriteStrip
    {
        public SpriteStripId id;
        public Rectangle frameSize;

        public int numberOfFramesTotal;

        public int numberOfFramesIn;
        public int numberOfFramesLoop;
        public int numberOfFramesOut;

        public int framesPerSecondIn;
        public int framesPerSecondLoop;
        public int framesPerSecondOut;

        public Point initialIn;
        public Point terminalIn;
        public Point initialLoop;
        public Point terminalLoop;
        public Point initialOut;
        public Point terminalOut;

        public int numberOfLoops;

        public Point spriteSheetSize;

        public Boolean finished;

        public SpriteStrip(SpriteStripId id, Rectangle frameSize, Point initialLoop, Point terminalLoop, Point spriteSheetSize, int framesPerSecondLoop, int numberOfLoops)
        {
            this.id = id;
            this.frameSize = frameSize;
            this.initialLoop = initialLoop;
            this.terminalLoop = terminalLoop;
            this.spriteSheetSize = spriteSheetSize;

            this.numberOfFramesLoop = this.countFrames(this.initialLoop, this.terminalLoop);
            this.numberOfFramesTotal = this.numberOfFramesLoop;

            this.framesPerSecondLoop = framesPerSecondLoop;

            this.numberOfLoops = numberOfLoops;

            this.finished = false;
        }

        private int countFrames(Point initial, Point terminal)
        {
            int numberOfFrames;
            numberOfFrames = (this.spriteSheetSize.X - initial.X);
            for (int i = 0; i < ((terminal.Y - initial.Y) - 1); i++)
            {
                numberOfFrames += this.spriteSheetSize.X;
            }
            numberOfFrames += terminal.X + 1;
            return numberOfFrames;
        }
    }
}
