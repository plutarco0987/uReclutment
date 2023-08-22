using DataAccess.Generic;
using Entities;
using Entities.DataContext;
using Entities.Formats;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace uReclutment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {
        private readonly IGenericRepository<Candidates> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private FormatData<Candidates> _formatData;
        public CandidatesController(IGenericRepository<Candidates> genericRepository, IUnitOfWork unitOfWork)
        {
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
            this._formatData= new FormatData<Candidates>();
        }

        [Route("GetAllCandidates")]
        [Produces("application/json")]
        [HttpGet ]
        public async Task<FormatData<CandidatesFormat>> GetAllCandidates()
        {
            string locationError = string.Empty;
            FormatData<CandidatesFormat> formatData = new FormatData<CandidatesFormat>();
            try
            {
                locationError = "GetAllCandidates";
                IEnumerable<Candidates> result = await _genericRepository.GetAsync();
                List<CandidatesFormat> resultGood = new List<CandidatesFormat>();
                foreach (var item in result)
                {
                    resultGood.Add(new CandidatesFormat(item.CandidatesId, item.StagesId, item.VacancyId, item.Name, item.Age, item.Address, item.City, item.Country, item.Active, item.NameCreated, item.DateCreated, item.NameModified, item.DateModified, item.Notes, item.RecluterName,item.Tags,item.ContactSource,item.RejectionEmcor,item.RejectionCandidate));
                }
                formatData = new FormatData<CandidatesFormat>(resultGood, true, 201,Constans.GetAll(ConstansType.Candidate));                 
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if(result)
                    formatData = new FormatData<CandidatesFormat>(new List<CandidatesFormat>(), false, 501, Constans.Error(ConstansType.Candidate), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<CandidatesFormat>(new List<CandidatesFormat>(), false, 501, Constans.Error(ConstansType.Candidate), "Error in Log", locationError, "Error");
            }

            return formatData;
        }

        [Route("GetById/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<Candidates>> GetById(int id)
        {
            string locationError = string.Empty;
            try
            {                                    
                locationError = "GetById";
                Candidates result = await _genericRepository.GetById(id);
                if(result==null)
                    return new FormatData<Candidates>(new List<Candidates>(), false, 201,Constans.ErrorFound(ConstansType.Candidate), "Object not found", locationError);

                List<Candidates> resultCandidates = new List<Candidates>();
                resultCandidates.Add(result);
                locationError = "FormatData";
                _formatData = new FormatData<Candidates>(resultCandidates, true, 201,Constans.Get(ConstansType.Candidate));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Candidates>(new List<Candidates>(), false, 501, Constans.Error(ConstansType.Candidate), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Candidates>(new List<Candidates>(), false, 501, Constans.Error(ConstansType.Candidate), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("AddCandidates")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<FormatData<CandidatesFormat>> AddCandidates([FromBody] CandidatesFormat Candidates)
        {
            string locationError = string.Empty;
            FormatData<CandidatesFormat> formatData = new FormatData<CandidatesFormat>(new List<CandidatesFormat>(), false, 501, Constans.ErrorFound(ConstansType.Candidate), "ModelState", locationError);
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    formatData = new FormatData<CandidatesFormat>(new List<CandidatesFormat>(), false, 501, Constans.ErrorFound(ConstansType.Candidate), "ModelState", locationError);                    
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the Candidates

                    Candidates candidates = new Candidates(Candidates);
                    DateTime date= DateTime.Now;
                    Candidates.DateCreated = date;
                    Candidates.DateModified = date;                    

                    locationError = "RepositoryCreate";
                    bool result = await _genericRepository.CreateAsync(candidates);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<CandidatesFormat> resultCandidates = new List<CandidatesFormat>();
                    resultCandidates.Add(Candidates);
                    locationError = "FormatData";
                    formatData = new FormatData<CandidatesFormat>(resultCandidates, result, 201,Constans.Add(ConstansType.Candidate));
                }                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<CandidatesFormat>(new List<CandidatesFormat>(), false, 501, Constans.Error(ConstansType.Candidate), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<CandidatesFormat>(new List<CandidatesFormat>(), false, 501, Constans.Error(ConstansType.Candidate), "Error in Log", locationError, "Error");
            }
            return formatData;
        }

        [Route("UpdateCandidates/{id}")]
        [Produces("application/json")]
        [HttpPut]
        public async Task<FormatData<CandidatesFormat>> UpdateCandidates(int id,[FromBody] CandidatesFormat Candidates)
        {
            string locationError = string.Empty;
            FormatData<CandidatesFormat> formatData = new FormatData<CandidatesFormat>(new List<CandidatesFormat>(), false, 501, Constans.ErrorFound(ConstansType.Candidate), "ModelState", locationError);
            try
            {
                locationError = "ModelState";                
                if (!ModelState.IsValid)
                {
                    formatData = new FormatData<CandidatesFormat>(new List<CandidatesFormat>(), false, 501, Constans.InvalidObject(ConstansType.Candidate), "ModelState",locationError);
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the Candidates
                    
                    Candidates candidates = await _genericRepository.GetById(id);
                    
                    candidates.StagesId = Candidates.StagesId;
                    candidates.VacancyId = Candidates.VacancyId;
                    candidates.Name = Candidates.Name;
                    candidates.Age = Candidates.Age;
                    candidates.Address = Candidates.Address;
                    candidates.City = Candidates.City;
                    candidates.Country= Candidates.Country;
                    candidates.Active= Candidates.Active;
                    candidates.Notes = Candidates.Notes;
                    candidates.RecluterName = Candidates.RecluterName;
                    candidates.Tags = Candidates.Tags;
                    candidates.ContactSource = Candidates.ContactSource;
                    candidates.RejectionEmcor = Candidates.RejectionEmcor;
                    candidates.RejectionCandidate = Candidates.RejectionCandidate;


                    candidates.NameModified= Candidates.NameModified;
                    candidates.DateModified = DateTime.Now;

                    locationError = "RepositoryUpdate";
                    bool result = _genericRepository.Update(candidates);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<CandidatesFormat> resultCandidates = new List<CandidatesFormat>();
                    resultCandidates.Add(Candidates);
                    locationError = "FormatData";
                    formatData = new FormatData<CandidatesFormat>(resultCandidates, result, 201,Constans.Update(ConstansType.Candidate,id));
                }
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<CandidatesFormat>(new List<CandidatesFormat>(), false, 501, Constans.Error(ConstansType.Candidate), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<CandidatesFormat>(new List<CandidatesFormat>(), false, 501, Constans.Error(ConstansType.Candidate), "Error in Log", locationError, "Error");
            }
            return formatData;
        }

        [Route("DeleteCandidates/{id}")]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<FormatData<Candidates>> DeleteCandidates(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetId";
                Candidates requestOriginal =  await _genericRepository.GetById(id);
                if(requestOriginal==null)
                    return new FormatData<Candidates>(new List<Candidates>(), false, 501,Constans.ErrorFound(ConstansType.Candidate), "Object not found", locationError);

                locationError = "UpdateCandidates";
                requestOriginal.Active = false;
                var result = _unitOfWork.Context.Set<Candidates>().Update(requestOriginal);

                locationError = "Commit";
                if (result!=null)
                    _unitOfWork.Commit();

                List<Candidates> resultCandidates = new List<Candidates>();
                resultCandidates.Add(requestOriginal);
                locationError = "FormatData";
                _formatData = new FormatData<Candidates>(resultCandidates, true, 201, Constans.Delete(ConstansType.Candidate,id));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Candidates>(new List<Candidates>(), false, 501, Constans.Error(ConstansType.Candidate), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Candidates>(new List<Candidates>(), false, 501, Constans.Error(ConstansType.Candidate), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }
    }
}