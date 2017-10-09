using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VONE.Models;
using VONE;
using System.Web.Script.Serialization;

namespace VONE.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
              
        public ViewResult Question()
        {
            var keyWord = Request["search"];
            var api = new DBAPI();
             var list = api.searchQuestion(keyWord);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(list);
            TempData["json"] = json;

            var allField = api.AccessField();
            var fieldJson = serializer.Serialize(allField);
            TempData["field"] = fieldJson;

            return View();
        }

        public ViewResult FieldQuestion(string field)
        {
            var api = new DBAPI();
            var list = api.searchRelatedQuestion(field);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(list);
            TempData["json"] = json;

            var allField = api.AccessField();
            var fieldJson = serializer.Serialize(allField);
            TempData["field"] = fieldJson;

            return View("Question");
        }

        
        public ViewResult QuestionDetails(int Q_ID)         //呈现的是 问题的各种回答 点进去之后是回答的各种显示
        {
            DBAPI api = new DBAPI();
            var answerList = api.searchAnswer(Q_ID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(answerList);
            TempData["json"] = json;
            TempData["Q_ID"] = Q_ID;
            return View();
        }

        public ViewResult Jobs()
        {
            DBAPI api = new DBAPI();
            var jobs = api.SearchJobs();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jobList = serializer.Serialize(jobs);
            TempData["json"] = jobList;
            return View();
        }

        [HttpPost]
        public ViewResult JobDetails(int POS_ID)                    //需要显示出job的信息和公司的信息
        {
            DBAPI api = new DBAPI();
            var job = api.JobDetails(POS_ID);
            var compInfo = api.CompanyInfo(job.c_id);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jobJSON = serializer.Serialize(job);
            var compJSON = serializer.Serialize(compInfo);

            TempData["jobJSON"] = jobJSON;
            TempData["compJSON"] = compJSON;

            return View();
        }

        public FileResult DownloadTemplate()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"C:\Users\13701\Desktop\VONE\VONE\Resume\Resume.7z");
            string fileName = "Resume.7z";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public FileResult DownloadResume(string doc_path)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(@doc_path);
            string[] sArray = doc_path.Split('j');
            string fileName = sArray[2];
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public RedirectResult favor(int Q_ID,int S_ID)
        {
            DBAPI api = new DBAPI();
            api.favor(S_ID, Q_ID);
            return Redirect("QuestionDetails?Q_ID=" + Q_ID);
        }

    }
}