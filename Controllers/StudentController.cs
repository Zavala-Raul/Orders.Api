using Microsoft.AspNetCore.Mvc;
using Orders.Api.services;

namespace Orders.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public IActionResult GesStudentInformation()
        {
            var studentInfo = _studentService.GetStudentInfo();
            return StatusCode(200, new{studentInfo});
        }
    }
}