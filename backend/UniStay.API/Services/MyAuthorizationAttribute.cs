namespace UniStay.API.Services;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]


    public class MyAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _requiredRole;

        public MyAuthorizationAttribute(string requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authService = context.HttpContext.RequestServices.GetService(typeof(IMyAuthService)) as IMyAuthService;
            if (authService == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var authInfo = authService.GetAuthInfoFromRequest();
            if (!authInfo.IsLoggedIn)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!string.IsNullOrEmpty(_requiredRole) && authInfo.RoleName != _requiredRole)
            {
                context.Result = new ForbidResult();
            }
        }
    }

