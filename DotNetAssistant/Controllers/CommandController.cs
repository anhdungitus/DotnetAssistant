﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAssistant.Controllers;

[Route("api/[controller]")]
[Authorize]
public class CommandController : ControllerBase
{
    [HttpGet("{term}")]
    public Task<ActionResult<IEnumerable<string>>> GetCommand(string term)
    {
        var result = new List<string>();
        term = term.ToLower();
        var lastWord = term.Split(' ').Last();

        if (term.Contains("angular"))
        {
            if (term.Contains("cli"))
            {
                if (term.Contains("install"))
                {
                    result.Add("npm install -g @angular/cli");
                }
            }
        } else if (term.Contains("dotnet"))
        {
            
        }
        
        if (term.Contains("update"))
        {
            if (term.Contains("database"))
            {
                result.Add("dotnet ef database update");
            }
        }
        else if (term.Contains("migration"))
        {
            result.Add($"dotnet ef migrations add {lastWord}");
        } else if (term.Contains("create"))
        {
            if (term.Contains("asp") && term.Contains("core") && term.Contains("api"))
            {
                result.Add($"dotnet new webapi -–name {lastWord}");
            }
        } else if (term.Contains("sql connection string"))
        {
            result.Add("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=test.api;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        } else if (term.Contains("angular") && term.Contains("component"))
        {
            result.Add($"ng generate component {lastWord}");
        } else if (term.Contains("angular") && term.Contains("service"))
        {
            result.Add($"ng generate service {lastWord}");
        }else if (term.Contains("angular") && term.Contains("new") && term.Contains("app"))
        {
            result.Add($"ng new {lastWord}");
        } else if (term.Contains("nuget") || term.Contains("package") || term.Contains("install"))
        {
            if (term.Contains("entity") || term.Contains("framework"))
            {
                result.Add("dotnet add package Microsoft.EntityFrameworkCore --version 6.0.10");
            }

            if (term.Contains("code first") || term.Contains("design"))
            {
                result.Add("dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.10");
            }
            
            if (term.Contains("sql"))
            {
                result.Add("dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.10");
            }
        }
        else
        {
            result.Add("I don't understand you, please contact anhdung.itus@gmail.com for support!");
        }
        
        return Task.FromResult<ActionResult<IEnumerable<string>>>(result);
    }
}