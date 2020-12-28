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
        // available actions and directions
        private static readonly string[] actions = { "move", "eat", "take", "put" };
        private static readonly string[] directions = { "up", "down", "right", "left" };
        private static readonly JsonSerializer serializer = new JsonSerializer() {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public static async Task Main(string[] args)
        {
            await WebHost.CreateDefaultBuilder(args)
                   .UseUrls("http://*:7070") // starting listen for http calls on port :7070
                   .Configure(app => 
                       app.Run(async context =>
                       {
                           var reader = new StreamReader(context.Request.BodyReader.AsStream(), Encoding.Default);
                           var inputStr = reader.ReadToEnd();
                           // prepare json object
                           JObject request = JObject.Parse(inputStr);
                           var orders = new List<Order>();
                           // loop through ants and give orders
                           foreach(JToken ant in request["ants"])
                           {
                               orders.Add(new Order { // add order to your response object from line 36
                                   AntId = ant.Value<int>("id"),
                                   Act = actions[rand.Next(actions.Length)], // pick random action from array on line 21
                                   Dir = directions[rand.Next(directions.Length)] // pick random direction from array on line 22
                               });
                           }
                           await context.Response.WriteAsync(
                               JObject.FromObject(new { Orders = orders.ToArray() }, serializer).ToString());
                       })
                   )
                   .Build()
                   .RunAsync();
        }
        // response json should look something like this
        // {"orders": [
        //	 {"antId":1,"act":"move","dir":"down"},
        //	 {"antId":17,"act":"move","dir":"up"}
        //	]}

        private class Order
        {
            public int AntId { get; set; }
            public string Act { get; set; }
            public string Dir { get; set; }
        }
    }
}

// this code available at https://github.com/anthive/csharp
// to test it localy, submit post request with payload.json using postman or curl
// curl -X 'POST' -d @payload.json http://localhost:7070

// have fun!

