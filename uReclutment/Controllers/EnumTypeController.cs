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
    public class EnumTypeController : ControllerBase
    {
        private readonly IGenericRepository<EnumType> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private FormatData<EnumType> _formatData;
        public EnumTypeController(IGenericRepository<EnumType> genericRepository, IUnitOfWork unitOfWork)
        {
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
            this._formatData= new FormatData<EnumType>();
        }

        [Route("GetAllEnumType")]
        [Produces("application/json")]
        [HttpGet ]
        public async Task<FormatData<EnumType>> GetAllEnumType()
        {
            string locationError = string.Empty;            
            try
            {
                locationError = "GetAllEnumType";
                IEnumerable<EnumType> result = await _genericRepository.GetAsync();
                _formatData = new FormatData<EnumType>(result, true, 201,Constans.GetAll(ConstansType.EnumType));                 
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if(result)
                    _formatData = new FormatData<EnumType>(new List<EnumType>(), false, 501, Constans.Error(ConstansType.EnumType), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<EnumType>(new List<EnumType>(), false, 501, Constans.Error(ConstansType.EnumType), "Error in Log", locationError, "Error");
            }

            return _formatData;
        }

        [Route("GetById/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<EnumType>> GetById(int id)
        {
            string locationError = string.Empty;
            try
            {                                    
                locationError = "GetById";
                EnumType result = await _genericRepository.GetById(id);
                if(result==null)
                    return new FormatData<EnumType>(new List<EnumType>(), false, 201,Constans.ErrorFound(ConstansType.EnumType), "Object not found", locationError);

                List<EnumType> resultEnumType = new List<EnumType>();
                resultEnumType.Add(result);
                locationError = "FormatData";
                _formatData = new FormatData<EnumType>(resultEnumType, true, 201,Constans.Get(ConstansType.EnumType));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<EnumType>(new List<EnumType>(), false, 501, Constans.Error(ConstansType.EnumType), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<EnumType>(new List<EnumType>(), false, 501, Constans.Error(ConstansType.EnumType), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("AddEnumType")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<FormatData<EnumType>> AddEnumType([FromBody] EnumType EnumType)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    _formatData = new FormatData<EnumType>(new List<EnumType>(), false, 501, Constans.ErrorFound(ConstansType.EnumType), "ModelState", locationError);                    
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the EnumType
                    EnumType.DateCreated=DateTime.Now;
                    EnumType.DateModified = DateTime.Now;
                    locationError = "RepositoryCreate";
                    bool result = await _genericRepository.CreateAsync(EnumType);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<EnumType> resultEnumType = new List<EnumType>();
                    resultEnumType.Add(EnumType);
                    locationError = "FormatData";
                    _formatData = new FormatData<EnumType>(resultEnumType, result, 201,Constans.Add(ConstansType.EnumType));
                }                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<EnumType>(new List<EnumType>(), false, 501, Constans.Error(ConstansType.EnumType), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<EnumType>(new List<EnumType>(), false, 501, Constans.Error(ConstansType.EnumType), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("UpdateEnumType/{id}")]
        [Produces("application/json")]
        [HttpPut]
        public async Task<FormatData<EnumType>> UpdateEnumType(int id,[FromBody] EnumType EnumType)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "ModelState";
                EnumType.SetId(id);
                if (!ModelState.IsValid)
                {
                    _formatData = new FormatData<EnumType>(new List<EnumType>(), false, 501, Constans.InvalidObject(ConstansType.EnumType), "ModelState",locationError);
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the EnumType                    
                    EnumType.DateModified = DateTime.Now;
                    locationError = "RepositoryUpdate";
                    bool result = _genericRepository.Update(EnumType);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<EnumType> resultEnumType = new List<EnumType>();
                    resultEnumType.Add(EnumType);
                    locationError = "FormatData";
                    _formatData = new FormatData<EnumType>(resultEnumType, result, 201,Constans.Update(ConstansType.EnumType,id));
                }
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<EnumType>(new List<EnumType>(), false, 501, Constans.Error(ConstansType.EnumType), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<EnumType>(new List<EnumType>(), false, 501, Constans.Error(ConstansType.EnumType), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("DeleteEnumType/{id}")]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<FormatData<EnumType>> DeleteEnumType(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetId";
                EnumType requestOriginal =  await _genericRepository.GetById(id);
                if(requestOriginal==null)
                    return new FormatData<EnumType>(new List<EnumType>(), false, 501,Constans.ErrorFound(ConstansType.EnumType), "Object not found", locationError);

                locationError = "UpdateEnumType";
                requestOriginal.Active = false;
                var result = _unitOfWork.Context.Set<EnumType>().Update(requestOriginal);

                locationError = "Commit";
                if (result!=null)
                    _unitOfWork.Commit();

                List<EnumType> resultEnumType = new List<EnumType>();
                resultEnumType.Add(requestOriginal);
                locationError = "FormatData";
                _formatData = new FormatData<EnumType>(resultEnumType, true, 201, Constans.Delete(ConstansType.EnumType,id));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<EnumType>(new List<EnumType>(), false, 501, Constans.Error(ConstansType.EnumType), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<EnumType>(new List<EnumType>(), false, 501, Constans.Error(ConstansType.EnumType), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }
    }
}