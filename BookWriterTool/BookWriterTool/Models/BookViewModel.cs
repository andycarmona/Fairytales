using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookWriterTool.Models
{
    public class BookModel
    {
        public string ChapterId { get; set; }
        public string PageId { get; set; }
        public string Target { get; set; }
        public string FrameId { get; set; }
        public List<ObjectModel> Objects { get; set; }
        public List<FrameModel> Frames { get; set; }

    }
    public class FrameModel
    {
        public string Bordertype { get; set; }
    }
    public class ObjectModel
    {

        public string ObjectId { get; set; }
        public string ImageObj { get; set; }
        public string ScaleX { get; set; }
        public string ScaleY { get; set; }
        public string OrigoX { get; set; }
        public string OrigoY { get; set; }
        public string Type { get; set; }
    }
}