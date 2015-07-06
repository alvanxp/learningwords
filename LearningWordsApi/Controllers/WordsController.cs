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
        int randomID =
            db.Database.SqlQuery<int>(string.Format("select top 1 WL.ID from dbo.WordLearned wl inner join dbo.language l on wl.languageid = l.id  where l.laNguagecode = '{0}' ORDER BY NEWID()", from))
                .FirstOrDefault();
        WordLearned wordLearned =
            db.WordLearneds.FirstOrDefault(f=>f.ID== randomID);
      
        if (wordLearned == null)
        {
            return NotFound();
        }

        IQueryable<WordLearned> translateWord = db.WordLearneds
           .Where(tw => tw.WordId == wordLearned.WordId);

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
        public async Task<IHttpActionResult> PostWordLearned(WordLearned wordLearned)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.WordLearneds.Add(wordLearned);

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

        private bool WordLearnedExists(int id)
        {
            return db.WordLearneds.Count(e => e.ID == id) > 0;
        }
    }
}