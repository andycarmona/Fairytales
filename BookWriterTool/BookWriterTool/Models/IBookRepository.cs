using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookWriterTool.Models
{
    public interface IBookRepository
    {
       bookChapter GetChapterById(string chapterId);

        string SetActualFile(string actualBook);
        book AddPage(string chapterNumber,string fileName);

       
        string AddObjectToContent(BookModel content, string fileName);
        string AddSpeechBubbleObject(BookModel content, string fileName);
        string DeleteObjectFromContent(BookModel content, string fileName);
        string UpdateObjectPosition(BookModel content, string fileName);
        string AddBackgroundToContent(BookModel content, string fileName);
        string AddFrame(BookModel content, string fileName);
        book GetAllContent();
        void EditChapter(string chapterId);
        string AddTextToBubble(string model, string componentId, string fileName, string type, string form);
        string AddTextToContent(string model, string componentId, string fileName,string type,string form);
        
    }
}
