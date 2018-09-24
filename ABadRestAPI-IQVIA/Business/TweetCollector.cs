using ABadRestAPI_IQVIA.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ABadRestAPI_IQVIA.Business
{
    public class TweetCollector
    {
        private const string _badRestBaseUrl = "https://badapi.iqvia.io";
        /// <summary>
        /// GetTweets
        /// </summary>
        /// <returns></returns>
        public TweetViewModel GetTweets()
        {
            TweetViewModel tweetView = new TweetViewModel();
            bool IsResponseAvailable = true;
            //End date
            string endDate = "2017-12-31T23:59:59.999Z";
            //Start date
            string startDate = "2016-01-01T00:00:00.001Z";
            /*
             * Loop through and get the response of Restapi until we get less then 100 records, assuming there may be less record in the last page or empty
             * 
             * Filter the response list based on year, as we need to get records of 2016 & 2017 years
             * 
             * Remove duplicates based on tweet id, by grouping based on tweet id and selecting first.
             * */
           
            while (IsResponseAvailable)
            {
                try
                {
                    var client = new RestClient(_badRestBaseUrl);
                    var request = new RestRequest("/api/v1/Tweets", Method.GET);
                    request.AddQueryParameter("startDate", startDate);
                    request.AddQueryParameter("endDate", endDate);

                    var response = client.Execute(request);
                    if (response == null || string.IsNullOrEmpty(response.Content))
                        break;
                    var tweetList = JsonConvert.DeserializeObject<List<TweetModel>>(response.Content);
                    if (tweetList.Count < 100)
                    {
                        IsResponseAvailable = false;
                    }
                    var filtered = tweetList.Where(a => a.stamp.Year >= 2016 && a.stamp.Year <= 2017);
                    if (filtered.Count() > 0)
                    {
                        tweetView.TweetList.AddRange(filtered);
                        //Taking max timestamp from api response
                        startDate = filtered.Max(t => t.stamp).ToString("yyyy-MM-ddThh:mm:ss.fffZ");
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }

            //Removing Duplicates
            var tweetsWithoutDuplicate = tweetView.TweetList.GroupBy(i => i.id).Select(g => g.First()).ToList();

            //Getting count of duplicate records
            tweetView.DuplicateCount = tweetView.TweetList.Count() - tweetsWithoutDuplicate.Count();

            tweetView.TweetList = tweetsWithoutDuplicate;
            
            return tweetView;
        }
    }
}