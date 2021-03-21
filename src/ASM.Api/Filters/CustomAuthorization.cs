using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using ASM.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASM.Api.Filters
{
    public class CustomAuthorization : IAuthorizationFilter
    {
        private readonly ISsoService _ssoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomAuthorization(ISsoService ssoService, IHttpContextAccessor httpContextAccessor)
        {
            _ssoService = ssoService ?? throw new ArgumentNullException(nameof(ssoService));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <summary>  
        /// Authorize User  
        /// </summary>  
        /// <returns></returns>  
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext == null) return;

            var hasAllowAnonymous = filterContext.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute));

            if (hasAllowAnonymous) return;

            filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out var authTokens);

            var token = authTokens.FirstOrDefault();

            if (token != null)
            {
                if (_ssoService.ValidateToken(token).Result)
                {
                    var handler = new JwtSecurityTokenHandler();
                    var tokenInformation = handler.ReadToken(token) as JwtSecurityToken;
                    _httpContextAccessor.HttpContext?.Request.HttpContext.Items.Add("UserId",
                        (tokenInformation?.Claims ?? Array.Empty<Claim>()).FirstOrDefault(x => x.Type == "pid")
                        ?.Value);
                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    filterContext.Result = new UnauthorizedResult();
                }
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = (int) HttpStatusCode.ExpectationFailed;
                filterContext.Result = new UnauthorizedResult();
            }
        }
    }
}
