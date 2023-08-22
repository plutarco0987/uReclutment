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
    public class RequirementsController : ControllerBase
    {
        private readonly IGenericRepository<Requirements> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private FormatData<Requirements> _formatData;
        public RequirementsController(IGenericRepository<Requirements> genericRepository, IUnitOfWork unitOfWork)
        {
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
            this._formatData= new FormatData<Requirements>();
        }

        [Route("GetAllRequirements")]
        [Produces("application/json")]
        [HttpGet ]
        public async Task<FormatData<Requirements>> GetAllRequirements()
        {
            string locationError = string.Empty;            
            try
            {
                locationError = "GetAllRequirements";
                IEnumerable<Requirements> result = await _genericRepository.GetAsync();
                //we don't need the information of the vacancy just the id is good
                foreach (var item in result)
                {
                    item.Vacancy = null;
                }
                _formatData = new FormatData<Requirements>(result, true, 201,Constans.GetAll(ConstansType.Requirement));                 
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if(result)
                    _formatData = new FormatData<Requirements>(new List<Requirements>(), false, 501, Constans.Error(ConstansType.Requirement), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Requirements>(new List<Requirements>(), false, 501, Constans.Error(ConstansType.Requirement), "Error in Log", locationError, "Error");
            }

            return _formatData;
        }

        [Route("GetAllRequirementsByVacancy/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<Requirements>> GetAllRequirementsByVacancy(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetAllRequirements";
                IEnumerable<Requirements> result = await _genericRepository.GetAsync();
                //we don't need the information of the vacancy just the id is good
                foreach (var item in result)
                {
                    item.Vacancy = null;
                }
                result= result.Where(x=>x.VacancyId==id && x.Active).ToList();


                _formatData = new FormatData<Requirements>(result, true, 201, Constans.GetAll(ConstansType.Requirement));
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Requirements>(new List<Requirements>(), false, 501, Constans.Error(ConstansType.Requirement), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Requirements>(new List<Requirements>(), false, 501, Constans.Error(ConstansType.Requirement), "Error in Log", locationError, "Error");
            }

            return _formatData;
        }



        [Route("GetById/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<Requirements>> GetById(int id)
        {
            string locationError = string.Empty;
            try
            {                                    
                locationError = "GetById";
                Requirements result = await _genericRepository.GetById(id);
                result.Vacancy = null;
                if(result==null)
                    return new FormatData<Requirements>(new List<Requirements>(), false, 201,Constans.ErrorFound(ConstansType.Requirement), "Object not found", locationError);

                List<Requirements> resultRequirements = new List<Requirements>();
                resultRequirements.Add(result);
                locationError = "FormatData";
                _formatData = new FormatData<Requirements>(resultRequirements, true, 201,Constans.Get(ConstansType.Requirement));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Requirements>(new List<Requirements>(), false, 501, Constans.Error(ConstansType.Requirement), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Requirements>(new List<Requirements>(), false, 501, Constans.Error(ConstansType.Requirement), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("AddRequirement")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<FormatData<Requirements>> AddRequirement([FromBody] RequirementsFormat Requirements)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    _formatData = new FormatData<Requirements>(new List<Requirements>(), false, 501, Constans.ErrorFound(ConstansType.Requirement), "ModelState", locationError);
                }
                else if (Requirements.VacancyId == 0)
                {
                    _formatData = new FormatData<Requirements>(new List<Requirements>(), false, 501, Constans.CustomMessages(0));
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the Requirements
                    Requirements.DateCreated = DateTime.Now;
                    Requirements.DateModified = DateTime.Now;
                    Requirements RequirementsGood = new Requirements(Requirements);
                    locationError = "RepositoryCreate";
                    bool result = await _genericRepository.CreateAsync(RequirementsGood);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<Requirements> resultRequirements = new List<Requirements>();
                    resultRequirements.Add(RequirementsGood);
                    locationError = "FormatData";
                    _formatData = new FormatData<Requirements>(resultRequirements, result, 201,Constans.Add(ConstansType.Requirement));
                }                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Requirements>(new List<Requirements>(), false, 501, Constans.Error(ConstansType.Requirement), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Requirements>(new List<Requirements>(), false, 501, Constans.Error(ConstansType.Requirement), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("UpdateRequirement/{id}")]
        [Produces("application/json")]
        [HttpPut]
        public async Task<FormatData<Requirements>> UpdateRequirement(int id,[FromBody] RequirementsFormat Requirements)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "ModelState";
                //Requirements.SetId(id);
                if (!ModelState.IsValid)
                {
                    _formatData = new FormatData<Requirements>(new List<Requirements>(), false, 501, Constans.InvalidObject(ConstansType.Requirement), "ModelState", locationError);
                }
                else if (Requirements.VacancyId == 0)
                {
                    _formatData = new FormatData<Requirements>(new List<Requirements>(), false, 501, Constans.CustomMessages(0));
                }
                else
                {

                    Requirements requirements = await _genericRepository.GetById(id);
                    requirements.Name = Requirements.Name;
                    requirements.Description = Requirements.Description;
                    requirements.Required = Requirements.Required;
                    requirements.Active = Requirements.Active;
                    requirements.AgeExperience = Requirements.AgeExperience;
                    requirements.Benefits= Requirements.Benefits;
                    //requirements.Vacancy = await _unitOfWork.Context.Set<Vacancy>().FindAsync(Requirements.VacancyId);
                    requirements.DateModified = DateTime.Now;
                    requirements.NameModified = Requirements.NameModified;
                    requirements.VacancyId = Requirements.VacancyId;

                    locationError = "RepositoryUpdate";
                    bool result = _genericRepository.Update(requirements);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<Requirements> resultRequirements = new List<Requirements>();
                    resultRequirements.Add(requirements);
                    locationError = "FormatData";
                    _formatData = new FormatData<Requirements>(resultRequirements, result, 201,Constans.Update(ConstansType.Requirement,id));
                }
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Requirements>(new List<Requirements>(), false, 501, Constans.Error(ConstansType.Requirement), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Requirements>(new List<Requirements>(), false, 501, Constans.Error(ConstansType.Requirement), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("DeleteRequirement/{id}")]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<FormatData<Requirements>> DeleteRequirement(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetId";
                Requirements requestOriginal =  await _genericRepository.GetById(id);
                if(requestOriginal==null)
                    return new FormatData<Requirements>(new List<Requirements>(), false, 501,Constans.ErrorFound(ConstansType.Requirement), "Object not found", locationError);

                locationError = "UpdateRequirement";
                requestOriginal.Active = false;
                var result = _unitOfWork.Context.Set<Requirements>().Update(requestOriginal);

                locationError = "Commit";
                if (result!=null)
                    _unitOfWork.Commit();

                List<Requirements> resultRequirements = new List<Requirements>();
                resultRequirements.Add(requestOriginal);
                locationError = "FormatData";
                _formatData = new FormatData<Requirements>(resultRequirements, true, 201, Constans.Delete(ConstansType.Requirement,id));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Requirements>(new List<Requirements>(), false, 501, Constans.Error(ConstansType.Requirement), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Requirements>(new List<Requirements>(), false, 501, Constans.Error(ConstansType.Requirement), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }
    }
}