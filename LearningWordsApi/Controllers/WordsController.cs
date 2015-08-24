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
using LearningWords.DomainModel;
using LearningWords.Model;
using LearningWordsApi.DataAccess;
using LearningWordsApi.Models;
using LerarningWords.Repository;

namespace LearningWordsApi.Controllers
{

public class WordsController : ApiController
{

    private LeraningWordsService _service;
    public WordsController()
    {
        _service = new LeraningWordsService(new Repository());
    }

    // GET: api/Words
        public IEnumerable<WordModel> GetWordLearneds(string language, string toLanguage)
        {
            return _service.GetWordLearneds(language, toLanguage);
        }

        // GET: api/Words/5
        [ResponseType(typeof(WordModel))]
        public IHttpActionResult GetWordLearned(Guid id, string language, string toLanguage)
        {
            WordModel wordLearned = _service.GetWordLearned(id, language, toLanguage);
            if (wordLearned == null)
            {
                return NotFound();
            }

            return Ok(wordLearned);
        }


        // GET: api/Words/5
    [ResponseType(typeof (WordModel))]
    [Route("api/words/random")]
    public IHttpActionResult GetWordRandom(string from, string to)
    {
        try
        {
            WordModel word = _service.GetRandomWord(from, to);
            return Ok(word);
        }
        catch (ArgumentNullException ex)
        {
            return NotFound();
        }
        catch (Exception e)
        {
            return InternalServerError();
        }
    }

    // PUT: api/Words/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWordLearned(WordModel wordLearned)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _service.UpdateWord(wordLearned);

                return StatusCode(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // POST: api/Words
        [ResponseType(typeof(WordLearned))]
        public IHttpActionResult PostWordLearned(WordModel word)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
               var id = _service.CreateWord(word);
                return CreatedAtRoute("DefaultApi", new { id = id }, id);
            }
            catch (DbUpdateException)
            {
                return InternalServerError();
            }

            
        }

        //// DELETE: api/Words/5
        //[ResponseType(typeof(WordLearned))]
        //public async Task<IHttpActionResult> DeleteWordLearned(int id)
        //{
        //    WordLearned wordLearned = await db.WordLearneds.FindAsync(id);
        //    if (wordLearned == null)
        //    {
        //        return NotFound();
        //    }

        //    db.WordLearneds.Remove(wordLearned);
        //    await db.SaveChangesAsync();

        //    return Ok(wordLearned);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _service = null;
            }
            base.Dispose(disposing);
        }

        //private bool WordLearnedExists(long id)
        //{
        //    return db.WordLearneds.Count(e => e.ID == id) > 0;
        //}
    }
}
