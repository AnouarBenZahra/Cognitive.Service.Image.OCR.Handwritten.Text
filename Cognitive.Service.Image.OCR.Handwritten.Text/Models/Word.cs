using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cognitive.Service.Image.OCR.Handwritten.Text.Models
{
    public class Word
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
    }
}
