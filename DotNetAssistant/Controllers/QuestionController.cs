using DotNetAssistant.Data;
using DotNetAssistant.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAssistant.Controllers;

[Route("api/[controller]")]
public class QuestionController : ControllerBase
{
    private readonly IRepository<Question> _customerRepository;
    private readonly IValidator<Question> _validator;

    public QuestionController(IRepository<Question> customerRepository, IValidator<Question> validator)
    {
        _customerRepository = customerRepository;
        _validator = validator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Question>> Question(int id)
    {
        var data = await _customerRepository.GetByIdAsync(id);
        return await Task.FromResult<ActionResult<Question>>(data);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Question>>> Questions(int pageIndex = 0, int pageSize = 10, string sortOrder = "id", string sortDirection = "asc", string keyword = "")
    {
        var data = await _customerRepository.GetAllPagedAsync(query =>
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower().Trim();
                query = query.Where(s => s.Text != null && s.Text.ToLower().Contains(keyword.ToLower()));
            }
            if (sortOrder == nameof(Entities.Question.Id).ToLower())
            {
                query = sortDirection == "asc" ? query.OrderBy(q => q.Id) : query.OrderByDescending(q => q.Id);
            }

            if (sortOrder == nameof(Entities.Question.Text).ToLower())
            {
                query = sortDirection == "asc" ? query.OrderBy(q => q.Text) : query.OrderByDescending(q => q.Text);
            }
            return query;
        }, pageIndex, pageSize);
        return await Task.FromResult<ActionResult<List<Question>>>(data.ToList());
    }
    
    [HttpPost]
    public async Task<ActionResult<Question>> CreateQuestion([FromBody] Question question)
    {
        var validationResult = await _validator.ValidateAsync(question);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(this.ModelState);
            return BadRequest(this.ModelState);
        }
        question.CreatedOnUtc = DateTime.UtcNow;
        var result = await _customerRepository.InsertAsync(question);
        return await Task.FromResult<ActionResult<Question>>(result);
    }
    
    [HttpPatch]
    public async Task<ActionResult<Question>> Update([FromBody] Question question)
    {
        var result = await _customerRepository.UpdateAsync(question);
        return await Task.FromResult<ActionResult<Question>>(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromQuery]int id)
    {
        var data = await _customerRepository.GetByIdAsync(id);
        await _customerRepository.DeleteAsync(data);
        return await Task.FromResult<ActionResult>(Ok());
    }
}