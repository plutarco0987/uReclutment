using DataAccess.Generic;
using Entities;
using Entities.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.IO;

namespace uReclutment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IGenericRepository<Files> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private FormatData<Files> _formatData;
        public FilesController(IGenericRepository<Files> genericRepository, IUnitOfWork unitOfWork)
        {
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
            this._formatData = new FormatData<Files>();
        }

        [Route("GetAllFiles")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<Files>> GetAllFiles()
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetAllFiles";
                IEnumerable<Files> result = await _genericRepository.GetAsync();
                _formatData = new FormatData<Files>(result, true, 201, Constans.GetAll(ConstansType.EnumType));
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Files>(new List<Files>(), false, 501, "Error in GetAllFiles", ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Files>(new List<Files>(), false, 501, "Error in GetAllFiles", "Error in Log", locationError, "Error");
            }

            return _formatData;
        }


        [Route("GetById/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<Files>> GetById(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetById";
                List<Files> result = await _unitOfWork.Context.Set<Files>().Where(x => x.CandidatesId == id).ToListAsync();

                if (result == null)
                    return new FormatData<Files>(new List<Files>(), false, 201, "Good", "Object not found", locationError);

                locationError = "FormatData";
                _formatData = new FormatData<Files>(result, true, 201, "Good");
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Files>(new List<Files>(), false, 501, "Bad", ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Files>(new List<Files>(), false, 501, "Bad", "Error in Log", locationError, "Error");
            }
            return _formatData;
        }


        [Route("GetFile/{path}-{key}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<byte[]> GetFile (string path,string key)
        {
            byte[] returnValue= new byte[0];
            string locationError = string.Empty;
            try
            {
                if (key == "ureclutmentKey1")
                {
                    locationError = "GetById";
                    path = path.Replace('µ', '\\');
                    returnValue = System.IO.File.ReadAllBytes(path);
                }
                else
                {
                    returnValue=new byte[0];
                }                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                returnValue= new byte[0];
            }
            return returnValue;
        }


    }
}