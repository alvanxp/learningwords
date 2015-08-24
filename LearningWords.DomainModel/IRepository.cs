using System;
using System.Collections.Generic;
using System.Linq;
using LearningWords.Model;

namespace LearningWords.DomainModel
{
    public interface IRepository
    {
       
        Guid GetRandomWordId(string fromLanguage);
        IList<IGrouping<Guid, WordLearned>> GetGroupedWords();
        IGrouping<Guid, WordLearned> GetWordLearnedGroup(Guid id);
        void UpdateWord(WordModel word);
        Guid CreateWord(WordModel word);
    }
}
