using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordMine
{
    class bonusword
    {
        public String word;
        public String partOfSpeech;
        public String definition;
        public String similarTo;
        public int length;

        public String Word { get; set; }
        public String PartOfSpeech { get; set; }
        public String Definition { get; set; }
        public String SimilarTo { get; set; }

        public bonusword(String pos, String word, String similar, String def)
        {
            this.partOfSpeech = pos;
            this.word = word;
            this.similarTo = similar;
            this.definition = def;
            this.length = word.Length;
        }
    }
}
