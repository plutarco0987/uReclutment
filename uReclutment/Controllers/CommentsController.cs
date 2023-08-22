using DataAccess.Generic;
using Entities;
using Entities.DataContext;
using Microsoft.AspNetCore.Mvc;
using Entities.Formats;
using Newtonsoft.Json;
using NuGet.Protocol;
using Newtonsoft.Json.Linq;

namespace uReclutment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly IGenericRepository<Comments> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private FormatData<Comments> _formatData;
        public CommentsController(IGenericRepository<Comments> genericRepository, IUnitOfWork unitOfWork)
        {
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
            this._formatData= new FormatData<Comments>();
        }


        [Route("GetAllCommentsFull")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<Comments>> GetAllCommentsFull()
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetAllEnumType";
                IEnumerable<Comments> result = await _genericRepository.GetAsync();
                _formatData = new FormatData<Comments>(result, true, 201, Constans.GetAll(ConstansType.EnumType));
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Comments>(new List<Comments>(), false, 501, Constans.Error(ConstansType.EnumType), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Comments>(new List<Comments>(), false, 501, Constans.Error(ConstansType.EnumType), "Error in Log", locationError, "Error");
            }

            return _formatData;
        }

        [Route("GetAllComments")]
        [Produces("application/json")]
        [HttpGet ]
        public async Task<FormatData<CommentsFormat>> GetAllComments()
        {
            string locationError = string.Empty;
            FormatData<CommentsFormat>  formatData = new FormatData<CommentsFormat>();
            try
            {
                locationError = "GetAllComments";
                IEnumerable<Comments> result = await _genericRepository.GetAsync();
                List<CommentsFormat> format = new List<CommentsFormat>();
                foreach (Comments item in result)
                {
                    format.Add(new CommentsFormat(item.CommentsId, item.QuestionDetailsId, item.Value, item.Active, item.NameCreated, item.DateCreated, item.NameCreated, item.DateModified));
                }

                formatData = new FormatData<CommentsFormat>(format, true, 201,Constans.GetAll(ConstansType.Comment));                 
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if(result)
                    formatData = new FormatData<CommentsFormat>(new List<CommentsFormat>(), false, 501, Constans.Error(ConstansType.Comment), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<CommentsFormat>(new List<CommentsFormat>(), false, 501, Constans.Error(ConstansType.Comment), "Error in Log", locationError, "Error");
            }

            return formatData;
        }

        [Route("GetById/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<Comments>> GetById(int id)
        {
            string locationError = string.Empty;
            try
            {                                    
                locationError = "GetById";
                Comments result = await _genericRepository.GetById(id);
                if(result==null)
                    return new FormatData<Comments>(new List<Comments>(), false, 201,Constans.ErrorFound(ConstansType.Comment), "Object not found", locationError);

                List<Comments> resultComments = new List<Comments>();
                resultComments.Add(result);
                locationError = "FormatData";
                _formatData = new FormatData<Comments>(resultComments, true, 201,Constans.Get(ConstansType.Comment));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Comments>(new List<Comments>(), false, 501, Constans.Error(ConstansType.Comment), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Comments>(new List<Comments>(), false, 501, Constans.Error(ConstansType.Comment), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("AddComments")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<FormatData<CommentsFormat>> AddComments([FromBody] CommentsFormat Comments)
        {
            string locationError = string.Empty;
            FormatData<CommentsFormat> formatData = new FormatData<CommentsFormat>();
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    formatData = new FormatData<CommentsFormat>(new List<CommentsFormat>(), false, 501, Constans.ErrorFound(ConstansType.Comment), "ModelState", locationError);                    
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the Comments
                    DateTime now = DateTime.Now;
                    Comments.DateCreated = now;
                    Comments.DateModified = now;

                    Comments comments = new Comments(Comments);

                    



                    locationError = "RepositoryCreate";
                    bool result = await _genericRepository.CreateAsync(comments);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<CommentsFormat> resultComments = new List<CommentsFormat>();
                    resultComments.Add(Comments);
                    locationError = "FormatData";
                    formatData = new FormatData<CommentsFormat>(resultComments, result, 201,Constans.Add(ConstansType.Comment));
                }                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<CommentsFormat>(new List<CommentsFormat>(), false, 501, Constans.Error(ConstansType.Comment), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<CommentsFormat>(new List<CommentsFormat>(), false, 501, Constans.Error(ConstansType.Comment), "Error in Log", locationError, "Error");
            }
            return formatData;
        }

        [Route("UpdateComments/{id}")]
        [Produces("application/json")]
        [HttpPut]
        public async Task<FormatData<CommentsFormat>> UpdateComments(int id,[FromBody] CommentsFormat Comments)
        {
            string locationError = string.Empty;
            FormatData<CommentsFormat> formatData = new FormatData<CommentsFormat>();
            try
            {
                locationError = "ModelState";                
                if (!ModelState.IsValid)
                {
                    formatData = new FormatData<CommentsFormat>(new List<CommentsFormat>(), false, 501, Constans.InvalidObject(ConstansType.Comment), "ModelState",locationError);
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the Comments
                    Comments comments = await _genericRepository.GetById(id);
                    Comments.DateModified = DateTime.Now;
                    comments.CommentsFormat(Comments);

                    locationError = "RepositoryUpdate";
                    bool result = _genericRepository.Update(comments);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<CommentsFormat> resultComments = new List<CommentsFormat>();
                    resultComments.Add(Comments);
                    locationError = "FormatData";
                    formatData = new FormatData<CommentsFormat>(resultComments, result, 201,Constans.Update(ConstansType.Comment,id));
                }
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<CommentsFormat>(new List<CommentsFormat>(), false, 501, Constans.Error(ConstansType.Comment), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<CommentsFormat>(new List<CommentsFormat>(), false, 501, Constans.Error(ConstansType.Comment), "Error in Log", locationError, "Error");
            }
            return formatData;
        }


        [Route("UpdateCommentsExecute/{id}")]
        [Produces("application/json")]
        [HttpPut]
        public async Task<FormatData<CommentsFormat>> UpdateCommentsExecute(int id, [FromBody] CommentsFormat Comments)
        {
            string locationError = string.Empty;
            FormatData<CommentsFormat> formatData = new FormatData<CommentsFormat>();
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    formatData = new FormatData<CommentsFormat>(new List<CommentsFormat>(), false, 501, Constans.InvalidObject(ConstansType.Comment), "ModelState", locationError);
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the Comments
                    Comments comments = await _genericRepository.GetById(id);
                    Comments.DateModified = DateTime.Now;
                    if (Comments.Active)
                    {
                        comments.Value = Comments.Value;
                        comments.NameModified = Comments.NameModified;
                    }
                    else
                    {
                        //will be "deleted"
                        comments.Active = false;
                    }
                    
                    
                   

                    locationError = "RepositoryUpdate";
                    bool result = _genericRepository.Update(comments);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<CommentsFormat> resultComments = new List<CommentsFormat>();
                    resultComments.Add(Comments);
                    locationError = "FormatData";
                    formatData = new FormatData<CommentsFormat>(resultComments, result, 201, Constans.Update(ConstansType.Comment, id));
                }
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<CommentsFormat>(new List<CommentsFormat>(), false, 501, Constans.Error(ConstansType.Comment), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<CommentsFormat>(new List<CommentsFormat>(), false, 501, Constans.Error(ConstansType.Comment), "Error in Log", locationError, "Error");
            }
            return formatData;
        }


        [Route("DeleteComments/{id}")]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<FormatData<Comments>> DeleteComments(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetId";
                Comments requestOriginal =  await _genericRepository.GetById(id);
                if(requestOriginal==null)
                    return new FormatData<Comments>(new List<Comments>(), false, 501,Constans.ErrorFound(ConstansType.Comment), "Object not found", locationError);

                locationError = "UpdateComments";
                requestOriginal.Active = false;
                var result = _unitOfWork.Context.Set<Comments>().Update(requestOriginal);

                locationError = "Commit";
                if (result!=null)
                    _unitOfWork.Commit();

                List<Comments> resultComments = new List<Comments>();
                resultComments.Add(requestOriginal);
                locationError = "FormatData";
                _formatData = new FormatData<Comments>(resultComments, true, 201, Constans.Delete(ConstansType.Comment,id));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Comments>(new List<Comments>(), false, 501, Constans.Error(ConstansType.Comment), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Comments>(new List<Comments>(), false, 501, Constans.Error(ConstansType.Comment), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }
    }
}