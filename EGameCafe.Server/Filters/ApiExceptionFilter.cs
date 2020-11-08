using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Common.Models;
using Laboratory.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGameCafe.Server.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilter()
        {
            // Register known exception types and handlers.
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(SMSException), HandleSMSException },
                { typeof(RequestTimeoutException), HandleRequestTimeout },
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            Type type = context.Exception.GetType();

            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            HandleUnknownException(context);
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            var details = Result.Failure("An error occurred while processing your request.","خطا در انجام عملیات"
                ,type: "https://tools.ietf.org/html/rfc7231#section-6.6.1" ,status: StatusCodes.Status500InternalServerError);
           
            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }

        private void HandleValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;
            var details = Result.Failure(exception.Message, "اطلاعات کامل وارد نشده است" , type: "https://tools.ietf.org/html/rfc7231#section-6.5.1");
      
            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;

            var details = Result.Failure(exception.Message, "یافت نشد", "Failure",
                "https://tools.ietf.org/html/rfc7231#section-6.5.4", 404, "NotFount");
        
            context.Result = new NotFoundObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleSMSException(ExceptionContext context)
        {
            var exception = context.Exception as SMSException;

            var details = Result.Failure("Sending may times", exception.Message, status: StatusCodes.Status500InternalServerError);

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }

        private void HandleRequestTimeout(ExceptionContext context)
        {
            var exception = context.Exception as RequestTimeoutException;

            var details = Result.Failure("RequestTimeoutException", exception.Message);

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };

            context.ExceptionHandled = true;
        }

    }
}
