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
    public class SettingsController : ControllerBase
    {
        private readonly IGenericRepository<Settings> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private FormatData<Settings> _formatData;
        public SettingsController(IGenericRepository<Settings> genericRepository, IUnitOfWork unitOfWork)
        {
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
            this._formatData= new FormatData<Settings>();
        }

        [Route("GetAllSettings")]
        [Produces("application/json")]
        [HttpGet ]
        public async Task<FormatData<Settings>> GetAllSettings()
        {
            string locationError = string.Empty;            
            try
            {
                locationError = "GetAllSettings";
                IEnumerable<Settings> result = await _genericRepository.GetAsync();
                _formatData = new FormatData<Settings>(result, true, 201,Constans.GetAll(ConstansType.Setting));                 
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if(result)
                    _formatData = new FormatData<Settings>(new List<Settings>(), false, 501, Constans.Error(ConstansType.Setting), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Settings>(new List<Settings>(), false, 501, Constans.Error(ConstansType.Setting), "Error in Log", locationError, "Error");
            }

            return _formatData;
        }

        [Route("GetById/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<Settings>> GetById(int id)
        {
            string locationError = string.Empty;
            try
            {                                    
                locationError = "GetById";
                Settings result = await _genericRepository.GetById(id);
                if(result==null)
                    return new FormatData<Settings>(new List<Settings>(), false, 201,Constans.ErrorFound(ConstansType.Setting), "Object not found", locationError);

                List<Settings> resultSettings = new List<Settings>();
                resultSettings.Add(result);
                locationError = "FormatData";
                _formatData = new FormatData<Settings>(resultSettings, true, 201,Constans.Get(ConstansType.Setting));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Settings>(new List<Settings>(), false, 501, Constans.Error(ConstansType.Setting), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Settings>(new List<Settings>(), false, 501, Constans.Error(ConstansType.Setting), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("AddSetting")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<FormatData<Settings>> AddSetting([FromBody] Settings Settings)
        {
            string locationError = string.Empty;
            Settings.SettingsId = 0;
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    _formatData = new FormatData<Settings>(new List<Settings>(), false, 501, Constans.ErrorFound(ConstansType.Setting), "ModelState", locationError);                    
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the Settings
                    Settings.DateCreated=DateTime.Now;
                    Settings.DateModified = Settings.DateCreated;
                    locationError = "RepositoryCreate";
                    bool result = await _genericRepository.CreateAsync(Settings);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<Settings> resultSettings = new List<Settings>();
                    resultSettings.Add(Settings);
                    locationError = "FormatData";
                    _formatData = new FormatData<Settings>(resultSettings, result, 201,Constans.Add(ConstansType.Setting));
                }                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Settings>(new List<Settings>(), false, 501, Constans.Error(ConstansType.Setting), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Settings>(new List<Settings>(), false, 501, Constans.Error(ConstansType.Setting), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("UpdateSetting/{id}")]
        [Produces("application/json")]
        [HttpPut]
        public async Task<FormatData<Settings>> UpdateSetting(int id,[FromBody] Settings Settings)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "ModelState";
                Settings.SetId(id);
                if (!ModelState.IsValid)
                {
                    _formatData = new FormatData<Settings>(new List<Settings>(), false, 501, Constans.InvalidObject(ConstansType.Setting), "ModelState",locationError);
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the Settings                    
                    Settings.DateModified = DateTime.Now;
                    locationError = "RepositoryUpdate";
                    bool result = _genericRepository.Update(Settings);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<Settings> resultSettings = new List<Settings>();
                    resultSettings.Add(Settings);
                    locationError = "FormatData";
                    _formatData = new FormatData<Settings>(resultSettings, result, 201,Constans.Update(ConstansType.Setting,id));
                }
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Settings>(new List<Settings>(), false, 501, Constans.Error(ConstansType.Setting), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Settings>(new List<Settings>(), false, 501, Constans.Error(ConstansType.Setting), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("DeleteSetting/{id}")]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<FormatData<Settings>> DeleteSetting(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetId";
                Settings requestOriginal =  await _genericRepository.GetById(id);
                if(requestOriginal==null)
                    return new FormatData<Settings>(new List<Settings>(), false, 501,Constans.ErrorFound(ConstansType.Setting), "Object not found", locationError);

                locationError = "UpdateSetting";
                requestOriginal.Active = false;
                var result = _unitOfWork.Context.Set<Settings>().Update(requestOriginal);

                locationError = "Commit";
                if (result!=null)
                    _unitOfWork.Commit();

                List<Settings> resultSettings = new List<Settings>();
                resultSettings.Add(requestOriginal);
                locationError = "FormatData";
                _formatData = new FormatData<Settings>(resultSettings, true, 201, Constans.Delete(ConstansType.Setting,id));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Settings>(new List<Settings>(), false, 501, Constans.Error(ConstansType.Setting), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Settings>(new List<Settings>(), false, 501, Constans.Error(ConstansType.Setting), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }
    }
}