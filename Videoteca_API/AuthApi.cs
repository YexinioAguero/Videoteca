using System.Net.NetworkInformation;
using System.Text;
using Videoteca_API.Models;
using Newtonsoft.Json;

namespace Videoteca_API
{
    public class AuthApi
    {
        private readonly RequestDelegate next;
        private readonly string realm;

        public AuthApi(RequestDelegate next, string realm)
        {
            this.next = next;
            this.realm = realm;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                await HandleUnauthorizedAsync(context, "No authorization header", "Please provide credentials in the Authorization header");
                return;
            }

            // Basic userid:password
            var header = context.Request.Headers["Authorization"].ToString();
            var encodedCreds = header.Substring(6);
            var creds = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCreds));
            string[] uidpwd = creds.Split(':');
            var uid = uidpwd[0];
            var password = uidpwd[1];

            if (uid != "Alex" && password != "password")
            {
                await HandleUnauthorizedAsync(context, "Invalid credentials", "Both username and password are incorrect");
                return;
            }

            if (uid != "Alex")
            {
                await HandleUnauthorizedAsync(context, "Invalid username", "The username is incorrect");
                return;
            }

            if (password != "password")
            {
                await HandleUnauthorizedAsync(context, "Invalid password", "The password is incorrect");
                return;
            }

            await next(context);

 
        }


        private async Task HandleUnauthorizedAsync(HttpContext context, string error, string details)
        {
            context.Response.StatusCode = 401;
            context.Response.Headers.Add("WWW-Authenticate", "Basic realm=\"Postman\"");

            var errorResponse = new ErrorResponse
            {
                StatusCode = 401,
                Message = "Unauthorized",
                Error = error,
                Details = details
            };

            var json = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(json, Encoding.UTF8);
        }


    }

}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string Error { get; set; }
    public string Details { get; set; }
}

