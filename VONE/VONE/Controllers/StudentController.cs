using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VONE.Models;
using System.Web.Script.Serialization;
using System.IO;

namespace VONE.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult AskQuestions()
        {
            DBAPI api = new DBAPI();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var x = api.AccessField();
            var json = serializer.Serialize(x);
            TempData["json"] = json;
            return View();
        }

        [HttpPost]
        public ActionResult AddSuccess()
        {
            var title = Request["question"];
            var field = Request["checkbox"];
            var content = Request["content"];
            var api = new DBAPI();
            string[] s = field.Split(new char[] { ',' });
            api.addQuestion(Convert.ToInt32(Request.Cookies["Account"].Value), title, content, s);

            return RedirectToAction("Index", "Home");
        }
        

        [HttpPost]
        public ActionResult AnswerQuestions()
        {
            TempData["S_ID"] = Request.Cookies["Account"].Value;
            TempData["Q_ID"] = Request["Q_ID"];
            return View();
        }

        [HttpPost]
        public ActionResult AnswerQuestionsSuccess()
        {
            var q_num = Convert.ToInt32(Request["Q_ID"]);
            var content = Request["content"];
            DBAPI api = new DBAPI();
            api.AddAnswer(Convert.ToInt32(Request.Cookies["Account"].Value), q_num, content, 0);
            return RedirectToAction("Index", "Home");
        }

        public ViewResult UserInfo()
        {
            if(Request["s_id"]==null)
            {
                DBAPI api = new DBAPI();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                JavaScriptSerializer serializer2 = new JavaScriptSerializer();
                var studentInfo = api.StudentInfo(Convert.ToInt32(Request.Cookies["Account"].Value));
                var json = serializer.Serialize(studentInfo);
                TempData["studentInfo"] = json;

                var studentAnswer = api.StudentAnswer(Convert.ToInt32(Request.Cookies["Account"].Value) );
                var answer = serializer2.Serialize(studentAnswer);
                var time = studentInfo.ADMITTED_YEAR.ToString();
                TempData["studentAnswer"] = answer;
                TempData["admittedYear"] = time;
                return View();
            }
            else
            {
                var s_id = Convert.ToInt32(Request["s_id"]);
                var studentname = Request["studentname"];
                var college_name = Request["college_name"];
                var major = Request["major"];
                var degree = Request["degree"];
                DateTime? admitted_year = Convert.ToDateTime(Request["admitted_year"]);
                var email = Request["email"];
                var phone_num = long.Parse(Request["phone_num"]); 

                var stu = new student
                {
                    S_ID = s_id,
                    NAME = studentname,
                    COLLEGE_NAME = college_name,
                    MAJOR = major,
                    DEGREE = degree,
                    ADMITTED_YEAR = admitted_year,
                    EMAIL = email,
                    PHONE_NUM = phone_num
                };

                var api = new DBAPI();
                api.UpdateStudent(stu);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                JavaScriptSerializer serializer2 = new JavaScriptSerializer();
                var studentInfo = api.StudentInfo(Convert.ToInt32(Request.Cookies["Account"].Value));
                var json = serializer.Serialize(studentInfo);
                TempData["studentInfo"] = json;

                var studentAnswer = api.StudentAnswer(Convert.ToInt32(Request.Cookies["Account"].Value));
                var answer = serializer2.Serialize(studentAnswer);
                var time = studentInfo.ADMITTED_YEAR.ToString();
                TempData["studentAnswer"] = answer;
                TempData["admittedYear"] = time;
                return View();
            }
        }

        [HttpPost]
        public ActionResult ApplySuccess(HttpPostedFileBase re)
        {
  
            var c_id =Convert.ToInt32(Request["comp_id"]);
            var pos_name = Request["pos_name"];
            var S_ID =Convert.ToInt32(Request.Cookies["Account"].Value);
            var doc_path = "~/Resume/" + re.FileName;


            if (ModelState.IsValid&&re.ContentLength>0)
            {
                var filename = Path.GetFileName(re.FileName);
                var path = Path.Combine(Server.MapPath("~/Resume"), filename);
                re.SaveAs(path);

                DBAPI api = new DBAPI();
                api.AddResume(S_ID,c_id,pos_name,doc_path);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}