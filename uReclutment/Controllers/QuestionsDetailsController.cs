using DataAccess.Generic;
using Entities;
using Entities.DataContext;
using Entities.Formats;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace uReclutment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionDetailsController : ControllerBase
    {
        private readonly IGenericRepository<QuestionDetails> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private FormatData<QuestionDetails> _formatData;
        public QuestionDetailsController(IGenericRepository<QuestionDetails> genericRepository, IUnitOfWork unitOfWork)
        {
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
            this._formatData= new FormatData<QuestionDetails>();
        }

        [Route("GetAllQuestionDetails")]
        [Produces("application/json")]
        [HttpGet ]
        public async Task<FormatData<QuestionDetailsFormat>> GetAllQuestionDetails()
        {
            string locationError = string.Empty;
            FormatData<QuestionDetailsFormat>  formatData = new FormatData<QuestionDetailsFormat>();
            try
            {
                locationError = "GetAllQuestionDetails";
                List<Questions> questions= await _unitOfWork.Context.Set<Questions>().ToListAsync();
                List<Candidates> candidates = await _unitOfWork.Context.Set<Candidates>().ToListAsync();
                List<QuestionDetailsFormat> formats = new List<QuestionDetailsFormat>();
                IEnumerable<QuestionDetails> result = await _genericRepository.GetAsync();  
                foreach (QuestionDetails question in result)
                {
                    Questions x =questions.Find(x => x.QuestionsId == question.QuestionsId);
                    Candidates c = candidates.Find(x => x.CandidatesId == question.CandidatesId);
                    formats.Add(new QuestionDetailsFormat(question.QuestionDetailsId, c != null ? c.Name : "", x != null ? x.Question : "", question.Answer, question.Active, question.QuestionsId, question.CandidatesId, question.DateCreated, question.DateModified));
                }
                formatData = new FormatData<QuestionDetailsFormat>(formats, true, 201,Constans.GetAll(ConstansType.Question));                 
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if(result)
                    formatData = new FormatData<QuestionDetailsFormat>(new List<QuestionDetailsFormat>(), false, 501, Constans.Error(ConstansType.Question), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<QuestionDetailsFormat>(new List<QuestionDetailsFormat>(), false, 501, Constans.Error(ConstansType.Question), "Error in Log", locationError, "Error");
            }

            return formatData;
        }

        [Route("GetQuestionDetailsByCandidateeId/{candidateId}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<QuestionDetails>> GetQuestionDetailsByCandidateeId(int candidateId)
        {
            string locationError = string.Empty;
            FormatData<QuestionDetails> formatData = new FormatData<QuestionDetails>();
            try
            {
                locationError = "GetAllQuestionDetails";
                IEnumerable<QuestionDetails> result = await _genericRepository.GetAsync();
                result = result.Where(x => x.Active && x.CandidatesId== candidateId);
                
                formatData = new FormatData<QuestionDetails>(result, true, 201, Constans.GetAll(ConstansType.Question));
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<QuestionDetails>(new List<QuestionDetails>(), false, 501, Constans.Error(ConstansType.Question), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<QuestionDetails>(new List<QuestionDetails>(), false, 501, Constans.Error(ConstansType.Question), "Error in Log", locationError, "Error");
            }

            return formatData;
        }


        [Route("GetById/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<QuestionDetails>> GetById(int id)
        {
            string locationError = string.Empty;
            try
            {                                    
                locationError = "GetById";
                QuestionDetails result = await _genericRepository.GetById(id);
                if(result==null)
                    return new FormatData<QuestionDetails>(new List<QuestionDetails>(), false, 201,Constans.ErrorFound(ConstansType.Question), "Object not found", locationError);
                
                List<QuestionDetails> resultQuestionDetails = new List<QuestionDetails>();
                resultQuestionDetails.Add(result);
                locationError = "FormatData";
                _formatData = new FormatData<QuestionDetails>(resultQuestionDetails, true, 201,Constans.Get(ConstansType.Question));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<QuestionDetails>(new List<QuestionDetails>(), false, 501, Constans.Error(ConstansType.Question), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<QuestionDetails>(new List<QuestionDetails>(), false, 501, Constans.Error(ConstansType.Question), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("AddQuestionDetails")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<FormatData<QuestionDetailsFormat>> AddQuestionDetails([FromBody] QuestionDetailsFormat QuestionDetails)
        {
            string locationError = string.Empty;
            FormatData<QuestionDetailsFormat> formatData = new FormatData<QuestionDetailsFormat>();
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    formatData = new FormatData<QuestionDetailsFormat>(new List<QuestionDetailsFormat>(), false, 501, Constans.ErrorFound(ConstansType.Question), "ModelState", locationError);                    
                }
                else if (QuestionDetails.CandidatesId == 0 || QuestionDetails.QuestionsId==0)
                {
                    if(QuestionDetails.CandidatesId== 0)
                        formatData = new FormatData<QuestionDetailsFormat>(new List<QuestionDetailsFormat>(), false, 501, Constans.CustomMessages(4));
                    else
                        formatData = new FormatData<QuestionDetailsFormat>(new List<QuestionDetailsFormat>(), false, 501, Constans.CustomMessages(3));
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the QuestionDetails
                    DateTime now = DateTime.Now;
                    QuestionDetails.DateCreated = now;
                    QuestionDetails.DateModified = now;
                    QuestionDetails question = new QuestionDetails(QuestionDetails);



                    
                    locationError = "RepositoryCreate";
                    bool result = await _genericRepository.CreateAsync(question);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<QuestionDetailsFormat> resultQuestionDetails = new List<QuestionDetailsFormat>();
                    resultQuestionDetails.Add(QuestionDetails);
                    locationError = "FormatData";
                    formatData = new FormatData<QuestionDetailsFormat>(resultQuestionDetails, result, 201,Constans.Add(ConstansType.Question));
                }                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<QuestionDetailsFormat>(new List<QuestionDetailsFormat>(), false, 501, Constans.Error(ConstansType.Question), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<QuestionDetailsFormat>(new List<QuestionDetailsFormat>(), false, 501, Constans.Error(ConstansType.Question), "Error in Log", locationError, "Error");
            }
            return formatData;
        }

        [Route("UpdateQuestionDetails/{id}")]
        [Produces("application/json")]
        [HttpPut]
        public async Task<FormatData<QuestionDetailsFormat>> UpdateQuestionDetails(int id,[FromBody] QuestionDetailsFormat QuestionDetails)
        {
            string locationError = string.Empty;
            FormatData<QuestionDetailsFormat> formatData = new FormatData<QuestionDetailsFormat>();
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    formatData = new FormatData<QuestionDetailsFormat>(new List<QuestionDetailsFormat>(), false, 501, Constans.InvalidObject(ConstansType.Question), "ModelState", locationError);
                }
                else if (QuestionDetails.CandidatesId == 0 || QuestionDetails.QuestionsId == 0)
                {
                    if (QuestionDetails.CandidatesId == 0)
                        formatData = new FormatData<QuestionDetailsFormat>(new List<QuestionDetailsFormat>(), false, 501, Constans.CustomMessages(4));
                    else
                        formatData = new FormatData<QuestionDetailsFormat>(new List<QuestionDetailsFormat>(), false, 501, Constans.CustomMessages(3));
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the QuestionDetails
                    //
                    QuestionDetails.DateModified = DateTime.Now;
                    QuestionDetails details = await _genericRepository.GetById(id);
                    details.QuestionDetailsUpdate(QuestionDetails);




                    
                    locationError = "RepositoryUpdate";
                    bool result = _genericRepository.Update(details);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<QuestionDetailsFormat> resultQuestionDetails = new List<QuestionDetailsFormat>();
                    resultQuestionDetails.Add(QuestionDetails);
                    locationError = "FormatData";
                    formatData = new FormatData<QuestionDetailsFormat>(resultQuestionDetails, result, 201,Constans.Update(ConstansType.Question,id));
                }
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<QuestionDetailsFormat>(new List<QuestionDetailsFormat>(), false, 501, Constans.Error(ConstansType.Question), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<QuestionDetailsFormat>(new List<QuestionDetailsFormat>(), false, 501, Constans.Error(ConstansType.Question), "Error in Log", locationError, "Error");
            }
            return formatData;
        }

        [Route("DeleteQuestionDetails/{id}")]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<FormatData<QuestionDetails>> DeleteQuestionDetails(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetId";
                QuestionDetails requestOriginal =  await _genericRepository.GetById(id);
                if(requestOriginal==null)
                    return new FormatData<QuestionDetails>(new List<QuestionDetails>(), false, 501,Constans.ErrorFound(ConstansType.Question), "Object not found", locationError);

                locationError = "UpdateQuestionDetails";
                requestOriginal.Active = false;
                var result = _unitOfWork.Context.Set<QuestionDetails>().Update(requestOriginal);

                locationError = "Commit";
                if (result!=null)
                    _unitOfWork.Commit();

                List<QuestionDetails> resultQuestionDetails = new List<QuestionDetails>();
                resultQuestionDetails.Add(requestOriginal);
                locationError = "FormatData";
                _formatData = new FormatData<QuestionDetails>(resultQuestionDetails, true, 201, Constans.Delete(ConstansType.Question,id));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<QuestionDetails>(new List<QuestionDetails>(), false, 501, Constans.Error(ConstansType.Question), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<QuestionDetails>(new List<QuestionDetails>(), false, 501, Constans.Error(ConstansType.Question), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }
    }
}