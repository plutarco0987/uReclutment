using DataAccess.Generic;
using Entities;
using Entities.DataContext;
using Microsoft.AspNetCore.Mvc;

namespace uReclutment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IGenericRepository<User> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private FormatData<User> _formatData;
        public UserController(IGenericRepository<User> genericRepository, IUnitOfWork unitOfWork)
        {
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
            this._formatData= new FormatData<User>();
        }

        [Route("GetAllUser")]
        [Produces("application/json")]
        [HttpGet ]
        public async Task<FormatData<User>> GetAllUser()
        {
            string locationError = string.Empty;            
            try
            {
                locationError = "GetAllUser";
                IEnumerable<User> result = await _genericRepository.GetAsync();
                _formatData = new FormatData<User>(result, true, 201,Constans.GetAll(ConstansType.User));                 
            }
            catch (Exception ex)
            {
                _formatData = new FormatData<User>(new List<User>(), false, 501, Constans.Error(ConstansType.User), ex.Message, locationError, ex.StackTrace!=null ? ex.StackTrace : string.Empty);
            }            
            return _formatData;
        }

        [Route("GetById/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<User>> GetById(int id)
        {
            string locationError = string.Empty;
            try
            {                                    
                locationError = "GetById";
                User result = await _genericRepository.GetById(id);
                if(result==null)
                    return new FormatData<User>(new List<User>(), false, 201,Constans.ErrorFound(ConstansType.User), "Object not found", locationError);

                List<User> resultUser = new List<User>();
                resultUser.Add(result);
                locationError = "FormatData";
                _formatData = new FormatData<User>(resultUser, true, 201,Constans.Get(ConstansType.User));                
            }
            catch (Exception ex)
            {
                _formatData = new FormatData<User>(new List<User>(), false, 501, Constans.Error(ConstansType.User), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
            }
            return _formatData;
        }

        [Route("AddUser")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<FormatData<User>> AddUser([FromBody] User User)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    _formatData = new FormatData<User>(new List<User>(), false, 501, Constans.ErrorFound(ConstansType.User), "ModelState", locationError);                    
                }
                else
                {                                                            
                    locationError = "RepositoryCreate";
                    //check if the user exist
                    User userResult = _unitOfWork.Context.Set<User>().FirstOrDefault(x => x.UserName == User.UserName);
                    if (userResult!= null)
                    {
                        _formatData = new FormatData<User>(new List<User>(), false, 301, "Please select another user name", "ModelState", locationError);
                    }
                    else
                    {
                        bool result = await _genericRepository.CreateAsync(User);
                        locationError = "Commit";
                        if (result)
                            _unitOfWork.Commit();

                        List<User> resultUser = new List<User>();
                        resultUser.Add(User);
                        locationError = "FormatData";
                        _formatData = new FormatData<User>(resultUser, result, 201, Constans.Add(ConstansType.User));
                    }                    
                }                
            }
            catch (Exception ex)
            {
                _formatData = new FormatData<User>(new List<User>(), false, 501, Constans.Error(ConstansType.User), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
            }
            return _formatData;
        }

        [Route("Reset")]
        [Produces("application/json")]        
        [HttpPost]
        public async Task<FormatData<User>> Reset([FromBody]User User)
        {
            string locationError = string.Empty;            
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    _formatData = new FormatData<User>(new List<User>(), false, 501, Constans.ErrorFound(ConstansType.User), "ModelState", locationError);
                }
                else 
                {
                    locationError = "RepositoryCreate";
                    //check if the user exist
                    User userResult =  _unitOfWork.Context.Set<User>().FirstOrDefault(x => x.UserName == User.UserName);
                    if (userResult == null)
                    {
                        _formatData = new FormatData<User>(new List<User>(), false, 301, "Please select another user name", "ModelState", locationError);
                    }
                    else
                    {                                          
                        userResult.Password = User.Password; 
                        
                        bool result =  _genericRepository.Update(userResult);
                        locationError = "Commit";
                        if (result)
                            _unitOfWork.Commit();

                        List<User> resultUser = new List<User>();
                        resultUser.Add(userResult);
                        locationError = "FormatData";
                        _formatData = new FormatData<User>(resultUser, result, 201, Constans.Add(ConstansType.User));                      
                    }
                }
            }
            catch (Exception ex)
            {
                _formatData = new FormatData<User>(new List<User>(), false, 501, Constans.Error(ConstansType.User), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
            }
            return _formatData;
        }


        [Route("EditUser/{password}")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<FormatData<User>> EditUser(string password,[FromBody] User User)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    _formatData = new FormatData<User>(new List<User>(), false, 501, Constans.ErrorFound(ConstansType.User), "ModelState", locationError);
                }
                else
                {
                    locationError = "RepositoryCreate";
                    //check if the user exist
                    User userResult =  _unitOfWork.Context.Set<User>().FirstOrDefault(x => x.UserName == User.UserName);
                    if (userResult == null)
                    {
                        _formatData = new FormatData<User>(new List<User>(), false, 301, "Please select another user name", "ModelState", locationError);
                    }
                    else
                    {
                        //password change
                        //User.SetId(userResult.UserId);
                        if(userResult.Password==password)
                        {
                            userResult.Password = User.Password;
                            bool result = _genericRepository.Update(userResult);
                            locationError = "Commit";
                            if (result)
                                _unitOfWork.Commit();

                            List<User> resultUser = new List<User>();
                            resultUser.Add(User);
                            locationError = "FormatData";
                            _formatData = new FormatData<User>(resultUser, result, 201, Constans.Add(ConstansType.User));
                        }
                        else
                        {
                            _formatData = new FormatData<User>(new List<User>(), false, 201, "The password is grong");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _formatData = new FormatData<User>(new List<User>(), false, 501, Constans.Error(ConstansType.User), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
            }
            return _formatData;
        }

        [Route("DeleteUser/{id}")]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<FormatData<User>> DeleteUser(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetId";
                User requestOriginal =  await _genericRepository.GetById(id);
                if(requestOriginal==null)
                    return new FormatData<User>(new List<User>(), false, 501,Constans.ErrorFound(ConstansType.User), "User not found", locationError);

                locationError = "UpdateUser";                
                var result = _unitOfWork.Context.Set<User>().Update(requestOriginal);

                locationError = "Commit";
                if (result!=null)
                    _unitOfWork.Commit();

                List<User> resultUser = new List<User>();
                resultUser.Add(requestOriginal);
                locationError = "FormatData";
                _formatData = new FormatData<User>(resultUser, true, 201, Constans.Delete(ConstansType.User,id));                
            }
            catch (Exception ex)
            {
                _formatData = new FormatData<User>(new List<User>(), false, 501, Constans.Error(ConstansType.User), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
            }
            return _formatData;
        }


        [Route("Validation")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<FormatData<bool>> Validation([FromBody] User user)
        {
            FormatData<bool> formatData = new FormatData<bool>();
            List<bool> bools=new List<bool>();
            bools.Add(false);
            formatData.Data = bools;
            string errorLocation = string.Empty;
            try
            {
                errorLocation = "Call";
                User result =  _unitOfWork.Context.Set<User>().FirstOrDefault(x => x.UserName == user.UserName);
                if (result == null)
                    return formatData;
                else
                {
                    errorLocation = "ifCall";
                    if (user.Password == result.Password && user.UserName == result.UserName)
                    {
                        bools = new List<bool>();
                        bools.Add(true);
                        formatData.Data = bools;                        
                    }                                                                   
                }
            }
            catch (Exception ex)
            {                    
                formatData.ErrorLocation = errorLocation;
                formatData.ErrorMessage = ex.Message;
                formatData.ErrorStackTrace = ex.StackTrace;
                formatData.StatusCode = 501;
                formatData.Result = false;
            }
            return formatData;
        }


        [Route("Update/{id}")]
        [Produces("application/json")]
        [HttpPut]
        public async Task<FormatData<User>> Update(int id, [FromBody] User user)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "ModelState";
                user.SetId(id);
                if (!ModelState.IsValid)
                {
                    _formatData = new FormatData<User>(new List<User>(), false, 501, Constans.InvalidObject(ConstansType.User), "ModelState", locationError);
                }
                else
                {
                    locationError = "RepositoryUpdate";
                    bool result = _genericRepository.Update(user);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List < User> resultMeetings = new List<User>();
                    resultMeetings.Add(user);
                    locationError = "FormatData";
                    _formatData = new FormatData<User>(resultMeetings, result, 201, Constans.Update(ConstansType.User, id));
                }
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<User>(new List<User>(), false, 501, Constans.Error(ConstansType.Meeting), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<User>(new List<User>(), false, 501, Constans.Error(ConstansType.Meeting), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }
    }
}