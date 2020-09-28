using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
namespace empty_bot
{
    using System.IO;
    public static class Program
    {
        private static readonly Random rand = new Random(DateTime.Now.Millisecond);
        private static readonly string[] actions = { "move", "eat", "load", "unload" };
        private static readonly string[] directions = { "up", "down", "right", "left" };
        private static readonly JsonSerializer serializer = new JsonSerializer() {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public static async Task Main(string[] args)
        {
            await WebHost.CreateDefaultBuilder(args)
                   .UseUrls("http://*:7070")
                   .Configure(app => 
                       app.Run(async context =>
                       {
                           var reader = new StreamReader(context.Request.BodyReader.AsStream(), Encoding.Default);
                           var inputStr = reader.ReadToEnd();
                           JObject request = JObject.Parse(inputStr);
                           var orders = new List<Order>();
                           foreach(JToken ant in request["ants"])
                           {
                               orders.Add(new Order {
                                   AntId = ant.Value<int>("id"),
                                   Act = actions[rand.Next(actions.Length)],
                                   Dir = directions[rand.Next(directions.Length)]
                               });
                           }
                           await context.Response.WriteAsync(
                               JObject.FromObject(new { Orders = orders.ToArray() }, serializer).ToString());
                       })
                   )
                   .Build()
                   .RunAsync();
        }
        private class Order
        {
            public int AntId { get; set; }
            public string Act { get; set; }
            public string Dir { get; set; }
        }
    }
}
