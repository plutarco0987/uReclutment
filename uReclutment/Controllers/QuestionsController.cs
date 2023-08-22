using DataAccess.Generic;
using Entities;
using Entities.DataContext;
using Entities.Formats;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using System;

namespace uReclutment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IGenericRepository<Questions> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private FormatData<Questions> _formatData;
        public QuestionsController(IGenericRepository<Questions> genericRepository, IUnitOfWork unitOfWork)
        {
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
            this._formatData = new FormatData<Questions>();
        }

        [Route("GetAllQuestions")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<QuestionsFormat>> GetAllQuestions()
        {
            string locationError = string.Empty;            
            FormatData<QuestionsFormat> formatData = new FormatData<QuestionsFormat>();
            try
            {
                locationError = "GetAllQuestions";
                IEnumerable<Questions> result = await _genericRepository.GetAsync();
                List<QuestionsFormat> formats = new List<QuestionsFormat>();
                foreach (var item in result)
                {
                    formats.Add(new QuestionsFormat(item.QuestionsId, item.EnumTypeId, item.VacancyId, item.Question, item.Required, item.MaxLength, item.Active, item.NameCreated, item.DateCreated, item.NameModified, item.DateModified));
                }

                formatData = new FormatData<QuestionsFormat>(formats, true, 201, Constans.GetAll(ConstansType.Question));
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<QuestionsFormat>(new List<QuestionsFormat>(), false, 501, Constans.Error(ConstansType.Question), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<QuestionsFormat>(new List<QuestionsFormat>(), false, 501, Constans.Error(ConstansType.Question), "Error in Log", locationError, "Error");
            }
            return formatData;
        }

        [Route("GetAllQuestionsByVacancy/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<QuestionsFormat>> GetAllQuestionsByVacancy(int id)
        {
            string locationError = string.Empty;
            FormatData<QuestionsFormat> formatData = new FormatData<QuestionsFormat>();
            try
            {
                locationError = "GetAllQuestions";
                IEnumerable<Questions> result = await _genericRepository.GetAsync();
                result = result.Where(x => x.Active && x.VacancyId==id).ToList();

                List<QuestionsFormat> formats = new List<QuestionsFormat>();
                foreach (var item in result)
                {
                    formats.Add(new QuestionsFormat(item.QuestionsId, item.EnumTypeId, item.VacancyId, item.Question, item.Required, item.MaxLength, item.Active, item.NameCreated, item.DateCreated, item.NameModified, item.DateModified));
                }

                formatData = new FormatData<QuestionsFormat>(formats, true, 201, Constans.GetAll(ConstansType.Question));
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<QuestionsFormat>(new List<QuestionsFormat>(), false, 501, Constans.Error(ConstansType.Question), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<QuestionsFormat>(new List<QuestionsFormat>(), false, 501, Constans.Error(ConstansType.Question), "Error in Log", locationError, "Error");
            }
            return formatData;
        }


        [Route("GetById/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<Questions>> GetById(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetById";
                Questions result = await _genericRepository.GetById(id);
                if (result == null)
                    return new FormatData<Questions>(new List<Questions>(), false, 201, Constans.ErrorFound(ConstansType.Question), "Object not found", locationError);

                result.EnumType = null;
                List<Questions> resultQuestions = new List<Questions>();
                resultQuestions.Add(result);
                locationError = "FormatData";
                _formatData = new FormatData<Questions>(resultQuestions, true, 201, Constans.Get(ConstansType.Question));
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Questions>(new List<Questions>(), false, 501, Constans.Error(ConstansType.Question), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Questions>(new List<Questions>(), false, 501, Constans.Error(ConstansType.Question), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }


        [Route("GetByVacancyId/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<Questions>> GetByVacancyId(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetById";
                List<Questions> result = await _unitOfWork.Context.Set<Questions>().ToListAsync();
                result = result.Where(x => x.Active && x.VacancyId == id).ToList();

                if (result == null)
                    return new FormatData<Questions>(new List<Questions>(), false, 201, Constans.ErrorFound(ConstansType.Question), "Object not found", locationError);
                                
                locationError = "FormatData";
                _formatData = new FormatData<Questions>(result, true, 201, Constans.Get(ConstansType.Question));
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Questions>(new List<Questions>(), false, 501, Constans.Error(ConstansType.Question), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Questions>(new List<Questions>(), false, 501, Constans.Error(ConstansType.Question), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }


        [Route("AddQuestions")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<FormatData<QuestionsFormat>> AddQuestions([FromBody] QuestionsFormat Questions)
        {
            string locationError = string.Empty;
            FormatData<QuestionsFormat> formatData = new FormatData<QuestionsFormat>();
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    formatData = new FormatData<QuestionsFormat>(new List<QuestionsFormat>(), false, 501, Constans.ErrorFound(ConstansType.Question), "ModelState", locationError);
                }
                else if (Questions.VacancyId == 0 || Questions.EnumTypeId == 0)
                {
                    if (Questions.VacancyId == 0)
                        formatData = new FormatData<QuestionsFormat>(new List<QuestionsFormat>(), false, 501, Constans.CustomMessages(1));
                    else
                        formatData = new FormatData<QuestionsFormat>(new List<QuestionsFormat>(), false, 501, Constans.CustomMessages(2));
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the Questions

                    Questions questions = new Questions(Questions);
                    DateTime now = DateTime.Now;
                    questions.DateCreated = now;
                    questions.DateModified = now;
                    locationError = "RepositoryCreate";
                    bool result = await _genericRepository.CreateAsync(questions);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<QuestionsFormat> resultQuestions = new List<QuestionsFormat>();
                    resultQuestions.Add(Questions);
                    locationError = "FormatData";
                    formatData = new FormatData<QuestionsFormat>(resultQuestions, result, 201, Constans.Add(ConstansType.Question));
                }
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<QuestionsFormat>(new List<QuestionsFormat>(), false, 501, Constans.Error(ConstansType.Question), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<QuestionsFormat>(new List<QuestionsFormat>(), false, 501, Constans.Error(ConstansType.Question), "Error in Log", locationError, "Error");
            }
            return formatData;
        }

        [Route("UpdateQuestions/{id}")]
        [Produces("application/json")]
        [HttpPut]
        public async Task<FormatData<QuestionsFormat>> UpdateQuestions(int id, [FromBody] QuestionsFormat Questions)
        {
            string locationError = string.Empty;
            FormatData<QuestionsFormat> formatData = new FormatData<QuestionsFormat>();
            try
            {
                locationError = "ModelState";

                if (!ModelState.IsValid)
                {
                    formatData = new FormatData<QuestionsFormat>(new List<QuestionsFormat>(), false, 501, Constans.InvalidObject(ConstansType.Question), "ModelState", locationError);
                }
                else if (Questions.VacancyId == 0 || Questions.EnumTypeId == 0)
                {
                    if (Questions.VacancyId == 0)
                        formatData = new FormatData<QuestionsFormat>(new List<QuestionsFormat>(), false, 501, Constans.CustomMessages(1));
                    else
                        formatData = new FormatData<QuestionsFormat>(new List<QuestionsFormat>(), false, 501, Constans.CustomMessages(2));
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the Questions                    
                    Questions questions1 = await _genericRepository.GetById(id);

                    if (questions1 != null)
                    {                        
                        questions1.EnumTypeId = Questions.EnumTypeId;
                        questions1.VacancyId = Questions.VacancyId;
                        questions1.Question = Questions.Question;
                        questions1.Required = Questions.Required;
                        questions1.MaxLength = Questions.MaxLength;
                        questions1.Active = Questions.Active;                        
                        questions1.NameModified = Questions.NameModified;
                        questions1.DateModified = DateTime.Now;

                        locationError = "RepositoryUpdate";
                        bool result = _genericRepository.Update(questions1);
                        locationError = "Commit";
                        if (result)
                            _unitOfWork.Commit();

                        List<QuestionsFormat> resultQuestions = new List<QuestionsFormat>();
                        resultQuestions.Add(Questions);
                        locationError = "FormatData";
                        formatData = new FormatData<QuestionsFormat>(resultQuestions, result, 201, Constans.Update(ConstansType.Question, id));
                    }
                    else
                        formatData = new FormatData<QuestionsFormat>(new List<QuestionsFormat>(), false, 501, Constans.Error(ConstansType.Question), "Question doesn't exist", locationError, "Error");

                }
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<QuestionsFormat>(new List<QuestionsFormat>(), false, 501, Constans.Error(ConstansType.Question), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<QuestionsFormat>(new List<QuestionsFormat>(), false, 501, Constans.Error(ConstansType.Question), "Error in Log", locationError, "Error");
            }
            return formatData;
        }

        [Route("DeleteQuestions/{id}")]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<FormatData<Questions>> DeleteQuestions(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetId";
                Questions requestOriginal = await _genericRepository.GetById(id);
                if (requestOriginal == null)
                    return new FormatData<Questions>(new List<Questions>(), false, 501, Constans.ErrorFound(ConstansType.Question), "Object not found", locationError);

                locationError = "UpdateQuestions";
                requestOriginal.Active = false;
                var result = _unitOfWork.Context.Set<Questions>().Update(requestOriginal);

                locationError = "Commit";
                if (result != null)
                    _unitOfWork.Commit();

                List<Questions> resultQuestions = new List<Questions>();
                resultQuestions.Add(requestOriginal);
                locationError = "FormatData";
                _formatData = new FormatData<Questions>(resultQuestions, true, 201, Constans.Delete(ConstansType.Question, id));
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Questions>(new List<Questions>(), false, 501, Constans.Error(ConstansType.Question), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Questions>(new List<Questions>(), false, 501, Constans.Error(ConstansType.Question), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }
    }
}