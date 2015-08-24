using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningWords.Model;

namespace LearningWords.DomainModel
{
    public class LeraningWordsService
    {
        private readonly IRepository _wordsRepository;

        public LeraningWordsService(IRepository wordsRepository)
        {
            _wordsRepository = wordsRepository;
        }

        public IEnumerable<WordModel> GetWordLearneds(string language, string toLanguage)
        {
            IList<WordModel> result = new List<WordModel>();
            try
            {
                var groups = _wordsRepository.GetGroupedWords();
                foreach (var group in groups)
                {
                    var wordModel = this.GetWordModel(language, toLanguage, @group);
                    result.Add(wordModel);
                }
                return result.OrderBy(w=>w.Word);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Coudn't get  learned Words");
            }
        }

        private WordModel GetWordModel(string language, string toLanguage, IGrouping<Guid, WordLearned> @group)
        {
            var word = @group.FirstOrDefault(g => g.Language.LanguageCode == language);
            var toWord = @group.FirstOrDefault(g => g.Language.LanguageCode == toLanguage);
            var wordModel = new WordModel()
            {
                WordId = @group.Key,
                Word = word.Word,
                Description = word.Description,
                Language = language,
                ToWord = toWord.Word,
                ToDescription = toWord.Description,
                ToLanguage = toLanguage
            };
            return wordModel;
        }

        public WordModel GetWordLearned(Guid id, string language, string toLanguage)
        {
            var wordGroup = _wordsRepository.GetWordLearnedGroup(id);
            if (wordGroup == null) throw new ArgumentNullException("Word not found: "+ id);
            return GetWordModel(language, toLanguage, wordGroup);
        }

        public WordModel GetRandomWord(string from, string to)
        {
            Guid randomId = _wordsRepository.GetRandomWordId(from);
            if (randomId == null)
                throw new ArgumentNullException("Word id not found");
            return this.GetWordLearned(randomId, from, to);
        }


        public void UpdateWord(WordModel word)
        {
            _wordsRepository.UpdateWord(word);
        }

        public Guid CreateWord(WordModel word)
        {
            return _wordsRepository.CreateWord(word);
        }

      
    }
}
