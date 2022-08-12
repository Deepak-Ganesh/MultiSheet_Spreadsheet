using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SS
{
    /// <summary>
    /// The SpaceWars panel. Paints the world and its objects.
    /// </summary>
    public class SpreadsheetListPanel : Panel
    {

        // Used to properly display the list of spreadsheets
        private string[] spreadsheets;

        private object theLock;

        /// <summary>
        /// Constructs the drawingpanel.
        /// </summary>
        public SpreadsheetListPanel(object locker)
        {
            DoubleBuffered = true;
            theLock = locker;

            // instantiate this so that whenever onpaint is called before a list is given,
            //  there will be something to iterate over (nothing, as opposed to null)
            spreadsheets = new string[0];
        }

        public void SetSheets(string[] sheets)
        {
            spreadsheets = sheets;
        }

        /// <summary>
        /// Helper method for DrawObjectWithTransform
        /// </summary>
        /// <param name="size">The world (and image) size</param>
        /// <param name="w">The worldspace coordinate</param>
        /// <returns></returns>
        private static int SheetSpaceToImageSpace(int size, double w)
        {
            return (int)w + size / 2;
        }

        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);


        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldSize">The size of one edge of the world (assuming the world is square)</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, int worldSize, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // Perform the transformation
            int x = SheetSpaceToImageSpace(worldSize, worldX);
            int y = SheetSpaceToImageSpace(worldSize, worldY);
            e.Graphics.TranslateTransform(x, y);
            e.Graphics.RotateTransform((float)angle);
            // Draw the object 
            drawer(o, e);
            // Then undo the transformation
            e.Graphics.ResetTransform();
        }

        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void SheetDrawer(object o, PaintEventArgs e, int y)
        {
            string s = o as string;

            Font drawFont = new Font("Comic Sans MS", 12);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            Point drawPoint = new Point(0, y);

            //if (playerID == p.ID)
            //    PaintHighlight(e, drawPoint);

            //e.Graphics.DrawString(drawString, drawFont, drawBrush, drawPoint);    
            DrawBCKG(s, e, drawPoint);
            e.Graphics.DrawString(s, drawFont, drawBrush, drawPoint);
        }

        /// <summary>
        /// Draws the background of the spreadsheet filename.
        /// </summary>
        private void DrawBCKG(string filename, PaintEventArgs e, Point p)
        {
            Rectangle outline = new Rectangle(new Point(p.X, p.Y), new Size(950, 28));
            
            Color back = Color.FromKnownColor(KnownColor.Control);
            System.Drawing.SolidBrush bckgBrush = new System.Drawing.SolidBrush(back);

            using (bckgBrush)
                e.Graphics.FillRectangle(bckgBrush, outline);
        }

        // This method is invoked when the DrawingPanel needs to be re-drawn
        protected override void OnPaint(PaintEventArgs e)
        {
            lock (theLock)
            {
                int DistanceBetween = 0;
                foreach (string s in spreadsheets)
                {
                    SheetDrawer(s, e, DistanceBetween);
                    DistanceBetween += 30;
                }
                base.OnPaint(e);
            }

            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }
    }
}

