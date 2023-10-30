﻿using blog_rest_api.Contracts.V1.Request;
using FluentValidation;

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