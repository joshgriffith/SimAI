using System;
using System.Drawing;
using System.Threading;

namespace SimAI.Core.Automation {
    public class MouseCursor {

        public void MoveMouse(int x, int y) {
            var width = 1920;
            var height = 1080;

            x = (int)((float)x / (float)100 * (float)width);
            y = (int)((float)y / (float)100 * (float)height);

            var steps = 50;
            var newPosition = new MouseOperations.MousePoint(x, y);
            var start = MouseOperations.GetCursorPosition();
            var iterPoint = start;
            
            PointF slope = new PointF(newPosition.X - start.X, newPosition.Y - start.Y);
            
            slope.X = (float) Math.Round(slope.X / steps);
            slope.Y = (float) Math.Round(slope.Y / steps);
            
            for (int i = 0; i < steps; i++) {
                iterPoint = new MouseOperations.MousePoint((int)iterPoint.X + (int)slope.X, (int)iterPoint.Y + (int)slope.Y);

                MouseOperations.SetCursorPosition(iterPoint);
                Thread.Sleep(10);
            }
            
            MouseOperations.SetCursorPosition(newPosition);
        }
    }
}
