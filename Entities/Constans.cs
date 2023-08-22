using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    /// <summary>
    /// This class is for use the message to display in the front end
    /// </summary>
    public class Constans
    {
        public static string GetAll(ConstansType constansType)
        {
            return string.Format("Everything {0}s", constansType.ToString());
        }
        public static string Add(ConstansType constansType)
        {
            return string.Format("The {0} was added successful", constansType.ToString());
        }
        public static string Get(ConstansType constansType) {
            return string.Format("This is the {0} found", constansType.ToString());
        }
        public static string Update(ConstansType constansType, int id)
        {
            return string.Format("The {0} with the ID {1} was updated successful", constansType.ToString(),id.ToString());
        }
        public static string Delete(ConstansType constansType, int id)
        {
            return string.Format("The {0} with the ID {1} was deleted successful", constansType.ToString(), id.ToString());
        }
        public static string InvalidObject(ConstansType constansType)
        {
            return string.Format("The format of {0} object is Bad, please check and try again", constansType.ToString());
        }
        public static string Error(ConstansType constansType)
        {
            return string.Format("Error!!! Please contact to the Manager System", constansType.ToString());
        }
        public static string ErrorFound(ConstansType constansType)
        {
            return string.Format("Object Not Found", constansType.ToString());
        }
        public static string CustomMessages(int option)
        {
            switch (option)
            {
                case 0:
                    return "The Requirement require one vacancyId";
                case 1:
                    return "The Question require one VacancyId";
                case 2:
                    return "The Question require one EnumTypeId";
                case 3:
                    return "The QuestionDetails require one QuestionId";
                case 4:
                    return "The QuestionDetails require one CandidateId";
            }
            return string.Empty;
        }       
    }
    public enum ConstansType
    {         
        Stage=1,
        Vacancy=2,
        Setting=3,
        Requirement=4,
        Question=5,
        QuestionDetail=6,
        Meeting=7,
        Log=8,
        EnumType=9,
        Customer=10,
        Comment=11,
        Candidate=12,
        User=13
    }
}
