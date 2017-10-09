using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VONE.Models;
using VONE;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace VONE.Controllers
{
    public class DBAPI
    {
        public bool SignIn(int ID,string pwd,string identity)                 //as for indentity 学生 stands for students 公司 stands for company
        {
           if(identity=="学生")
            {
                using (Entities entity = new Entities())
                {
                    var x = (from v in entity.STUDENT
                             select new
                             {
                                 S_ID = v.S_ID,
                                 NAME = v.NAME,
                                 S_KEY = v.S_KEY

                             }).AsEnumerable().ToList();
                    foreach (var item in x)
                    {
                        if (item.S_ID == ID && item.S_KEY == pwd)
                        {
                            return true;
                        }
                    }
                }
            }

           if(identity=="公司")
            {
                using (Entities entity = new Entities())
                {
                    var x = (from v in entity.COMPANY
                             select new
                             {
                                 C_ID = v.C_ID,
                                 NAME = v.COMP_NAME,
                                 C_KEY = v.C_KEY
                                 
                             }).AsEnumerable().ToList();
                    foreach (var item in x)
                    {
                        if (item.C_ID == ID && item.C_KEY == pwd)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public string SearchName(int ID,string identity)        
        {
            if (identity == "学生")
            {
                using (Entities entity = new Entities())
                {
                    var x = (from v in entity.STUDENT
                             where v.S_ID == ID
                             select new
                             {
                                 name = v.NAME
                             }).AsEnumerable().ToList();
                    return x[0].name;
                }
            }
            if (identity=="公司")
            {
                using (Entities entity = new Entities())
                {
                    var y = (from v in entity.COMPANY
                             where v.C_ID == ID
                             select new
                             {
                                 name = v.COMP_NAME
                             }).AsEnumerable().ToList();
                    return y[0].name;
                }
            }

            return null;
        }

        public List<FieldName> AccessField()
        {
            using (Entities entity = new Entities())
            {
                var x = (from v in entity.FIELD
                         select new FieldName
                         {
                             fieldname = v.FIELD_NAME
                         }).AsEnumerable().ToList();
                return x;
            }
        }

        public void addQuestion(int S_ID,string title,string description,string[] fields)                               //需要对三个表格进行修改 一个是question表格 一个是q_to_field 一个是prompt_q 就是问题和学生的对应
        {
            using (Entities entity = new Entities())
            {
                var length = entity.QUESTION.Count();
                var question = new QUESTION
                {
                    Q_NUM = length + 1,
                    TITLE = title,
                    DESCRIPTION = description
                };
                entity.QUESTION.Add(question);

                var prompt_q = new PROMPT_Q
                {
                    S_ID = S_ID,
                    Q_NUM = length + 1
                };
                entity.PROMPT_Q.Add(prompt_q);

                for(var i=0;i<fields.Length;i++)
                {
                    var q_to_field = new Q_TO_FIELD
                    {
                        Q_NUM = length + 1,
                        FIELD_NAME = fields[i]
                    };
                    entity.Q_TO_FIELD.Add(q_to_field);
                }
                entity.SaveChanges();
            }
        }

        public List<QuestionModel> searchQuestion(string keyWord)                                      //寻找问题的界面 需要获得问题的 id 标题 描述 这个还是简单的
        {
            using (Entities entity = new Entities())
            {
                var x = (from v in entity.QUESTION
                         where v.TITLE.Contains(keyWord)
                         select new QuestionModel
                         {
                             Q_Num = v.Q_NUM,
                             Q_Title = v.TITLE,
                             Q_Description = v.DESCRIPTION
                         }).AsEnumerable().ToList();
                return x;
            }
        }

        public List<QuestionModel> searchRelatedQuestion(string field)
        {
            using (Entities entity = new Entities())
            {
                var x = (from v in entity.QUESTION
                         from u in entity.Q_TO_FIELD
                         where v.Q_NUM == u.Q_NUM && u.FIELD_NAME == field
                         select new QuestionModel
                         {
                             Q_Num = v.Q_NUM,
                             Q_Title = v.TITLE,
                             Q_Description = v.DESCRIPTION
                         }).AsEnumerable().ToList();
                return x;
            }
        }               //获取相关领域问题

        public void favor(int S_ID,int Q_NUM)
        {
            using (Entities entity = new Entities())
            {
                var x = (from v in entity.ANSWER_TO_Q
                         where v.S_ID == S_ID && v.Q_NUM == Q_NUM
                         select v).First();
                x.POINTS += 1;
                entity.SaveChanges();
            }
        }

        public List<AnswerModel> searchAnswer(int num)
        {
            using (Entities entity = new Entities())
            {
                var x = (from v in entity.ANSWER_TO_Q
                         from u in entity.STUDENT
                         where v.Q_NUM == num && v.S_ID == u.S_ID
                         select new AnswerModel
                         {
                             q_num = v.Q_NUM,
                             s_id = u.S_ID,
                             studentName = u.NAME,
                             content = v.CONTENT,
                             point = v.POINTS
                         }).AsEnumerable().ToList();
                return x;
            }
        }

        public void AddAnswer(int S_ID,int Q_NUM,string content,int points)                                             //添加回答需要的数据 第一个是
        {
            var answer = new ANSWER_TO_Q();
            answer.S_ID = S_ID;
            answer.Q_NUM = Q_NUM;
            answer.POINTS = points;
            answer.CONTENT = content;
            using (Entities entity = new Entities())
            {
                entity.ANSWER_TO_Q.Add(answer);
                entity.SaveChanges();
            }
        }

        public List<JobModel> SearchJobs()                                                      //点击首页的工作按钮 可以跳入寻找工作的界面 返回的是JobModel的列表类型                           
        {
            using (Entities entity = new Entities())
            {
                var x = (from u in entity.POS_RELATED_FIELD
                         from v in entity.WANTED_POS
                         from w in entity.COMPANY
                         from y in entity.POS_RELEASE
                         where u.POS_ID == v.POS_ID && y.C_ID == w.C_ID && v.POS_ID == y.POS_ID
                         select new JobModel
                         {
                             POS_ID = v.POS_ID,
                             POS_NAME = v.POS_NAME,
                             SALARY = v.SALARY,
                             DESCRIPTION = v.POS_DESCRIPTION,
                             RELATED_FIELD = u.FIELD_NAME,
                             C_ID = w.C_ID,
                             COMP_NAME = w.COMP_NAME
                         
                         }).AsEnumerable().ToList();

                return x;
            }
        }

        public void AddJob(int C_ID,string pos_name,int salary,string description,string field)                             //需要添加公司需要的职业 修改三个表  就寻找最大的一个数 wanted_pos pos_release pos_related_field
        {
            using (Entities entity = new Entities())
            {
                var last = entity.WANTED_POS.Max(pos => pos.POS_ID);
                var wanted_pos = new WANTED_POS();
                wanted_pos.POS_ID = last + 1;
                wanted_pos.POS_NAME = pos_name;
                wanted_pos.SALARY = salary;
                wanted_pos.POS_DESCRIPTION = description;

                entity.WANTED_POS.Add(wanted_pos);

                var pos_release = new POS_RELEASE();
                pos_release.POS_ID = last + 1;
                pos_release.C_ID = C_ID;

                entity.POS_RELEASE.Add(pos_release);

                var pos_related_field = new POS_RELATED_FIELD();
                pos_related_field.POS_ID = last + 1;
                pos_related_field.FIELD_NAME = field;

                entity.POS_RELATED_FIELD.Add(pos_related_field);

                entity.SaveChanges();
            }
        }

        public void DeleteJob(int POS_ID)                                   //根据posid去三个表中删除数据
        {
            using (Entities entity = new Entities())
            {
                var deletePosition = from v in entity.WANTED_POS
                                     where v.POS_ID == POS_ID
                                     select v;

                foreach(var v in deletePosition)
                {
                    entity.WANTED_POS.Remove(v);
                }

                var deleteRelease = from v in entity.POS_RELEASE
                                     where v.POS_ID == POS_ID
                                     select v;
                foreach (var v in deleteRelease)
                {
                    entity.POS_RELEASE.Remove(v);
                }

                var deleteField = from v in entity.POS_RELATED_FIELD
                                    where v.POS_ID == POS_ID
                                    select v;
                foreach (var v in deleteField)
                {
                    entity.POS_RELATED_FIELD.Remove(v);
                }

                entity.SaveChanges();
            }
        }

        public job JobDetails(int pos_id)
        {
            using (Entities entity = new Entities())
            {
                var x = (from v in entity.WANTED_POS
                         from u in entity.POS_RELEASE
                         where v.POS_ID == u.POS_ID && v.POS_ID == pos_id
                         select new job
                         {
                             pos_id = v.POS_ID,
                             pos_name = v.POS_NAME,
                             salary = v.SALARY,
                             pos_description = v.POS_DESCRIPTION,
                             c_id = u.C_ID
                         }).AsEnumerable().ToList();
                return x[0];
            }
        }

        public List<job> CompanyJobs(int c_id)
        {
            using (Entities entity = new Entities())
            {
                var x = (from u in entity.WANTED_POS
                         from v in entity.POS_RELEASE
                         where u.POS_ID == v.POS_ID && v.C_ID == c_id
                         select new job
                         {
                             pos_id = u.POS_ID,
                             pos_name = u.POS_NAME,
                             salary = u.SALARY,
                             pos_description = u.POS_DESCRIPTION,
                             c_id = v.C_ID
                         }).AsEnumerable().ToList();
                return x;
            }
        }

        public List<ResumeInfo> CompanyResume(int c_id)
        {
            using (Entities entity = new Entities())
            {
                var x = (from u in entity.DOWNLOAD_RESUME
                         from v in entity.RESUME
                         from w in entity.STUDENT
                         where u.C_ID == c_id && u.S_ID == v.S_ID && u.OCCUPATION == v.OCCUPATION && u.S_ID == w.S_ID
                         select new ResumeInfo
                         {
                             s_id = u.S_ID,
                             s_name = w.NAME,
                             occupation = u.OCCUPATION,
                             doc_path = v.DOC_PATH
                         }).AsEnumerable().ToList();
                foreach(var each in x)
                {
                    each.points = studentPoint(each.s_id);
                }
                return x;
            }
        }

        public int studentPoint(int s_id)
        {
            using (Entities entity = new Entities())
            {
                var sum = (from v in entity.ANSWER_TO_Q
                           where v.S_ID == s_id
                           select v).Sum(t => t.POINTS);
                return Convert.ToInt32(sum);
            }
        }

        public student StudentInfo(int S_ID)
        {
            using (Entities entity = new Entities())
            {
                var x = (from v in entity.STUDENT
                         where v.S_ID == S_ID
                         select new student {
                             S_ID = v.S_ID,
                             NAME = v.NAME,
                             COLLEGE_NAME = v.COLLEGE_NAME,
                             MAJOR = v.MAJOR,
                             DEGREE = v.DEGREE,
                             ADMITTED_YEAR = v.ADMITTED_YEAR,
                             EMAIL = v.EMAIL,
                             PHONE_NUM = v.PHONE_NUM,
                             S_KEY = v.S_KEY
                         }).AsEnumerable().ToList();
                return x[0];
            }
        }

        public List<StudentAnswer> StudentAnswer(int S_ID)
        {
            using (Entities entity = new Entities())
            {
                var x = (from v in entity.PROMPT_Q
                         from u in entity.QUESTION
                         where v.Q_NUM == u.Q_NUM && v.S_ID == S_ID
                         select new StudentAnswer
                         {
                             q_num = v.Q_NUM,
                             title = u.TITLE
                         }).AsEnumerable().ToList();
                return x;
            }
        }

        public company CompanyInfo(int C_ID)
        {
            using (Entities entity = new Entities())
            {
                var x = (from v in entity.COMPANY
                         where v.C_ID == C_ID
                         select new company
                         {
                             c_id = v.C_ID,
                             comp_name = v.COMP_NAME,
                             comp_type = v.COMP_TYPE,
                             comp_address = v.COMP_ADDRESS,
                             comp_email = v.COMP_EMAIL,
                             comp_phone = v.COMP_PHONE
                         }).AsEnumerable().ToList();
                return x[0];
            }
        }

        public void UpdateStudent(student student)
        {
            using (Entities entity = new Entities())
            {
                var x = (from v in entity.STUDENT
                         where v.S_ID == student.S_ID
                         select v).First();
                x.S_ID = student.S_ID;
                x.NAME = student.NAME;
                x.COLLEGE_NAME = student.COLLEGE_NAME;
                x.MAJOR = student.MAJOR;
                x.DEGREE = student.DEGREE;
                x.ADMITTED_YEAR = student.ADMITTED_YEAR;
                x.EMAIL = student.EMAIL;
                x.PHONE_NUM = student.PHONE_NUM;
                entity.SaveChanges();
            }
        }

        public void UpdateCompany(company company)
        {
            using (Entities entity = new Entities())
            {
                var x = (from v in entity.COMPANY
                         where v.C_ID == company.c_id
                         select v).First();
                x.C_ID = company.c_id;
                x.COMP_NAME = company.comp_name;
                x.COMP_TYPE = company.comp_type;
                x.COMP_ADDRESS = company.comp_address;
                x.COMP_EMAIL = company.comp_email;
                x.COMP_PHONE = company.comp_phone;
                entity.SaveChanges();
            }
        }

        public void AddResume(int s_id,int c_id,string occupation,string doc_path)
        {
            using (Entities entity = new Entities())
            {
                entity.RESUME.Add(new RESUME
                {
                    S_ID = s_id,
                    OCCUPATION = occupation,
                    DOC_PATH = doc_path
                });

                entity.DOWNLOAD_RESUME.Add(new DOWNLOAD_RESUME
                {
                    C_ID = c_id,
                    S_ID = s_id,
                    OCCUPATION = occupation
                });

                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges

                    entity.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
        }
    }

}