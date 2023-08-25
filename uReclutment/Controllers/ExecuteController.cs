using DataAccess.Generic;
using Entities;
using Entities.DataContext;
using Entities.Formats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using NuGet.Packaging.Signing;
using NuGet.Protocol;
using System;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace uReclutment.Controllers
{
    public class ArchivoModel
    {
        public IFormFile Archivo { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ExecuteController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private CandidatesController _candidates;

        private VacancyController _vacancy;
        private QuestionsController _questions;
        private QuestionDetailsController _answers;
        private CommentsController _comments;

        private FormatData<string> _formatData;
        public ExecuteController(IGenericRepository<Comments> repositoryComments, IGenericRepository<Questions> repositoryQuestions, IGenericRepository<QuestionDetails> repositoryQuestionsDetails, IGenericRepository<Vacancy> repositoryVacancy, IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._vacancy = new VacancyController(repositoryVacancy, unitOfWork);
            this._questions = new QuestionsController(repositoryQuestions, unitOfWork);
            this._answers = new QuestionDetailsController(repositoryQuestionsDetails, unitOfWork);
            this._comments = new CommentsController(repositoryComments, unitOfWork);
        }

        [Route("DeleteEveritingCandidate/{id}")]
        [Produces("application/json")]
        [HttpGet ]
        public async Task<bool> DeleteEveritingCandidate(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "DeleteEveritingCandidate";
                FormatData<Candidates> candidates = await _candidates.GetById(id);
                QuestionDetails[] questionDetails;
                Meetings[] meetings;
                if (candidates.Data==null)
                {
                    if (candidates.Data.Count() == 0)
                        return false;
                    else
                    {
                        questionDetails= candidates.Data.First().QuestionDetails.ToArray();
                        meetings= candidates.Data.First().Meetings.ToArray();
                        foreach (var item in meetings)
                        {
                            item.CandidatesId = 1;
                        }
                        //remove the question dewtails first
                        _unitOfWork.Context.Set<QuestionDetails>().RemoveRange(questionDetails);
                        //Delete the meetings NOTE: will be update the meeting to the first Candidate that is the id 1 for can keep the information
                        _unitOfWork.Context.Set<Meetings>().UpdateRange(meetings);
                        //Delete the candidate
                        _unitOfWork.Context.Set<Candidates>().Remove(candidates.Data.First());
                        //commite to save the infortmation 
                        _unitOfWork.Commit();
                    }
                                            
                }
            }
            catch (Exception ex)
            {
                bool good;
                try
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    EntityEntry<Log> result = await _unitOfWork.Context.Set<Log>().AddAsync(new Log(0, ex.Message, ex.StackTrace != null ? ex.StackTrace : string.Empty, DateTime.Now, true));
#pragma warning restore CS8604 // Possible null reference argument.
                    good = true;
                }
                catch (Exception ex2)
                {
                    good = false;
                }

                if (good)
                    _formatData = new FormatData<string>(new List<string>(), false, 501, Constans.Error(ConstansType.Setting), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<string>(new List<string>(), false, 501, Constans.Error(ConstansType.Setting), "Error in Log", locationError, "Error");
                return false;
            }
            return false;
        }
        public static void Save(string path, byte[] content)
        {
            if (!System.IO.File.Exists(path))
            {
                using (FileStream writeStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    writeStream.Write(content, 0, content.Length);
                    writeStream.Close();
                }
            }            
        }


        [Route("MigrationCandidates/")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<bool> MigrationCandidates(MigrationCandidates migrationCandidates)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "MigrationCandidates";
                List<Candidates> candidates = new List<Candidates>();
                foreach (var item in migrationCandidates.Candidates)
                {
                    candidates.Add(new Candidates(item));
                }


                await _unitOfWork.Context.Set<Candidates>().AddRangeAsync(candidates);

                _unitOfWork.Commit();
                List<int> ids = new List<int>();
                foreach (var item in candidates)
                {
                    ids.Add(item.CandidatesId);
                }
                //Files
                List<Files> listFiles = new List<Files>();
                for (int i = 0; i < migrationCandidates.Files.Count; i++)
                {
                    if(!Directory.Exists(migrationCandidates.Path + "\\" + ids[i]))
                    {
                        Directory.CreateDirectory(migrationCandidates.Path + "\\" + ids[i]);
                    }

                    Save(migrationCandidates.Path + "\\" + ids[i] + "\\" + Path.GetFileName(migrationCandidates.NameFile[i]) , migrationCandidates.Files[i]);

                    Files file = new Files();
                    file.Active = true;
                    file.Path = migrationCandidates.Path + "\\" + ids[i] + "\\" + Path.GetFileName(migrationCandidates.NameFile[i]);
                    file.CandidatesId = ids[i];
                    file.Name = Path.GetFileName(migrationCandidates.NameFile[i]);
                    
                    listFiles.Add(file);
                }

                await _unitOfWork.Context.Set<Files>().AddRangeAsync(listFiles);

                _unitOfWork.Commit();
                return true;

            }
            catch (Exception ex)
            {
                bool good;
                try
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    EntityEntry<Log> result = await _unitOfWork.Context.Set<Log>().AddAsync(new Log(0, ex.Message, ex.StackTrace != null ? ex.StackTrace : string.Empty, DateTime.Now, true));
#pragma warning restore CS8604 // Possible null reference argument.
                    good = true;
                }
                catch (Exception ex2)
                {
                    good = false;
                }

                if (good)
                    _formatData = new FormatData<string>(new List<string>(), false, 501, Constans.Error(ConstansType.Setting), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<string>(new List<string>(), false, 501, Constans.Error(ConstansType.Setting), "Error in Log", locationError, "Error");
                return false;
            }
            return false;
        }


        [Route("AddCustomers")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<bool> AddCustomers(List<Customers> Customers)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "AddRangeAsync Customers";
                await _unitOfWork.Context.Set<Customers>().AddRangeAsync(Customers);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                bool good;
                try
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    EntityEntry<Log> result = await _unitOfWork.Context.Set<Log>().AddAsync(new Log(0, ex.Message, ex.StackTrace != null ? ex.StackTrace : string.Empty, DateTime.Now, true));
#pragma warning restore CS8604 // Possible null reference argument.
                    good = true;
                }
                catch (Exception ex2)
                {
                    good = false;
                }

                if (good)
                    _formatData = new FormatData<string>(new List<string>(), false, 501, Constans.Error(ConstansType.Setting), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<string>(new List<string>(), false, 501, Constans.Error(ConstansType.Setting), "Error in Log", locationError, "Error");
                return false;
            }
            return false;
        }

        [Route("MigrationVacancy")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<bool> MigrationVacancy(MigrationVacancy Vacancy)
        {
            string locationError = string.Empty;

            
            try
            {
                locationError = "ModelState";

                List<Vacancy> vacancies= new List<Vacancy>();
                foreach (var item in Vacancy.Vacancys)
                {
                    Vacancy v = new Vacancy(item);
                    v.Customers = await _unitOfWork.Context.Set<Customers>().FindAsync(v.CustomersId);
                    await _unitOfWork.Context.Set<Vacancy>().AddAsync(v);
                }



                
                _unitOfWork.Commit();
                //_unitOfWork.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                bool good;
                try
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    EntityEntry<Log> result = await _unitOfWork.Context.Set<Log>().AddAsync(new Log(0, ex.Message, ex.StackTrace != null ? ex.StackTrace : string.Empty, DateTime.Now, true));
#pragma warning restore CS8604 // Possible null reference argument.
                    good = true;
                }
                catch (Exception ex2)
                {
                    good = false;
                }

                if (good)
                    _formatData = new FormatData<string>(new List<string>(), false, 501, Constans.Error(ConstansType.Setting), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<string>(new List<string>(), false, 501, Constans.Error(ConstansType.Setting), "Error in Log", locationError, "Error");
                return false;
            }
            return false;
        }
        
        [HttpPost("upload")]
        [Route("AndswersAsync/{vacancyId}-{requestAndswer}-{candidate}")]
        public async Task AndswersAsync([FromForm] ArchivoModel modelo,int vacancyId,string requestAndswer,string candidate)
        {
            //modelo
            List<Questions> qacQuestions = _questions.GetByVacancyId(vacancyId).Result.Data.ToList();
            qacQuestions = qacQuestions.Where(x => x.Active).ToList();
            qacQuestions = qacQuestions.OrderBy(x => x.QuestionsId).ToList();
            List<string> result = requestAndswer.Split(",").ToList();
            List<string> resultCandidate = candidate.Split(",").ToList();
            List<QuestionDetails> questionDetails = new List<QuestionDetails>();
            IEnumerable<Stages> listStages = await _unitOfWork.Context.Set<Stages>().ToListAsync();
            Stages stages = listStages.Where(x => x.Active && x.Order == 1).First();
            DateTime now= DateTime.Now;
            //candidate
            Candidates candidates = new Candidates();
            candidates.Active = true;
            candidates.Name = resultCandidate[0];
            candidates.Address = resultCandidate[1];
            candidates.Age = int.Parse(resultCandidate[2]);
            candidates.Country = resultCandidate[4];
            candidates.City = resultCandidate[3];
            candidates.ContactSource = resultCandidate[5];
            string recluter= resultCandidate[7];
            //this is the EMAIL 
            candidates.Tags = resultCandidate[6];
            candidates.DateCreated = now;
            candidates.DateModified = now;
            candidates.RejectionCandidate = "";
            candidates.RejectionEmcor = "";
            candidates.Meetings = new List<Meetings>();
            candidates.NameCreated = "";
            candidates.NameModified = "";
            candidates.Notes = "";
            candidates.QuestionDetails = new List<QuestionDetails>();
            
            candidates.VacancyId = vacancyId;
            candidates.RecluterName = "";
            candidates.Stages = stages;

            //insert candidate
            await _unitOfWork.Context.Set<Candidates>().AddAsync(candidates);

            for (int i = 0; i < qacQuestions.Count; i++)
            {
                QuestionDetails details = new QuestionDetails();
                details.QuestionDetailsId = 0;
                details.CandidatesId = candidates.CandidatesId;
                details.QuestionsId = qacQuestions[i].QuestionsId;  
                details.Active = true;
                details.DateCreated = now;
                details.DateModified = now;                
                details.Answer = result[i];
                questionDetails.Add(details);
            }
            //insert questiondetails
            await _unitOfWork.Context.Set<QuestionDetails>().AddRangeAsync(questionDetails);


            //file
            IEnumerable<Settings> listSettings = await _unitOfWork.Context.Set<Settings>().ToListAsync();
            List<Settings> list = listSettings.ToList();
            Settings settings = list.Find(x => x.Name == "FilePathLocation" && x.Active);
            string x = requestAndswer;            
            if (settings != null)
            {
                if (modelo.Archivo != null && modelo.Archivo.Length > 0)
                {
                    string path = settings.Value + "\\" + candidates.CandidatesId;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string rutaArchivoDestino = path + "\\" + modelo.Archivo.FileName;

                    using (var stream = new FileStream(rutaArchivoDestino, FileMode.Create))
                    {
                        await modelo.Archivo.CopyToAsync(stream);
                    }
                }
            }


            SendEmail(candidates.Tags, recluter);
        }

        /// <summary>
        /// This methos is sending 2 emails one for the Candidate and another to the recluter for know who sent one information,
        /// the email of that is just information, that format can be edited but we just pass the Name of the candidate
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="recluter"></param>
        public async void SendEmail(string emailTo,string recluter,string nameCandidate)
        {
            string recluterEmail = "",recluterLink="";
            recluterEmail = recluter + "EMAIL";
            string recluterEmailSend = "", recluterLinkResult="";
            recluterLink = recluter + "LINK";
            IEnumerable<Settings> listSettings = await _unitOfWork.Context.Set<Settings>().ToListAsync();
            List<Settings> list = listSettings.ToList();
            string EmailFrom = "", EmailSubjectReset = "", EmailHost = "", SslEmail = "", EmailUseDefaultCredentials = "", PasswordEmail = "", Port = "",Body="", BodyRecluter="";
            foreach (var item in list)
            {
                switch (item.Name)
                {
                    case "EmailFrom":
                        EmailFrom = item.Value != null ? item.Value : "";
                        break;
                    case "EmailSubjectReset":
                        EmailSubjectReset = item.Value != null ? item.Value : "";
                        break;
                    case "EmailHost":
                        EmailHost = item.Value != null ? item.Value : "";
                        break;
                    case "SslEmail":
                        SslEmail = item.Value != null ? item.Value : "";
                        break;
                    case "EmailUseDefaultCredentials":
                        EmailUseDefaultCredentials = item.Value != null ? item.Value : "";
                        break;
                    case "PasswordEmail":
                        PasswordEmail = item.Value != null ? item.Value : "";
                        break;
                    case "Port":
                        Port = item.Value != null ? item.Value : "";
                        break;
                    case "Body":
                        Body = item.Value != null ? item.Value : "";
                        break;
                    case "BodyRecluter":
                        BodyRecluter = item.Value != null ? item.Value : "";
                        break;
                }
                if(item.Name == recluterEmail)
                {
                    recluterEmailSend = item.Value != null ? item.Value : "";
                }
                if (item.Name == recluterLink)
                {
                    recluterLinkResult = item.Value != null ? item.Value : "";
                }

            } 

            MailMessage message = new MailMessage();
            message.From = new MailAddress(EmailFrom);
            message.To.Add(new MailAddress(emailTo));
            message.Subject = EmailSubjectReset;
            message.IsBodyHtml = true;
            message.Body = string.Format(Body, recluterLinkResult);

            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = bool.Parse(SslEmail);
                smtpClient.UseDefaultCredentials = bool.Parse(EmailUseDefaultCredentials);
                smtpClient.Credentials = new NetworkCredential(EmailFrom, PasswordEmail);
                smtpClient.Host = EmailHost;
                smtpClient.Port = int.Parse(Port);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtpClient.Host = "smtp.gmail.com";                               
                //smtpClient.EnableSsl = true;                
                //smtpClient.UseDefaultCredentials = false;
                //smtpClient.Credentials = new NetworkCredential("lucerormzruiz.13@gamil.com", "ogzn-jrbi-jiyc-wjfo");
                //smtpClient.Port = 587;                
                smtpClient.Send(message);
            }

            message = new MailMessage();
            message.From = new MailAddress(EmailFrom);
            message.To.Add(new MailAddress(recluterEmailSend));
            message.Subject = EmailSubjectReset;
            message.IsBodyHtml = true;
            message.Body = string.Format(BodyRecluter, nameCandidate);

            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = bool.Parse(SslEmail);
                smtpClient.UseDefaultCredentials = bool.Parse(EmailUseDefaultCredentials);
                smtpClient.Credentials = new NetworkCredential(EmailFrom, PasswordEmail);
                smtpClient.Host = EmailHost;
                smtpClient.Port = int.Parse(Port);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtpClient.Host = "smtp.gmail.com";                               
                //smtpClient.EnableSsl = true;                
                //smtpClient.UseDefaultCredentials = false;
                //smtpClient.Credentials = new NetworkCredential("lucerormzruiz.13@gamil.com", "ogzn-jrbi-jiyc-wjfo");
                //smtpClient.Port = 587;                
                smtpClient.Send(message);
            }
        }



        [Route("GetActiveList/{type}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<IdClass>> GetActiveList(int type)
        {
            string locationError = string.Empty;
            FormatData<IdClass> formatData = new FormatData<IdClass>();
            try
            {
                locationError = "GetAllActiveList";
                List<IdClass> ids = new List<IdClass>();

                formatData=GetListIdClass(type, _unitOfWork, ids);



            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                await _unitOfWork.Context.Set<Log>().AddAsync(new Log(0, ex.Message, string.Empty, DateTime.Now, true));
#pragma warning restore CS8604 // Possible null reference argument.                
                formatData = new FormatData<IdClass>(new List<IdClass>(), false, 501, Constans.Error(ConstansType.EnumType), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);                
            }

            return formatData;
        }

        public static  FormatData<IdClass> GetListIdClass(int type,IUnitOfWork _unitOfWork, List<IdClass> ids)
        {
            FormatData<IdClass> formatData = new FormatData<IdClass>();
            switch (type)
            {
                case 0:
                    IEnumerable<Customers> resultCustomers =  _unitOfWork.Context.Set<Customers>().ToListAsync().Result;

                    foreach (Customers item in resultCustomers)
                    {
                        if (item.Active)
                            ids.Add(new IdClass(item.CustomersId, item.Name));
                    }


                    formatData = new FormatData<IdClass>(ids, true, 201, "Get all active Customers is okay");
                    break;
                case 1:
                    IEnumerable<Stages> resultStages =  _unitOfWork.Context.Set<Stages>().ToListAsync().Result;

                    foreach (Stages item in resultStages)
                    {
                        if (item.Active)
                            ids.Add(new IdClass(item.StagesId, item.Name));
                    }


                    formatData = new FormatData<IdClass>(ids, true, 201, "Get all active Stages is okay");
                    break;
                case 2:
                    IEnumerable<Vacancy> resultVacancy =  _unitOfWork.Context.Set<Vacancy>().ToListAsync().Result;

                    foreach (Vacancy item in resultVacancy)
                    {
                        if (item.Active)
                            ids.Add(new IdClass(item.VacancyId, item.Name));
                    }


                    formatData = new FormatData<IdClass>(ids, true, 201, "Get all active Vacancy is okay");

                    break;
                case 3:
                    IEnumerable<Candidates> resultCandidates =  _unitOfWork.Context.Set<Candidates>().ToListAsync().Result;

                    foreach (Candidates item in resultCandidates)
                    {
                        if (item.Active)
                            ids.Add(new IdClass(item.CandidatesId, item.Name));
                    }


                    formatData = new FormatData<IdClass>(ids, true, 201, "Get all active Candidates is okay");
                    break;
                case 4:
                    IEnumerable<EnumType> resultEnumType =  _unitOfWork.Context.Set<EnumType>().ToListAsync().Result;

                    foreach (EnumType item in resultEnumType)
                    {
                        if (item.Active)
                            ids.Add(new IdClass(item.EnumTypeId, item.Name));
                    }


                    formatData = new FormatData<IdClass>(ids, true, 201, "Get all active EnumType is okay");
                    break;
          
            }
            return formatData;
        }



        [Route("GetQAC/{candidateId}-{vacancyId}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<QA>> GetQAC(int candidateId, int vacancyId)
        {
            string locationError = string.Empty;
            FormatData<QA> formatData = new FormatData<QA>();



            try
            {
                locationError = "GetQuestions";
                //questions
                List<Questions> qacQuestions = _questions.GetByVacancyId(vacancyId).Result.Data.ToList();
                //comments
                List<Comments> comments = _comments.GetAllCommentsFull().Result.Data.ToList();
                //Answers
                List<QuestionDetails> questionDetails = _answers.GetQuestionDetailsByCandidateeId(candidateId).Result.Data.ToList();

                List<QA> qAs = new List<QA>();

                int order = 1;
                foreach (var item in questionDetails)
                {
                    item.Comments = item.Comments.Where(x => x.Active).ToList();

                    //first question
                    qAs.Add(new QA(order.ToString(), item.Questions.Question, null,0,item.QuestionDetailsId));
                    //second Answer
                    qAs.Add(new QA((order + 1).ToString(), item.Answer, order.ToString(),0, item.QuestionDetailsId));
                    int staticOrder = order + 1;
                    order = order + 2;

                    foreach (var item2 in item.Comments)
                    {
                        qAs.Add(new QA(order.ToString(), item2.Value, staticOrder.ToString(), item2.CommentsId, item.QuestionDetailsId));
                        order++;
                    }
                }
                formatData = new FormatData<QA>(qAs, true, 201, "It's Okay");                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                await _unitOfWork.Context.Set<Log>().AddAsync(new Log(0, ex.Message, string.Empty, DateTime.Now, true));
#pragma warning restore CS8604 // Possible null reference argument.                
                formatData = new FormatData<QA>(new List<QA>(), false, 501, Constans.Error(ConstansType.EnumType), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
            }

            return formatData;
        }
    }
}