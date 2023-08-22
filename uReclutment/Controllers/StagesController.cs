using DataAccess.Generic;
using Entities;
using Entities.DataContext;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace uReclutment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StagesController : ControllerBase
    {
        private readonly IGenericRepository<Stages> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private FormatData<Stages> _formatData;
        public StagesController(IGenericRepository<Stages> genericRepository, IUnitOfWork unitOfWork)
        {
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
            this._formatData= new FormatData<Stages>();
        }

        [Route("GetAllStages")]
        [Produces("application/json")]
        [HttpGet ]
        public async Task<FormatData<Stages>> GetAllStages()
        {
            string locationError = string.Empty;            
            try
            {
                locationError = "GetAllStages";
                IEnumerable<Stages> result = await _genericRepository.GetAsync();
                _formatData = new FormatData<Stages>(result, true, 201,Constans.GetAll(ConstansType.Stage));                 
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if(result)
                    _formatData = new FormatData<Stages>(new List<Stages>(), false, 501, Constans.Error(ConstansType.Stage), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Stages>(new List<Stages>(), false, 501, Constans.Error(ConstansType.Stage), "Error in Log", locationError, "Error");
            }

            return _formatData;
        }

        [Route("GetById/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<Stages>> GetById(int id)
        {
            string locationError = string.Empty;
            try
            {                                    
                locationError = "GetById";
                Stages result = await _genericRepository.GetById(id);
                if(result==null)
                    return new FormatData<Stages>(new List<Stages>(), false, 201,Constans.ErrorFound(ConstansType.Stage), "Object not found", locationError);

                List<Stages> resultStages = new List<Stages>();
                resultStages.Add(result);
                locationError = "FormatData";
                _formatData = new FormatData<Stages>(resultStages, true, 201,Constans.Get(ConstansType.Stage));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Stages>(new List<Stages>(), false, 501, Constans.Error(ConstansType.Stage), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Stages>(new List<Stages>(), false, 501, Constans.Error(ConstansType.Stage), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("AddStage")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<FormatData<Stages>> AddStage([FromBody] Stages stages)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    _formatData = new FormatData<Stages>(new List<Stages>(), false, 501, Constans.ErrorFound(ConstansType.Stage), "ModelState", locationError);                    
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the stages
                    stages.DateCreated=DateTime.Now;
                    stages.DateModified = DateTime.Now;
                    locationError = "RepositoryCreate";
                    bool result = await _genericRepository.CreateAsync(stages);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<Stages> resultStages = new List<Stages>();
                    resultStages.Add(stages);
                    locationError = "FormatData";
                    _formatData = new FormatData<Stages>(resultStages, result, 201,Constans.Add(ConstansType.Stage));
                }                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Stages>(new List<Stages>(), false, 501, Constans.Error(ConstansType.Stage), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Stages>(new List<Stages>(), false, 501, Constans.Error(ConstansType.Stage), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("UpdateStage/{id}")]
        [Produces("application/json")]
        [HttpPut]
        public async Task<FormatData<Stages>> UpdateStage(int id,[FromBody] Stages stages)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "ModelState";
                stages.SetId(id);
                if (!ModelState.IsValid)
                {
                    _formatData = new FormatData<Stages>(new List<Stages>(), false, 501, Constans.InvalidObject(ConstansType.Stage), "ModelState",locationError);
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the stages                    
                    stages.DateModified = DateTime.Now;
                    locationError = "RepositoryUpdate";
                    bool result = _genericRepository.Update(stages);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<Stages> resultStages = new List<Stages>();
                    resultStages.Add(stages);
                    locationError = "FormatData";
                    _formatData = new FormatData<Stages>(resultStages, result, 201,Constans.Update(ConstansType.Stage,id));
                }
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Stages>(new List<Stages>(), false, 501, Constans.Error(ConstansType.Stage), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Stages>(new List<Stages>(), false, 501, Constans.Error(ConstansType.Stage), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("DeleteStage/{id}")]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<FormatData<Stages>> DeleteStage(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetId";
                Stages requestOriginal =  await _genericRepository.GetById(id);
                if(requestOriginal==null)
                    return new FormatData<Stages>(new List<Stages>(), false, 501,Constans.ErrorFound(ConstansType.Stage), "Object not found", locationError);

                locationError = "UpdateStage";
                requestOriginal.Active = false;
                var result = _unitOfWork.Context.Set<Stages>().Update(requestOriginal);

                locationError = "Commit";
                if (result!=null)
                    _unitOfWork.Commit();

                List<Stages> resultStages = new List<Stages>();
                resultStages.Add(requestOriginal);
                locationError = "FormatData";
                _formatData = new FormatData<Stages>(resultStages, true, 201, Constans.Delete(ConstansType.Stage,id));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Stages>(new List<Stages>(), false, 501, Constans.Error(ConstansType.Stage), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Stages>(new List<Stages>(), false, 501, Constans.Error(ConstansType.Stage), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }
    }
}