using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VONE.Models;

namespace VONE.Controllers
{
    public class CompanyController : Controller
    {
        public ActionResult AddWantedJob()                                                
        {
            DBAPI api = new DBAPI();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var x = api.AccessField();
            var json = serializer.Serialize(x);
            TempData["json"] = json;
            return View();
        }

        [HttpPost]
        public ActionResult AddJobSuccess()                  //需要添加公司需要的职业 修改三个表  就寻找最大的一个数 wanted_pos pos_release pos_related_field
        {
            var pos_name = Request["pos_name"];
            var salary = Convert.ToInt32(Request["salary"]);
            var radio = Request["radio"];                   //这个radio是针对field表
            var description = Request["description"];

            var api = new DBAPI();
            api.AddJob(Convert.ToInt32(Request.Cookies["Account"].Value), pos_name, salary, description, radio);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DeleteJob()           //写成函数好了
        {
            var pos_id = Convert.ToInt32(Request["pos_id"]);
            DBAPI api = new DBAPI();
            api.DeleteJob(pos_id);
            return RedirectToAction("Index", "Home");
        }

        public ViewResult CompInfo()
        {
            DBAPI api = new DBAPI();
            if (Request["c_id"] == null)                //分成是否有修改
            {}
            else
            {
                var C_ID = Convert.ToInt32(Request["c_id"]);
                var name = Request["comp_name"];
                var type = Request["comp_type"];
                var address = Request["comp_address"];
                var Email = Request["email"];
                var Phone_num = Request["phone_num"];

                var comp = new company
                {
                    c_id = C_ID,
                    comp_name = name,
                    comp_type = type,
                    comp_address = address,
                    comp_email = Email,
                    comp_phone = decimal.Parse(Phone_num)
                };
                api.UpdateCompany(comp);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var c_id = Convert.ToInt32(Request.Cookies["Account"].Value);

            var companyInfo = api.CompanyInfo(c_id);
            var json = serializer.Serialize(companyInfo);
            TempData["companyInfo"] = json;

            var companyJobs = api.CompanyJobs(c_id);            //List<job>
            var companyjobs = serializer.Serialize(companyJobs);
            TempData["companyJobs"] = companyjobs;

            var companyResume = api.CompanyResume(c_id);        //List<ResumeInfo>
            var companyresume = serializer.Serialize(companyResume);
            TempData["companyResume"] = companyresume;

            return View();
        }
    }
}