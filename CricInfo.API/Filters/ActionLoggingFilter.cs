using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace CricInfo.API.Filters;

public class ActionLoggingFilter : IActionFilter
{
    private readonly ILogger<ActionLoggingFilter> _logger;
    private readonly Stopwatch _stopwatch = new();

    public ActionLoggingFilter(
        ILogger<ActionLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var controller =
            (context.ActionDescriptor as ControllerActionDescriptor)?.ControllerName;

        var action =
            (context.ActionDescriptor as ControllerActionDescriptor)?.ActionName;

        _logger.LogInformation("=================================");
        _logger.LogInformation("Request Started");
        _logger.LogInformation("Controller : {Controller}", controller);
        _logger.LogInformation("Action     : {Action}", action);
        _logger.LogInformation("Method     : {Method}", context.HttpContext.Request.Method);
        _logger.LogInformation("Path       : {Path}", context.HttpContext.Request.Path);

        foreach (var arg in context.ActionArguments)
        {
            _logger.LogInformation(
                "Parameter {Name}: {@Value}",
                arg.Key,
                arg.Value);
        }

        _stopwatch.Restart();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _stopwatch.Stop();

        _logger.LogInformation("Status Code : {StatusCode}",
            context.HttpContext.Response.StatusCode);

        _logger.LogInformation("Execution Time : {Elapsed} ms",
            _stopwatch.ElapsedMilliseconds);

        if (context.Exception != null)
        {
            _logger.LogError(
                context.Exception,
                "Exception occurred while executing the action.");
        }

        _logger.LogInformation("Request Completed");
        _logger.LogInformation("=================================");
    }
}