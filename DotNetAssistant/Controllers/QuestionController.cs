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

    [HttpGet("{id}")]
    public async Task<ActionResult<Question>> Question(int id)
    {
        var data = await _customerRepository.GetByIdAsync(id);
        return await Task.FromResult<ActionResult<Question>>(data);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Question>>> Questions()
    {
        var data = await _customerRepository.GetAllPagedAsync(questions => questions, 1, 10);
        return await Task.FromResult<ActionResult<List<Question>>>(data.ToList());
    }
    
    [HttpPost]
    public async Task<ActionResult<Question>> CreateQuestion([FromBody] Question question)
    {
        var result = await _customerRepository.InsertAsync(question);
        return await Task.FromResult<ActionResult<Question>>(result);
    }
    
    [HttpPatch]
    public async Task<ActionResult<Question>> Update([FromBody] Question question)
    {
        var result = await _customerRepository.UpdateAsync(question);
        return await Task.FromResult<ActionResult<Question>>(result);
    }
    
    [HttpDelete]
    public async Task<ActionResult<Question>> Delete([FromBody] Question question)
    {
        await _customerRepository.DeleteAsync(question);
        return await Task.FromResult<ActionResult<Question>>(question);
    }
}