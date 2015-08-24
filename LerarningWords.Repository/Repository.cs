using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using LearningWords.DomainModel;
using LearningWords.Model;
using LearningWords.Repository;

namespace LerarningWords.Repository
{
    public class Repository : IRepository
    {
        private LearningWordDataModel db = new LearningWordDataModel();

        
        public IList<IGrouping<Guid, WordLearned>> GetGroupedWords()
        {
            return db.WordLearneds.GroupBy(g => g.WordID).ToList();
        }
    

        public Guid GetRandomWordId(string from)
        {
            Guid randomID =
db.Database.SqlQuery<Guid>(string.Format("select top 1 WL.WordID from dbo.WordLearned wl inner join dbo.language l on wl.languageid = l.id  where l.laNguagecode = '{0}' ORDER BY NEWID()", from))
    .FirstOrDefault();
           
            return randomID;
        }


        public IGrouping<Guid, WordLearned> GetWordLearnedGroup(Guid id)
        {
            return db.WordLearneds.Where(w => w.WordID == id).GroupBy(g => g.WordID).FirstOrDefault();
        }

        public void UpdateWord(WordModel wordLearned)
        {
            var word = db.WordLearneds.FirstOrDefault(
                w => w.WordID == wordLearned.WordId && w.Language.LanguageCode == wordLearned.Language);
            var toWord = db.WordLearneds.FirstOrDefault(w => w.WordID == wordLearned.WordId &&
                                                             w.Language.LanguageCode == wordLearned.ToLanguage);

            word.Word = wordLearned.Word;
            word.Description = wordLearned.Description;

            toWord.Word = wordLearned.ToWord;
            toWord.Description = wordLearned.ToDescription;

            db.Entry(word).State = EntityState.Modified;
            db.Entry(toWord).State = EntityState.Modified;
            db.SaveChangesAsync();
        }


        public Guid CreateWord(WordModel word)
        {
            var language1 = db.Languages.FirstOrDefault(l => l.LanguageCode == word.Language);
            var language2 = db.Languages.FirstOrDefault(l => l.LanguageCode == word.ToLanguage);

            Guid wordId = Guid.NewGuid();
            var wordLearned = new WordLearned();
            wordLearned.LanguageID = language1.ID;
            wordLearned.Language = language1;
            wordLearned.Word = word.Word;
            wordLearned.WordID = wordId;
            wordLearned.Description = word.Description;
            db.WordLearneds.Add(wordLearned);

            var toWordLearned2 = new WordLearned();
            toWordLearned2.LanguageID = language2.ID;
            toWordLearned2.Language = language2;
            toWordLearned2.Word = word.ToWord;
            toWordLearned2.Description = word.ToDescription;
            toWordLearned2.WordID = wordId;
            db.WordLearneds.Add(toWordLearned2);

            db.SaveChangesAsync();
            return wordId;
        }
    }

   

}
