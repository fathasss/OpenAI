using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using HasGenerator.API_v2.Models;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace HasGenerator.API_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HasAIController : ControllerBase
    {
        [Route("UseChat")]
        [HttpGet]
        public async Task<IActionResult> UseChat(string prompt)
        {
            string outResult = string.Empty;
            var apiReferance = new APIReferances();
            var openAI = new OpenAIAPI(apiKeys:apiReferance.API_KEY);
            CompletionRequest compRequest = new CompletionRequest()
            {
                Prompt = prompt,
                Model = Model.DavinciText
            };

            var completionCollections = openAI.Completions.CreateCompletionAsync(compRequest);

            foreach (var completion in completionCollections.Result.Completions)
            {
                outResult += completion.Text;
            }
            return Ok(outResult);
        }

        [Route("UseImage")]
        [HttpGet]
        public async Task<IActionResult> UseImage(string prompt)
        {
            var apiReferance = new APIReferances();
            string body = "{\"prompt\": \""+prompt+ "\",\"n\": 2,\"size\": \"1024x1024\"}";
            var data = Encoding.ASCII.GetBytes(body);
            var request = (HttpWebRequest)WebRequest.Create("https://api.openai.com/v1/images/generations");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + apiReferance.API_KEY);

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);               
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseStr = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return Ok(responseStr);
        }
    }
}
