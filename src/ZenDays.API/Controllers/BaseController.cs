using Microsoft.AspNetCore.Mvc;
using ZenDays.Core.Models;

namespace ZenDays.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected ObjectResult Result(ResultViewModel responseModel) =>
             StatusCode(responseModel.StatusCode, responseModel);
    }
}
