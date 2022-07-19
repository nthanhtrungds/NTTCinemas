//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using Newtonsoft.Json;
//using NTTCinemas.Controllers;
//using NTTCinemas.Models.DbModels;
//using System.Threading.Tasks;

//namespace NTTCinemas.Services
//{
//    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
//    public class NTTCinemasSession
//    {
//        private readonly RequestDelegate _next;

//        public NTTCinemasSession(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public Task Invoke(HttpContext httpContext)
//        {
//            HandleCurrentBranch(httpContext);
//            return _next(httpContext);
//        }

//        // Store the current branch into session
//        private void HandleCurrentBranch(HttpContext httpContext)
//        {
//            // Get session
//            var session = httpContext.Session;
//            string key_access = "current_branch";

//            //Read session with key = nttcinemas_current_branch
//            string? valueJson = session.GetString(key_access);
//            if (BranchesController.CurrentBranch == null && valueJson != null && valueJson != "null")
//            {
//                BranchesController.CurrentBranch = JsonConvert.DeserializeObject<Branch>(valueJson);
//            }

//            //Update value in session
//            valueJson = JsonConvert.SerializeObject(BranchesController.CurrentBranch);
//            session.SetString(key_access, valueJson);
//        }

//    }

//    // Extension method used to add the middleware to the HTTP request pipeline.
//    public static class SessionMiddlewareExtensions
//    {
//        public static IApplicationBuilder UseNTTCinemasSession(this IApplicationBuilder builder)
//        {
//            return builder.UseMiddleware<NTTCinemasSession>();
//        }
//    }
//}
