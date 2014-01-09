using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookWriterTool.Helpers
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Web.Helpers;

    public class ImageHandler
    {
        
        public Bitmap ResizeImage(Bitmap originalBmp,int newWidth)
        {
           
            var origWidth = originalBmp.Width;
            var origHeight = originalBmp.Height;
            var sngRatio = origWidth / origHeight;
            var newHeight = newWidth / sngRatio;
            var newBmp = new Bitmap(originalBmp, newWidth, newHeight);

            return newBmp;
        }


    }
}