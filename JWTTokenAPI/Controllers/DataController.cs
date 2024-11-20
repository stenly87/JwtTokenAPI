using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTTokenAPI.Controllers
{
    //[Authorize(Roles = "admin")] так тоже можно
    [ApiController]
    [Route("api/data")]
    public class DataController : ControllerBase
    {
        public DataController(){}


        [HttpGet]
        public ActionResult<BaseResponce> GetForUser(){
            return Ok(new BaseResponce{
                Message = "Эта точка входа доступна всем!"
            });
        }

        [Authorize(Roles = "admin, buchgalter, marketolog")]
        [HttpGet("buch")]
        public ActionResult<BaseResponce> GetForBuchgalter(){
            var test = HttpContext.User.Claims.FirstOrDefault();
            // Получаем из клаймов Id пользователя и используем его в запросах к бд
            return Ok(new BaseResponce{
                Message = $"Эта точка входа доступа функционала бухгалтера! {test.Value}"
            });
        }

        [Authorize(Roles = "admin, marketolog")]
        [HttpGet("market")]
        public ActionResult<BaseResponce> GetForMarketolog(){
            return Ok(new BaseResponce{
                Message = "Эта точка входа доступа функционала маркетолога!"
            });
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin")]
        public ActionResult<BaseResponce> GetForAdmin(){
            return Ok(new BaseResponce{
                Message = "Эта точка входа доступа функционала админа!"
            });
        }
    }

    public class BaseResponce{
        public string Message { get; set;}
    }
}