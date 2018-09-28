using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cognitive.Service.Image.OCR.Handwritten.Text.Models
{
    public class Line
    {
        public List<int> boundingBox
        {
            get;
            set;
        }
        public string text
        {
            get;
            set;
        }
        public List<Word> words
        {
            get;
            set;
        }
    }
}
