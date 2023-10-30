using blog_rest_api.Contracts.V1.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace blog_rest_api.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                // If the ModelState is invalid, filter out the errors present in the ModelState.
                var errorsInModelState = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        keyValuePair => keyValuePair.Key,
                        keyValuePair => keyValuePair.Value.Errors.Select(x => x.ErrorMessage)
                    )
                    .ToArray();

                // Create an ErrorResponse object to collect all errors.
                var errorResponse = new ErrorResponse();

                // Iterate over each error in the ModelState, and populate the ErrorResponse object.
                foreach (var error in errorsInModelState)
                {
                    foreach (var subError in error.Value)
                    {
                        // Create an ErrorModel for each error and populate the necessary information.
                        var errorModel = new ErrorModel
                        {
                            FieldName = error.Key,
                            Message = subError
                        };

                        // Add each ErrorModel to the ErrorResponse object.
                        errorResponse.Errors.Add(errorModel);
                    }
                }

                // Set the Result of the context to a BadRequest with the ErrorResponse object. 
                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            await next();
        }
    }
}
