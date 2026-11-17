using Microsoft.Extensions.Options;
using Orders.Api.models.dtos;

namespace Orders.Api.services
{
    public class StudentService : IStudentService
    {
        private readonly IOptions<StudentInfo> _optionsStudentInfo;

        public StudentService(IOptions<StudentInfo> optionsStudentInfo)
        {
            _optionsStudentInfo = optionsStudentInfo;
        }

        public StudentInfo GetStudentInfo()
        {
            return new StudentInfo
            {
                Name = _optionsStudentInfo.Value.Name,
                Password = _optionsStudentInfo.Value.Password
            };
        }
    }
}