using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace AntHiveBotSample
{  
    public class Request
    {
        public Ant[] ants { get; set; }
    }

    public class Ant
    {
        public int id { get; set; }
    }

    public class Response
    {
        public Order[] orders { get; set; }
    }
    
    public class Order
    {
        public int antId { get; set; }
        public string act { get; set; }
        public string dir { get; set; }
    }

    public class Program
    {
        public async static Task Main(string[] args)
        {
            await CreateHostBuilder(args)
                .Build()
                .RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                
                .ConfigureWebHostDefaults(webBuilder => 
                { webBuilder
                    .UseUrls("http://*:7070")
                    .UseStartup<Startup>(); 
                });
    }
    
    public class Startup
    {
        private static readonly Random rand = new Random(DateTime.Now.Millisecond);
        // available actions and directions
        private static readonly string[] actions = { "move", "eat", "take", "put" };
        private static readonly string[] directions = { "up", "down", "right", "left" };

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost("/", async context =>
                {
                    // prepare json object from request
                    var reader = new StreamReader(context.Request.BodyReader.AsStream(), Encoding.Default);
                    var inputStr = reader.ReadToEnd();
                    var request = JsonSerializer.Deserialize<Request>(inputStr);

                    var orders = new List<Order>();
                    // loop through ants and give orders
                    foreach (var ant in request.ants)
                    {
                        // add order to your response object from line 36
                        orders.Add(new Order
                        {
                            antId = ant.id,
                            // pick random action from array on line 21
                            dir = directions[rand.Next(directions.Length)],
                            act = "move"
                        });
                    }

                    var output = new Response();
                    output.orders = orders.ToArray();
                    await context.Response.WriteAsync(JsonSerializer.Serialize(output));
                    // your json response should like this
                    // {"orders": [
                    //	 {"antId":1,"act":"move","dir":"down"},
                    //	 {"antId":17,"act":"load","dir":"up"}
                    //	]}
                });
            });
        }
    }
}

// this code available at https://github.com/anthive/csharp
// to test it localy, submit post request with payload.json using postman or curl
// curl -X 'POST' -d @payload.json http://localhost:7070

// have fun!

