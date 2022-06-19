using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Turnit.Service.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Turnit.GenericStore.Api.Filters
{
    public class ExceptionHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException)
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Content = context.Exception.Message
                };
                context.ExceptionHandled = true;
            }

            if (context.Exception is InvalidOperationException)
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Content = context.Exception.Message
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
