using DotNetAssistant.Data;
using FluentValidation;

namespace DotNetAssistant.Entities;

public class Question : BaseEntity
{
    public string? Text { get; set; }
    public DateTime CreatedOnUtc { get; set; }
}

public class QuestionValidator : AbstractValidator<Question> 
{
    public QuestionValidator()
    {
        RuleFor(x => x.Text).NotNull().WithMessage("Please input text");
    }
}