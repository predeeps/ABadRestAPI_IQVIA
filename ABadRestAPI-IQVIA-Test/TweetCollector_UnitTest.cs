using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ABadRestAPI_IQVIA.Business;

namespace ABadRestAPI_IQVIA_Test
{
    [TestClass]
    public class TweetCollector_UnitTest
    {
        /// <summary>
        /// GetTweets test method
        /// </summary>
        [TestMethod]
        public void GetTweets()
        {
            // Arrange
            TweetCollector tweetCollector = new TweetCollector();

            //Act
            var Model = tweetCollector.GetTweets();

            // Assert
            Assert.IsNotNull(Model);
            Assert.IsNotNull(Model.TweetList);
            Assert.IsNotNull(Model.DuplicateCount);
        }
    }
}
