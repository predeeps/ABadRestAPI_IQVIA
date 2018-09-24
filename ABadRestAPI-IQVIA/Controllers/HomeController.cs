using ABadRestAPI_IQVIA.Business;
using ABadRestAPI_IQVIA.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABadRestAPI_IQVIA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Result()
        {
            TweetCollector tweetCollector = new TweetCollector();
            var Model = tweetCollector.GetTweets();
            TempData["data"] = Model;
            return View(Model);
        }

        [HttpGet]
        [DeleteFileAttribute]
        public virtual ActionResult Download(string file)
        {
            string fullPath = Path.Combine(Server.MapPath("~/Views"), file);
            return File(fullPath, "application/vnd.ms-excel", file);
        }
        /// <summary>
        /// ExportExcel will generate file and return file name to download
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string ExportExcel()
        {
            string strFilePath = Request.PhysicalApplicationPath + "Views\\" + "TweetRecords" + ".xls";
            string strFileName =  "TweetRecords.xls";
            var model = TempData["data"] as TweetViewModel;
            TempData.Keep("data");
            if (model !=null)
            {
                System.Data.DataTable dt = new System.Data.DataTable();

                #region Excel column
                dt.Columns.Add("Id", typeof(string));
                dt.Columns.Add("Timestamp", typeof(string));
                dt.Columns.Add("Text", typeof(string));
                #endregion excel column

                strFileName = model.TweetList.Count == 0 ? "nofile" : strFileName;

                foreach (var item in model.TweetList)
                {
                    DataRow dr;
                    dr = dt.NewRow();
                    dr["Id"] = item.id;
                    dr["Timestamp"] = item.stamp.ToString();
                    dr["Text"] = item.text;

                    dt.Rows.Add(dr);
                }

                #region Decorate excel
                if (dt.Rows.Count > 0)
                {
                    GridView gv = new GridView();
                    gv.DataSource = dt;
                    gv.DataBind();
                    StreamWriter sw = new StreamWriter(strFilePath);
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gv.RenderControl(htw);
                    sw.Close();
                    System.IO.FileStream fs = System.IO.File.Open(strFilePath, System.IO.FileMode.Open);
                    byte[] btFile = new byte[fs.Length + 1];
                    fs.Read(btFile, 0, (int)fs.Length);
                    fs.Close();
                }
                #endregion
            }
            else
            {
                strFileName = "nofile";
            }
            
            return strFileName;
        }

    }
    /// <summary>
    /// Action filter to delete once the file is downloaded
    /// </summary>
    public class DeleteFileAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Flush();

            string filePath = (filterContext.Result as FilePathResult).FileName;

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}