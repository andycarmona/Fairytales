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
        book AddPage(string chapterNumber);

        void AddContentToFrame(string[] content);
        book GetAllContent();
        void EditChapter(string chapterId);
    }
}
