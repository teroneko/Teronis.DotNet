﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard.Tools
{
    public static class RectangleTools
    {
        public static bool IsRectangleInEllipse(int rectangleX, int rectangleY, int rectangleWidth, int rectangleHeight, int x, int y)
        {
            var relMiddleCenterX = rectangleWidth / 2f;
            var relMiddleCenterY = rectangleHeight / 2f;
            var absMiddleCenterX = rectangleX + relMiddleCenterX;
            var absMiddleCenterY = rectangleY + relMiddleCenterY;
            //
            return PointTools.IsPointInEllipse(absMiddleCenterX, absMiddleCenterY, relMiddleCenterX, relMiddleCenterY, x, y);
        }

        public static bool RectangleContains(int rectangleX, int rectangleY, int rectangleWidth, int rectangleHeight, int x, int y)
        {
            return rectangleX <= x &&
                x < rectangleX + rectangleWidth &&
                rectangleY <= y &&
                y < rectangleY + rectangleHeight;
        }

        /// <summary>
        /// Decision between Rcetangle.Contains() and "Ellipse.Contains()";
        /// </summary>
        /// <param name="rectangleX"></param>
        /// <param name="rectangleY"></param>
        /// <param name="rectangleWidth"></param>
        /// <param name="rectangleHeight"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="inEllipse"></param>
        /// <returns></returns>
        public static bool RectangleContains(int rectangleX, int rectangleY, int rectangleWidth, int rectangleHeight, int x, int y, bool inEllipse)
            => inEllipse ? IsRectangleInEllipse(rectangleX, rectangleY, rectangleWidth, rectangleHeight, x, y) : RectangleContains(rectangleX, rectangleY, rectangleWidth, rectangleHeight, x, y);
    }
}
