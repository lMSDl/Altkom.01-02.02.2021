using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Models;
using Services.Interfaces;
using WebApi.Controllers;

namespace WebApi.Hubs
{
    public class PeopleHub : Hub
    {
        private IService<Person> Service {get;}

        private Dictionary<string, Timer> _timers = new Dictionary<string, Timer>();

        public PeopleHub(IService<Person> service)
        {
            Service = service;
        }

        public async override Task OnConnectedAsync()
        {
            var timer = new Timer(TimerCaller, Clients.Caller ,TimeSpan.FromSeconds(5),TimeSpan.FromSeconds(5));
            _timers.Add(Context.ConnectionId, timer); 
            System.Console.WriteLine(Context.ConnectionId);
            
            await base.OnConnectedAsync();
            
        }

        private void TimerCaller(object state) {
            ((IClientProxy)state).SendAsync("Pong", "");
        }

        public async Task Ping() {
            await Clients.Caller.SendAsync("Pong", string.Empty);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            System.Console.WriteLine(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task AddPerson(Person person) {
            person = await Service.CreateAsync(person);
            await Clients.Group("Add").SendAsync(nameof(PeopleController.Post), person);
        }

        public async Task JoinToAddGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Add");
        }
        public async Task JoinToDeleteGroup() {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Delete");
        }
    }
}