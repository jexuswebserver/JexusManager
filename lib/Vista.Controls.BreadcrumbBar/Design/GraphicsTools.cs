using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Vista.Controls.Design
{
    /// <summary>
    /// Provides tools for graphics processing
    /// </summary>
    /// <remarks>
    /// 2007 José Manuel Menéndez Poo
    /// Visit my blog for upgrades and other renderers - www.menendezpoo.com
    /// </remarks>
    internal static class GraphicsTools
    {
        /// <summary>
        /// Creates a rounded rectangle from the specified rectangle and radius
        /// </summary>
        /// <param name="rectangle">Base rectangle</param>
        /// <param name="radius">Radius of the corners</param>
        /// <returns>Rounded rectangle as a GraphicsPath</returns>
        public static GraphicsPath CreateRoundRectangle(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            int l = rectangle.Left;
            int t = rectangle.Top;
            int w = rectangle.Width;
            int h = rectangle.Height;
            int d = radius << 1;

            path.AddArc(l, t, d, d, 180, 90); // topleft
            path.AddLine(l + radius, t, l + w - radius, t); // top
            path.AddArc(l + w - d, t, d, d, 270, 90); // topright
            path.AddLine(l + w, t + radius, l + w, t + h - radius); // right
            path.AddArc(l + w - d, t + h - d, d, d, 0, 90); // bottomright
            path.AddLine(l + w - radius, t + h, l + radius, t + h); // bottom
            path.AddArc(l, t + h - d, d, d, 90, 90); // bottomleft
            path.AddLine(l, t + h - radius, l, t + radius); // left
            path.CloseFigure();

            return path;
        }

        /// <summary>
        /// Creates a rectangle rounded on the top
        /// </summary>
        /// <param name="rectangle">Base rectangle</param>
        /// <param name="radius">Radius of the top corners</param>
        /// <returns>Rounded rectangle (on top) as a GraphicsPath object</returns>
        public static GraphicsPath CreateTopRoundRectangle(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            int l = rectangle.Left;
            int t = rectangle.Top;
            int w = rectangle.Width;
            int h = rectangle.Height;
            int d = radius << 1;

            path.AddArc(l, t, d, d, 180, 90); // topleft
            path.AddLine(l + radius, t, l + w - radius, t); // top
            path.AddArc(l + w - d, t, d, d, 270, 90); // topright
            path.AddLine(l + w, t + radius, l + w, t + h); // right
            path.AddLine(l + w, t + h, l, t + h); // bottom
            path.AddLine(l, t + h, l, t + radius); // left
            path.CloseFigure();
            
            return path;
        }

        /// <summary>
        /// Creates a rectangle rounded on the bottom
        /// </summary>
        /// <param name="rectangle">Base rectangle</param>
        /// <param name="radius">Radius of the bottom corners</param>
        /// <returns>Rounded rectangle (on bottom) as a GraphicsPath object</returns>
        public static GraphicsPath CreateBottomRoundRectangle(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            int l = rectangle.Left;
            int t = rectangle.Top;
            int w = rectangle.Width;
            int h = rectangle.Height;
            int d = radius << 1;

            path.AddLine(l + radius, t, l + w - radius, t); // top
            path.AddLine(l + w, t + radius, l + w, t + h - radius); // right
            path.AddArc(l + w - d, t + h - d, d, d, 0, 90); // bottomright
            path.AddLine(l + w - radius, t + h, l + radius, t + h); // bottom
            path.AddArc(l, t + h - d, d, d, 90, 90); // bottomleft
            path.AddLine(l, t + h - radius, l, t + radius); // left
            path.CloseFigure();

            return path;
        }
    }
}
