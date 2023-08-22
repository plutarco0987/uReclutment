using DataAccess.Generic;
using Entities;
using Entities.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Packaging;
using NuGet.Protocol;

namespace uReclutment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacancyController : ControllerBase
    {
        private readonly IGenericRepository<Vacancy> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private FormatData<Vacancy> _formatData;
        public VacancyController(IGenericRepository<Vacancy> genericRepository, IUnitOfWork unitOfWork)
        {
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
            this._formatData= new FormatData<Vacancy>();
        }

        [Route("GetAllVacancy")]
        [Produces("application/json")]
        [HttpGet ]
        public async Task<FormatData<VacancyFormat>> GetAllVacancy()
        {
            string locationError = string.Empty;
            FormatData<VacancyFormat> formatData = new FormatData<VacancyFormat>();
            List<VacancyFormat> vacancyFormat = new List<VacancyFormat>();
            try
            {
                locationError = "GetAllVacancy";
                IEnumerable<Vacancy> result = await _genericRepository.GetAsync();
                List<Questions> questions = await _unitOfWork.Context.Set<Questions>().ToListAsync();                                
                // we require the ids of the customers and the ids for can add the another point in the grid of the view 
                FormatData<IdClass> ids= ExecuteController.GetListIdClass(0, _unitOfWork, new List<IdClass>());

                foreach (Vacancy item in result)
                {
                    item.Questions.Clear();

                    foreach (Questions item2 in questions)
                    {
                        if ((item2.VacancyId == item.VacancyId) && item2.Active)
                            item.Questions.Add(item2);
                    }
                    //item.Questions.AddRange(questions.FindAll(questions => questions.VacancyId == item.VacancyId && questions.Active));

                    vacancyFormat.Add(new VacancyFormat(item,ids.Data));
                }
                formatData = new FormatData<VacancyFormat>(vacancyFormat, true, 201,Constans.GetAll(ConstansType.Vacancy));                 
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if(result)
                    formatData = new FormatData<VacancyFormat>(new List<VacancyFormat>(), false, 501, Constans.Error(ConstansType.Vacancy), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<VacancyFormat>(new List<VacancyFormat>(), false, 501, Constans.Error(ConstansType.Vacancy), "Error in Log", locationError, "Error");
            }

            return formatData;
        }

        [Route("GetById/{id}")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<FormatData<Vacancy>> GetById(int id)
        {
            string locationError = string.Empty;
            try
            {                                    
                locationError = "GetById";
                Vacancy result = await _genericRepository.GetById(id);
                if(result==null)
                    return new FormatData<Vacancy>(new List<Vacancy>(), false, 201,Constans.ErrorFound(ConstansType.Vacancy), "Object not found", locationError);

                List<Vacancy> resultVacancy = new List<Vacancy>();
                resultVacancy.Add(result);
                locationError = "FormatData";
                _formatData = new FormatData<Vacancy>(resultVacancy, true, 201,Constans.Get(ConstansType.Vacancy));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Vacancy>(new List<Vacancy>(), false, 501, Constans.Error(ConstansType.Vacancy), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Vacancy>(new List<Vacancy>(), false, 501, Constans.Error(ConstansType.Vacancy), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }

        [Route("AddVacancy")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<FormatData<VacancyFormat>> AddVacancy([FromBody] VacancyFormat Vacancy)
        {
            string locationError = string.Empty;

            FormatData<VacancyFormat> formatData = new FormatData<VacancyFormat>();
            try
            {
                locationError = "ModelState";
                if (!ModelState.IsValid)
                {
                    formatData = new FormatData<VacancyFormat>(new List<VacancyFormat>(), false, 501, Constans.ErrorFound(ConstansType.Vacancy), "ModelState", locationError);                    
                }
                else
                {
                    //we added the datetime
                    //NOTE: the order is setting by the user because he/she require know the order of the Vacancy
                    DateTime date = DateTime.Now;
                    Vacancy.DateCreated = date;
                    Vacancy.DateModified = date;
                    Vacancy vacancyAux = new Vacancy(Vacancy);  
                    
                    vacancyAux.Customers = await _unitOfWork.Context.Set<Customers>().FindAsync(vacancyAux.CustomersId);

                    locationError = "RepositoryCreate";
                    bool result = await _genericRepository.CreateAsync(vacancyAux);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<VacancyFormat> resultVacancy = new List<VacancyFormat>();
                    resultVacancy.Add(Vacancy);
                    locationError = "FormatData";
                    formatData = new FormatData<VacancyFormat>(resultVacancy, result, 201,Constans.Add(ConstansType.Vacancy));
                }                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<VacancyFormat>(new List<VacancyFormat>(), false, 501, Constans.Error(ConstansType.Vacancy), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<VacancyFormat>(new List<VacancyFormat>(), false, 501, Constans.Error(ConstansType.Vacancy), "Error in Log", locationError, "Error");
            }
            return formatData;
        }

        [Route("UpdateVacancy/{id}")]
        [Produces("application/json")]
        [HttpPut]
        public async Task<FormatData<VacancyFormat>> UpdateVacancy(int id,[FromBody] VacancyFormat Vacancy)
        {
            string locationError = string.Empty;
            FormatData<VacancyFormat> formatData = new FormatData<VacancyFormat>();
            try
            {
                locationError = "ModelState";
                //Vacancy.SetId(id);
                if (!ModelState.IsValid)
                {
                    formatData = new FormatData<VacancyFormat>(new List<VacancyFormat>(), false, 501, Constans.InvalidObject(ConstansType.Vacancy), "ModelState", locationError);
                }
                else
                {
                    Vacancy vacancyOriginal = await _genericRepository.GetById(id);

                    //Vacancy.DateModified = DateTime.Now;
                    //Vacancy vacancyAux = new Vacancy(Vacancy);

                    vacancyOriginal.Active= Vacancy.Active;
                    vacancyOriginal.CustomersId= Vacancy.CustomersId;
                    vacancyOriginal.NameModified = Vacancy.NameModified;
                    vacancyOriginal.DateModified= DateTime.Now;
                    vacancyOriginal.Description= Vacancy.Description;
                    vacancyOriginal.Responsabilitys= Vacancy.Responsabilitys;
                    vacancyOriginal.NamePosition= Vacancy.NamePosition;
                    vacancyOriginal.Name= Vacancy.Name;                    

                    //vacancyAux.Customers = await _unitOfWork.Context.Set<Customers>().FindAsync(vacancyAux.CustomersId);
                    //vacancyAux.Questions = vacancyOriginal.Questions;
                    //vacancyAux.SetId(id);
                    //vacancyAux.Requirements = vacancyOriginal.Requirements;
                    //vacancyAux.Candidates = vacancyOriginal.Candidates;
                        

                    locationError = "RepositoryUpdate";
                    bool result = _genericRepository.Update(vacancyOriginal);
                    locationError = "Commit";
                    if (result)
                        _unitOfWork.Commit();

                    List<VacancyFormat> resultVacancy = new List<VacancyFormat>();
                    resultVacancy.Add(Vacancy);
                    locationError = "FormatData";
                    formatData = new FormatData<VacancyFormat>(resultVacancy, result, 201,Constans.Update(ConstansType.Vacancy,id));
                }
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    formatData = new FormatData<VacancyFormat>(new List<VacancyFormat>(), false, 501, Constans.Error(ConstansType.Vacancy), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    formatData = new FormatData<VacancyFormat>(new List<VacancyFormat>(), false, 501, Constans.Error(ConstansType.Vacancy), "Error in Log", locationError, "Error");
            }
            return formatData;
        }

        [Route("DeleteVacancy/{id}")]
        [Produces("application/json")]
        [HttpDelete]
        public async Task<FormatData<Vacancy>> DeleteVacancy(int id)
        {
            string locationError = string.Empty;
            try
            {
                locationError = "GetId";
                Vacancy requestOriginal =  await _genericRepository.GetById(id);
                if(requestOriginal==null)
                    return new FormatData<Vacancy>(new List<Vacancy>(), false, 501,Constans.ErrorFound(ConstansType.Vacancy), "Object not found", locationError);

                locationError = "UpdateVacancy";
                requestOriginal.Active = false;
                var result = _unitOfWork.Context.Set<Vacancy>().Update(requestOriginal);

                locationError = "Commit";
                if (result!=null)
                    _unitOfWork.Commit();

                List<Vacancy> resultVacancy = new List<Vacancy>();
                resultVacancy.Add(requestOriginal);
                locationError = "FormatData";
                _formatData = new FormatData<Vacancy>(resultVacancy, true, 201, Constans.Delete(ConstansType.Vacancy,id));                
            }
            catch (Exception ex)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bool result = await _genericRepository.AddLog(ex.Message, ex.StackTrace);
#pragma warning restore CS8604 // Possible null reference argument.
                if (result)
                    _formatData = new FormatData<Vacancy>(new List<Vacancy>(), false, 501, Constans.Error(ConstansType.Vacancy), ex.Message, locationError, ex.StackTrace != null ? ex.StackTrace : string.Empty);
                else
                    _formatData = new FormatData<Vacancy>(new List<Vacancy>(), false, 501, Constans.Error(ConstansType.Vacancy), "Error in Log", locationError, "Error");
            }
            return _formatData;
        }
    }
}