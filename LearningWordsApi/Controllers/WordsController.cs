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
        public IQueryable<WordModel> GetWordLearneds()
        {
            try
            {
                return db.WordLearneds.Select(w => new WordModel(){Word = w.Word, Description = w.Description, Language = w.Language.LanguageCode});
               
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
        public async Task<IHttpActionResult> PutWordLearned(int id, WordLearned wordLearned)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != wordLearned.ID)
            {
                return BadRequest();
            }

            db.Entry(wordLearned).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WordLearnedExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
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
            db.WordLearneds.Add(wordLearned);

            var toWordLearned2 = new WordLearned();
            toWordLearned2.LanguageID = language2.ID;
            toWordLearned2.Language = language2;
            toWordLearned2.Word = word.ToWord;
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
