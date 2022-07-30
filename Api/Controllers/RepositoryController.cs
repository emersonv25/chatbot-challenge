using api_git.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace api_git.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        // GET: api/<RepositoryController>
        [HttpGet]
        public List<Repository> Get(string userLogin = "takenet", string sort = "created_at", int per_page = 5, string order = "asc", string language = "c#")
        {
            List<Repository> repository = new List<Repository>();
            RestClient client = new RestClient("https://api.github.com");
            var request = new RestRequest("/users/" + userLogin + "/repos?sort=" + sort + "&per_page=200&direction=" + order + "&page=1}", Method.Get);
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = response.Content;
                repository = JsonConvert.DeserializeObject<List<Repository>>(json);
                repository = repository
                    .Where(x => !string.IsNullOrWhiteSpace(x.language) && x.language.ToLower().Equals(language))
                    .Take(per_page)
                    .ToList();
            }
            return repository;
        }
    }
}
