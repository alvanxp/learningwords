using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearningWordsApi.Models
{
    public class WordModel
    {
        public string Word { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public string ToWord { get; set; }
        public string ToLanguage { get; set; }
        public string ToDescription { get; set; }
    }
}