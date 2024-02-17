using FluentValidation;
using Models.Contracts.V1.Requests;

namespace blog_rest_api.Validators
{
    public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
    {
        public CreateCommentRequestValidator()
        {
            RuleFor(x => x.Body).NotEmpty().MaximumLength(1000);
        }
    }
}
