using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using LearningWordsApi.DataAccess;
using LearningWordsApi.Models;

namespace LearningWordsApi.Controllers
{

public class WordsController : ApiController
    {
        private LearningWordDataModel db = new LearningWordDataModel();

        // GET: api/Words
        public IEnumerable<WordModel> GetWordLearneds(string language, string toLanguage)
        {            
            IList<WordModel> result = new List<WordModel>();
            try
            {
                var groups = db.WordLearneds.GroupBy(g => g.WordID);
                foreach (var group in groups)
                {
                   var word = group.FirstOrDefault(g => g.Language.LanguageCode == language);
                    var toWord = group.FirstOrDefault(g => g.Language.LanguageCode == toLanguage);
                    result.Add(new WordModel() { WordId = group.Key,
                        Word = word.Word, Description = word.Description, Language = language,
                    ToWord = toWord.Word, ToDescription = toWord.Description, ToLanguage = toLanguage});
                }
                return result;
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }

        // GET: api/Words/5
        [ResponseType(typeof(WordLearned))]
        public async Task<IHttpActionResult> GetWordLearned(int id)
        {
            WordLearned wordLearned = await db.WordLearneds.FindAsync(id);
            if (wordLearned == null)
            {
                return NotFound();
            }

            return Ok(wordLearned);
        }


        // GET: api/Words/5
    [ResponseType(typeof (WordModel))]
    [Route("api/words/random")]
    public async Task<IHttpActionResult> GetWordRandom(string from, string to)
    {
        long randomID =
            db.Database.SqlQuery<long>(string.Format("select top 1 WL.ID from dbo.WordLearned wl inner join dbo.language l on wl.languageid = l.id  where l.laNguagecode = '{0}' ORDER BY NEWID()", from))
                .FirstOrDefault();
        WordLearned wordLearned =
            db.WordLearneds.FirstOrDefault(f=>f.ID== randomID);
      
        if (wordLearned == null)
        {
            return NotFound();
        }

        IQueryable<WordLearned> translateWord = db.WordLearneds
           .Where(tw => tw.WordID == wordLearned.WordID);

        var toLearned = translateWord.FirstOrDefault(t => t.Language.LanguageCode == to);
        if (toLearned == null) return NotFound();

        
        return Ok(new WordModel()
        {
            Word = wordLearned.Word,
            Description = wordLearned.Description,
            Language = wordLearned.Language.LanguageCode,
            ToWord = toLearned.Word,
            ToLanguage = toLearned.Language.LanguageCode,
            ToDescription = toLearned.Description
        });
    }

    // PUT: api/Words/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutWordLearned(WordModel wordLearned)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
           var word = await db.WordLearneds.FirstOrDefaultAsync(
               w=>w.WordID == wordLearned.WordId && w.Language.LanguageCode == wordLearned.Language);
            var toWord = await db.WordLearneds.FirstOrDefaultAsync(w => w.WordID == wordLearned.WordId &&
            w.Language.LanguageCode == wordLearned.ToLanguage);

            word.Word = wordLearned.Word;
            word.Description = wordLearned.Description;

            toWord.Word = wordLearned.ToWord;
            toWord.Description = wordLearned.ToDescription;

            db.Entry(word).State = EntityState.Modified;
            db.Entry(toWord).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
                throw;
                //if (!WordLearnedExists(wordLearned.WordId))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Words
        [ResponseType(typeof(WordLearned))]
        public async Task<IHttpActionResult> PostWordLearned(WordModel word)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var language1 = await db.Languages.FirstOrDefaultAsync(l => l.LanguageCode == word.Language);
            var language2 = await db.Languages.FirstOrDefaultAsync(l => l.LanguageCode == word.ToLanguage);

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

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WordLearnedExists(wordLearned.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = wordLearned.ID }, wordLearned);
        }

        // DELETE: api/Words/5
        [ResponseType(typeof(WordLearned))]
        public async Task<IHttpActionResult> DeleteWordLearned(int id)
        {
            WordLearned wordLearned = await db.WordLearneds.FindAsync(id);
            if (wordLearned == null)
            {
                return NotFound();
            }

            db.WordLearneds.Remove(wordLearned);
            await db.SaveChangesAsync();

            return Ok(wordLearned);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WordLearnedExists(long id)
        {
            return db.WordLearneds.Count(e => e.ID == id) > 0;
        }
    }
}
