using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cognitive.Service.Image.OCR.Handwritten.Text.Models
{
    public class ImageInfo
    {
        public string status
        {
            get;
            set;
        }
        public RecognitionResult recognitionResult
        {
            get;
            set;
        }
    }
}
