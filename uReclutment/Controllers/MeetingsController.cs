using DataAccess.Generic;
using Entities;
using Entities.DataContext;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;

using Entities.Formats;

namespace uReclutment.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingsController : ControllerBase
    {
        private readonly IGenericRepository<Meetings> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private FormatData<Meetings> _formatData;
        public MeetingsController(IGenericRepository<Meetings> genericRepository, IUnitOfWork unitOfWork)
        {
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
            this._formatData= new FormatData<Meetings>();
        }

        [Route("GetAllMeetings")]
        [Produces("application/json")]
        [HttpGet ]
        public async Task<FormatData<MeetingsFormat>> GetAllMeetings()
        {
            string locationError = string.Empty;
            FormatData<MeetingsFormat> formatData = new FormatData<MeetingsFormat>();
            try
            {
                locationError = "GetAllMeetings";
                List<MeetingsFormat> formats = new List<MeetingsFormat>();
                IEnumerable<Meetings> result = await _genericRepository.GetAsync();
                foreach (var item in result)
                {
                    formats.Add(new MeetingsFormat(item.MeetingsId, item.CandidatesId, item.NumberMeeting, item.Time, item.Active, item.NameCreated, item.DateCreated, item.NameModified, item.DateModified));
                }
               

               formatData = new FormatData<MeetingsFormat>(formats, true, 201,Constans.GetAll(ConstansType.Meeting));                 
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if(result)
                    formatData = new FormatData<MeetingsFormat>(new List<MeetingsFormat>(), false, 501, Constans.Error(ConstansType.Meeting), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<MeetingsFormat>(new List<MeetingsFormat>(), false, 501, Constans.Error(ConstansType.Meeting), "Error in Log", locationError, "Error");
            }

            return formatData;
        }

        [Route("GetById/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<Meetings>> GetById(int id)
        {
            string locationError = string.Empty;
            try
            {                                    
                locationError = "GetById";
                Meetings result = await _genericRepository.GetById(id);
                if(result==null)
                    return new FormatData<Meetings>(new List<Meetings>(), false, 201,Constans.ErrorFound(ConstansType.Meeting), "Object not found", locationError);

                List<Meetings> resultMeetings = new List<Meetings>();
                resultMeetings.Add(result);
                locationError = "FormatData";
                _formatData = new FormatData<Meetings>(resultMeetings, true, 201,Constans.Get(ConstansType.Meeting));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Meetings>(new List<Meetings>(), false, 501, Constans.Error(ConstansType.Meeting), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Meetings>(new List<Meetings>(), false, 501, Constans.Error(ConstansType.Meeting), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("AddMeetings")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<FormatData<MeetingsFormat>> AddMeetings([FromBody] MeetingsFormat Meetings)
        {
            string locationError = string.Empty;
            FormatData<MeetingsFormat> formatData = new FormatData<MeetingsFormat>();
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    formatData = new FormatData<MeetingsFormat>(new List<MeetingsFormat>(), false, 501, Constans.ErrorFound(ConstansType.Meeting), "ModelState", locationError);                    
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the Meetings
                    

                    DateTime now=DateTime.Now;
                    Meetings.DateCreated=now;
                    Meetings.DateModified = now;
                    Meetings meetings = new Meetings(Meetings);
                    locationError = "RepositoryCreate";
                    bool result = await _genericRepository.CreateAsync(meetings);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<MeetingsFormat> resultMeetings = new List<MeetingsFormat>();
                    resultMeetings.Add(Meetings);
                    locationError = "FormatData";
                    formatData = new FormatData<MeetingsFormat>(resultMeetings, result, 201,Constans.Add(ConstansType.Meeting));
                }                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<MeetingsFormat>(new List<MeetingsFormat>(), false, 501, Constans.Error(ConstansType.Meeting), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<MeetingsFormat>(new List<MeetingsFormat>(), false, 501, Constans.Error(ConstansType.Meeting), "Error in Log", locationError, "Error");
            }
            return formatData;
        }

        [Route("AddMeetingsInAMetting")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<FormatData<MeetingsFormat>> AddMeetingsInAMetting([FromBody] MeetingsFormat Meetings)
        {
            string locationError = string.Empty;
            FormatData<MeetingsFormat> formatData = new FormatData<MeetingsFormat>();
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    formatData = new FormatData<MeetingsFormat>(new List<MeetingsFormat>(), false, 501, Constans.ErrorFound(ConstansType.Meeting), "ModelState", locationError);
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the Meetings

                    //we require check the meetings exist 
                    IEnumerable<Meetings> results = await _genericRepository.GetAsync();
                    int i = 0;
                    foreach (var item in results)
                    {
                        if(item.CandidatesId== Meetings.CandidatesId)
                        {
                            i++;
                        }
                    }
                    Meetings.NumberMeeting = i;
                    DateTime now = DateTime.Now;
                    Meetings.DateCreated = now;
                    Meetings.DateModified = now;
                    Meetings meetings = new Meetings(Meetings);
                    locationError = "RepositoryCreate";
                    bool result = await _genericRepository.CreateAsync(meetings);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<MeetingsFormat> resultMeetings = new List<MeetingsFormat>();
                    resultMeetings.Add(Meetings);
                    locationError = "FormatData";
                    formatData = new FormatData<MeetingsFormat>(resultMeetings, result, 201, Constans.Add(ConstansType.Meeting));
                }
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<MeetingsFormat>(new List<MeetingsFormat>(), false, 501, Constans.Error(ConstansType.Meeting), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<MeetingsFormat>(new List<MeetingsFormat>(), false, 501, Constans.Error(ConstansType.Meeting), "Error in Log", locationError, "Error");
            }
            return formatData;
        }


        [Route("UpdateMeetings/{id}")]
        [Produces("application/json")]
        [HttpPut]
        public async Task<FormatData<MeetingsFormat>> UpdateMeetings(int id,[FromBody] MeetingsFormat Meetings)
        {
            string locationError = string.Empty;
            FormatData<MeetingsFormat> formatData = new FormatData<MeetingsFormat>();
            try
            {
                locationError = "ModelState";                
                if (!ModelState.IsValid)
                {
                    formatData = new FormatData<MeetingsFormat>(new List<MeetingsFormat>(), false, 501, Constans.InvalidObject(ConstansType.Meeting), "ModelState",locationError);
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the Meetings
                    //
                    Meetings.DateModified = DateTime.Now;
                    Meetings meetings = await _genericRepository.GetById(id);
                    meetings.MeetingsFormat(Meetings);

                    
                    locationError = "RepositoryUpdate";
                    bool result = _genericRepository.Update(meetings);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<MeetingsFormat> resultMeetings = new List<MeetingsFormat>();
                    resultMeetings.Add(Meetings);
                    locationError = "FormatData";
                    formatData = new FormatData<MeetingsFormat>(resultMeetings, result, 201,Constans.Update(ConstansType.Meeting,id));
                }
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<MeetingsFormat>(new List<MeetingsFormat>(), false, 501, Constans.Error(ConstansType.Meeting), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<MeetingsFormat>(new List<MeetingsFormat>(), false, 501, Constans.Error(ConstansType.Meeting), "Error in Log", locationError, "Error");
            }
            return formatData;
        }

        [Route("DeleteMeetings/{id}")]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<FormatData<Meetings>> DeleteMeetings(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetId";
                Meetings requestOriginal =  await _genericRepository.GetById(id);
                if(requestOriginal==null)
                    return new FormatData<Meetings>(new List<Meetings>(), false, 501,Constans.ErrorFound(ConstansType.Meeting), "Object not found", locationError);

                locationError = "UpdateMeetings";
                requestOriginal.Active = false;
                var result = _unitOfWork.Context.Set<Meetings>().Update(requestOriginal);

                locationError = "Commit";
                if (result!=null)
                    _unitOfWork.Commit();

                List<Meetings> resultMeetings = new List<Meetings>();
                resultMeetings.Add(requestOriginal);
                locationError = "FormatData";
                _formatData = new FormatData<Meetings>(resultMeetings, true, 201, Constans.Delete(ConstansType.Meeting,id));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Meetings>(new List<Meetings>(), false, 501, Constans.Error(ConstansType.Meeting), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Meetings>(new List<Meetings>(), false, 501, Constans.Error(ConstansType.Meeting), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }


    }
}