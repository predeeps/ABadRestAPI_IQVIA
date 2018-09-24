using System;
using System.ComponentModel.DataAnnotations;

namespace ABadRestAPI_IQVIA.Models
{
    public class TweetModel
    {
        public long id { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-dd-MMThh:mm:ss.fffZ}", ApplyFormatInEditMode = true)]
        public DateTime stamp { get; set; }
        public string text { get; set; }
    }
}