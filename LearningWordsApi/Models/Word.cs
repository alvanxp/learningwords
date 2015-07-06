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
    }
}