using System;
using System.Web.Http;
using LearningWordsApi.Controllers;
using LearningWordsApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LearningWordsApi.Tests.Controllers
{
    [TestClass]
    public class WordControllerTest
    {
        [TestMethod]
        public async void TestGetRandom()
        {
            var wordController = new WordsController();
            WordLearned result = await wordController.GetWordRandom() as WordLearned;
            Assert.IsNotNull(result);
        }
    }
}
