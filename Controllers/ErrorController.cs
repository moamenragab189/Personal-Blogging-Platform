using AutoMapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal_Blogging_Platform.Exceptions;

namespace Personal_Blogging_Platform.Controllers
{
    [Route("api/[controller]")]
    [Route("/error")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
       
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (exception is AppException appException)
            { 
                return Problem(
                    detail: appException.Message
                    , statusCode: appException.StatusCode
                    );
            }
            if (exception is DbUpdateException)
            {
                return Problem(
                    detail: "A database error occurred."
                    , statusCode: StatusCodes.Status500InternalServerError
                    );
            }

            
            if (exception is AutoMapperMappingException)
            {
                return Problem(
                   detail: "A mapping error occurred."
                   , statusCode: StatusCodes.Status500InternalServerError
                   );
            }


            return Problem(
                   detail: "An unexpected error occurred."
                   , statusCode: StatusCodes.Status500InternalServerError
                   );
        }
    }
}
