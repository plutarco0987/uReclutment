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
    public class CustomersController : ControllerBase
    {
        private readonly IGenericRepository<Customers> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private FormatData<Customers> _formatData;
        public CustomersController(IGenericRepository<Customers> genericRepository, IUnitOfWork unitOfWork)
        {
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
            this._formatData= new FormatData<Customers>();
        }

        [Route("GetAllCustomers")]
        [Produces("application/json")]
        [HttpGet ]
        public async Task<FormatData<Customers>> GetAllCustomers()
        {
            string locationError = string.Empty;            
            try
            {
                locationError = "GetAllCustomers";
                IEnumerable<Customers> result = await _genericRepository.GetAsync();
                _formatData = new FormatData<Customers>(result, true, 201,Constans.GetAll(ConstansType.Customer));                 
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if(result)
                    _formatData = new FormatData<Customers>(new List<Customers>(), false, 501, Constans.Error(ConstansType.Customer), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Customers>(new List<Customers>(), false, 501, Constans.Error(ConstansType.Customer), "Error in Log", locationError, "Error");
            }

            return _formatData;
        }

        [Route("GetById/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<Customers>> GetById(int id)
        {
            string locationError = string.Empty;
            try
            {                                    
                locationError = "GetById";
                Customers result = await _genericRepository.GetById(id);
                if(result==null)
                    return new FormatData<Customers>(new List<Customers>(), false, 201,Constans.ErrorFound(ConstansType.Customer), "Object not found", locationError);

                List<Customers> resultCustomers = new List<Customers>();
                resultCustomers.Add(result);
                locationError = "FormatData";
                _formatData = new FormatData<Customers>(resultCustomers, true, 201,Constans.Get(ConstansType.Customer));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Customers>(new List<Customers>(), false, 501, Constans.Error(ConstansType.Customer), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Customers>(new List<Customers>(), false, 501, Constans.Error(ConstansType.Customer), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("AddCustomer")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<FormatData<Customers>> AddCustomer([FromBody] Customers Customers)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    _formatData = new FormatData<Customers>(new List<Customers>(), false, 501, Constans.ErrorFound(ConstansType.Customer), "ModelState", locationError);                    
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the Customers
                    Customers.DateCreated=DateTime.Now;
                    Customers.DateModified = DateTime.Now;
                    locationError = "RepositoryCreate";
                    bool result = await _genericRepository.CreateAsync(Customers);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<Customers> resultCustomers = new List<Customers>();
                    resultCustomers.Add(Customers);
                    locationError = "FormatData";
                    _formatData = new FormatData<Customers>(resultCustomers, result, 201,Constans.Add(ConstansType.Customer));
                }                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Customers>(new List<Customers>(), false, 501, Constans.Error(ConstansType.Customer), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Customers>(new List<Customers>(), false, 501, Constans.Error(ConstansType.Customer), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("UpdateCustomer/{id}")]
        [Produces("application/json")]
        [HttpPut]
        public async Task<FormatData<Customers>> UpdateCustomer(int id,[FromBody] Customers Customers)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "ModelState";
                Customers.SetId(id);
                if (!ModelState.IsValid)
                {
                    _formatData = new FormatData<Customers>(new List<Customers>(), false, 501, Constans.InvalidObject(ConstansType.Customer), "ModelState",locationError);
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the Customers                    
                    Customers.DateModified = DateTime.Now;
                    locationError = "RepositoryUpdate";
                    bool result = _genericRepository.Update(Customers);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<Customers> resultCustomers = new List<Customers>();
                    resultCustomers.Add(Customers);
                    locationError = "FormatData";
                    _formatData = new FormatData<Customers>(resultCustomers, result, 201,Constans.Update(ConstansType.Customer,id));
                }
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Customers>(new List<Customers>(), false, 501, Constans.Error(ConstansType.Customer), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Customers>(new List<Customers>(), false, 501, Constans.Error(ConstansType.Customer), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("DeleteCustomer/{id}")]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<FormatData<Customers>> DeleteCustomer(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetId";
                Customers requestOriginal =  await _genericRepository.GetById(id);
                if(requestOriginal==null)
                    return new FormatData<Customers>(new List<Customers>(), false, 501,Constans.ErrorFound(ConstansType.Customer), "Object not found", locationError);

                locationError = "UpdateCustomer";
                requestOriginal.Active = false;
                var result = _unitOfWork.Context.Set<Customers>().Update(requestOriginal);

                locationError = "Commit";
                if (result!=null)
                    _unitOfWork.Commit();

                List<Customers> resultCustomers = new List<Customers>();
                resultCustomers.Add(requestOriginal);
                locationError = "FormatData";
                _formatData = new FormatData<Customers>(resultCustomers, true, 201, Constans.Delete(ConstansType.Customer,id));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Customers>(new List<Customers>(), false, 501, Constans.Error(ConstansType.Customer), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Customers>(new List<Customers>(), false, 501, Constans.Error(ConstansType.Customer), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }
    }
}