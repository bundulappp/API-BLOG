using FluentValidation;
using Models.Contracts.V1.Requests;

namespace blog_rest_api.Validators
{
    public class CreateBlogRequestValidator : AbstractValidator<CreateBlogRequest>
    {
        public CreateBlogRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(1, 250);
        }
    }
}
