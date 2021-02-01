using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OwinWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();

            app.MapWhen(context => context.WebSockets.IsWebSocketRequest, appBuilder => {
                    appBuilder.Run(async context => {
                        var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                        var helloBytes = Encoding.ASCII.GetBytes("Hello in WebSocket");
                        await webSocket.SendAsync(new ArraySegment<byte>(helloBytes, 0, helloBytes.Length), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);

                        byte[] buffer = new byte[1024];

                        var received = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        while(!webSocket.CloseStatus.HasValue) {
                            await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, received.Count), received.MessageType, received.EndOfMessage, CancellationToken.None);
                            received = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        }

                        await webSocket.CloseAsync(webSocket.CloseStatus.Value, webSocket.CloseStatusDescription, CancellationToken.None);

                    });

            });

            app.UseOwin(owin => owin(next => OwinHello));
        }

        private Task OwinHello(IDictionary<string, object> environment)
        {
            string response = "Hello from OWIN!";
            byte[] responseBytes = Encoding.ASCII.GetBytes(response);

            //context.Response.WriteAsync()
            var resnponseStream = (Stream)environment["owin.ResponseBody"];
            var responseHeaders = ( IDictionary<string, string[]>)environment["owin.ResponseHeaders"];
            
            responseHeaders["Content-Length"] = new string[] {responseBytes.Length.ToString()};
            responseHeaders["Content-Type"] = new string[] {"text/plain"};

            return resnponseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }
    }
}
