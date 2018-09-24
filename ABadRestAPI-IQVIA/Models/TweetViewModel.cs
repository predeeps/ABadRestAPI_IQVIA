using System;
using System.Collections.Generic;

namespace ABadRestAPI_IQVIA.Models
{
    public class TweetViewModel
    {
        public List<TweetModel> TweetList = new List<TweetModel>();
        public int DuplicateCount { get; set; }

    }
}