using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookWriterTool.Helpers
{
    using System.IO;
    using System.Web.Hosting;
    using System.Web.Mvc;

    public class VideoHandler:ActionResult
    {
        /// <summary> 
        /// The below method will respond with the Video file 
        /// </summary> 
        /// <param name="context"></param> 
        public override void ExecuteResult(ControllerContext context)
        {
            //The File Path 
            var videoFilePath = HostingEnvironment.MapPath("~/Content/VideoFile/big_buck_bunny.mp4");
            //The header information 
            context.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=big_buck_bunny.mp4");

            var file = new FileInfo(videoFilePath);
            //Check the file exist,  it will be written into the response 
            if (file.Exists)
            {
                var stream = file.OpenRead();
                var bytesinfile = new byte[stream.Length];
                stream.Read(bytesinfile, 0, (int)file.Length);
                context.HttpContext.Response.BinaryWrite(bytesinfile);
            }
        } 
    }
}