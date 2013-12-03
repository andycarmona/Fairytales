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

        string AddCharacterToContent(string[] content,string fileName);
        string AddFrame(string[] content, string fileName);
        book GetAllContent();
        void EditChapter(string chapterId);
    }
}
