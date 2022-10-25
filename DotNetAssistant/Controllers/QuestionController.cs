using DotNetAssistant.Data;
using DotNetAssistant.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAssistant.Controllers;

[Route("api/[controller]")]
public class QuestionController : ControllerBase
{
    private readonly IRepository<Question> _customerRepository;

    public QuestionController(IRepository<Question> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    // GET api/question/data
    [HttpGet]
    public async Task<ActionResult<List<Question>>> Data()
    {
        var data = await _customerRepository.GetAllPagedAsync(questions => questions, 1, 10);
        return await Task.FromResult<ActionResult<List<Question>>>(data.ToList());
    }
}