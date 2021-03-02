using FluentValidation;
using Newsy.Models.BindingModels;

namespace Newsy.Validation
{
    public class ArticleBindingModelValidator : AbstractValidator<ArticleBindingModel>
    {
        public ArticleBindingModelValidator()
        {
            RuleFor(x => x.Content).NotEmpty().MinimumLength(10);
            RuleFor(x => x.Description).NotEmpty().MinimumLength(10);
            RuleFor(x => x.Title).NotEmpty().MinimumLength(10);
        }
    }
}
