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
    public class LogController : ControllerBase
    {
        private readonly IGenericRepository<Log> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private FormatData<Log> _formatData;
        public LogController(IGenericRepository<Log> genericRepository, IUnitOfWork unitOfWork)
        {
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
            this._formatData= new FormatData<Log>();
        }

        [Route("GetAllLog")]
        [Produces("application/json")]
        [HttpGet ]
        public async Task<FormatData<Log>> GetAllLog()
        {
            string locationError = string.Empty;            
            try
            {
                locationError = "GetAllLog";
                IEnumerable<Log> result = await _genericRepository.GetAsync();
                _formatData = new FormatData<Log>(result, true, 201,Constans.GetAll(ConstansType.Log));                 
            }
            catch (Exception ex)
            {
                _formatData = new FormatData<Log>(new List<Log>(), false, 501, Constans.Error(ConstansType.Log), ex.Message, locationError, ex.StackTrace!=null ? ex.StackTrace : string.Empty);
            }            
            return _formatData;
        }

        [Route("GetById/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<Log>> GetById(int id)
        {
            string locationError = string.Empty;
            try
            {                                    
                locationError = "GetById";
                Log result = await _genericRepository.GetById(id);
                if(result==null)
                    return new FormatData<Log>(new List<Log>(), false, 201,Constans.ErrorFound(ConstansType.Log), "Object not found", locationError);

                List<Log> resultLog = new List<Log>();
                resultLog.Add(result);
                locationError = "FormatData";
                _formatData = new FormatData<Log>(resultLog, true, 201,Constans.Get(ConstansType.Log));                
            }
            catch (Exception ex)
            {
                _formatData = new FormatData<Log>(new List<Log>(), false, 501, Constans.Error(ConstansType.Log), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
            }
            return _formatData;
        }

        [Route("AddLog")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<FormatData<Log>> AddLog([FromBody] Log Log)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    _formatData = new FormatData<Log>(new List<Log>(), false, 501, Constans.ErrorFound(ConstansType.Log), "ModelState", locationError);                    
                }
                else
                {                                        
                    Log.ErrorDate= DateTime.Now;                    
                    locationError = "RepositoryCreate";
                    bool result = await _genericRepository.CreateAsync(Log);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<Log> resultLog = new List<Log>();
                    resultLog.Add(Log);
                    locationError = "FormatData";
                    _formatData = new FormatData<Log>(resultLog, result, 201,Constans.Add(ConstansType.Log));
                }                
            }
            catch (Exception ex)
            {
                _formatData = new FormatData<Log>(new List<Log>(), false, 501, Constans.Error(ConstansType.Log), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
            }
            return _formatData;
        }        

        [Route("DeleteLog/{id}")]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<FormatData<Log>> DeleteLog(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetId";
                Log requestOriginal =  await _genericRepository.GetById(id);
                if(requestOriginal==null)
                    return new FormatData<Log>(new List<Log>(), false, 501,Constans.ErrorFound(ConstansType.Log), "Log not found", locationError);

                locationError = "UpdateLog";
                requestOriginal.Active = false;
                var result = _unitOfWork.Context.Set<Log>().Update(requestOriginal);

                locationError = "Commit";
                if (result!=null)
                    _unitOfWork.Commit();

                List<Log> resultLog = new List<Log>();
                resultLog.Add(requestOriginal);
                locationError = "FormatData";
                _formatData = new FormatData<Log>(resultLog, true, 201, Constans.Delete(ConstansType.Log,id));                
            }
            catch (Exception ex)
            {
                _formatData = new FormatData<Log>(new List<Log>(), false, 501, Constans.Error(ConstansType.Log), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
            }
            return _formatData;
        }
    }
}