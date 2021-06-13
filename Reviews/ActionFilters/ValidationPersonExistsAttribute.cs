using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Reviews.ActionFilters
{
    public class ValidationPersonExistsAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;

        public ValidationPersonExistsAttribute(ILoggerManager logger, IRepositoryManager repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = method.Equals("PUT") || method.Equals("PATCH");

            var id = (Guid)context.ActionArguments["id"];

            var person = await _repository.Person.GetPersonAsync(id, trackChanges);
            if (person == null)
            {
                _logger.LogInfo($"Person with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }

            context.HttpContext.Items.Add("person", person);
            
            await next();
        }
    }
}
