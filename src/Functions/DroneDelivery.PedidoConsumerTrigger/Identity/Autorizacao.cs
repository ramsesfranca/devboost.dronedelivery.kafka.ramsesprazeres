﻿using DroneDelivery.PedidoConsumerTrigger.Contrato;
using DroneDelivery.PedidoConsumerTrigger.Model;
using DroneDelivery.Shared.Utility.Events;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DroneDelivery.PedidoConsumerTrigger.Command
{
    public class Autorizacao : IAutorizacao
    {
        private readonly IHttpClientFactory _factory;
        private readonly  string _email ;
        private readonly string _password;


        public Autorizacao(IHttpClientFactory factory)
        {
            _factory = factory;
            _email = Environment.GetEnvironmentVariable("Login");
            _password = Environment.GetEnvironmentVariable("Senha");

        }
        public  async Task<string>   GetToken()
        {
            var client = _factory.CreateClient(HttpClientName.PedidoEndPoint);
            var loginResponse = await client.PostAsJsonAsync("/api/usuarios/login", new
            {
                _email,
                _password
            });

            loginResponse.EnsureSuccessStatusCode();
            var data = await loginResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokenDto>(data).Jwt.Token;
        }
    }
}