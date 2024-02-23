using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http;
using System.Security.Claims;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{


    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Получение значения роли из токена
        var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        if (roleClaim == null)
        {
            context.Result = new ForbidResult();
            return;
        }

        var RoleName = roleClaim.Value;


        switch (RoleName)
        {
            case "Admin":
                // Администратору разрешены все типы запросов
                return;
            case "Moderator":
                // Модератору разрешены GET и PUT запросы
                var method = context.HttpContext.Request.Method.ToUpper();
                if (method != "GET" && method != "PUT")
                {
                    context.Result = new ForbidResult();
                    return;
                }
                break;
            case "User":
                // Пользователю разрешены только GET запросы
                if (context.HttpContext.Request.Method.ToUpper() != "GET")
                {
                    context.Result = new ForbidResult();
                    return;
                }
                break;
            default:
                // Всем остальным ролям запрещен доступ
                context.Result = new ForbidResult();
                return;
        }
    }


}