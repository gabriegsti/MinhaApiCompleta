﻿using DevIO.API.Controllers;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.API.V2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/teste")]
    public class TesteController : MainController
    {
        private readonly ILogger<TesteController> _logger;
        public TesteController(INotificador notificador, IUser appUser, ILogger<TesteController> logger) : base(notificador, appUser)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Valor()
        {

            _logger.LogInformation("Valor");


            //try
            //{
            //    var i = 0;
            //    var result = 42 / i; 
            //}
            //catch(DivideByZeroException e)
            //{
            //    e.Ship(HttpContext);
            //}



            //_logger.LogTrace("Log de Trace");
            //_logger.LogDebug("Log de Debug");
            //_logger.LogInformation("Log de Informação");
            //_logger.LogWarning("Log de Aviso");
            //_logger.LogError("Log de Erro");
            //_logger.LogCritical("Log de problema Crítico");

            return "Sou a v2";
        }
    }
}
