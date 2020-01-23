using System;
using System.Web.Mvc;
using AirPro.Logging;
using AirPro.Logging.Interface;
using Microsoft.AspNet.Identity;

namespace AirPro.Site.Filters
{
    public class RequestLoggingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                // Check For Context.
                if (filterContext?.HttpContext != null)
                {
                    // Create New Request.
                    var request =
                        new RequestLogEntryDto
                        {
                            SessionId = filterContext.HttpContext?.Session?.SessionID,
                            RawUrl = filterContext.HttpContext?.Request?.RawUrl,
                            UserAddress = filterContext.HttpContext?.Request?.UserHostAddress,
                            UserAgent = filterContext.HttpContext?.Request?.UserAgent,
                            RequestMethod = filterContext.HttpContext?.Request?.HttpMethod,
                            RouteUrl = (filterContext.RouteData?.Route as System.Web.Routing.Route)?.Url,
                            RouteArea = filterContext.RouteData?.DataTokens?["area"]?.ToString(),
                            RouteController = filterContext.RouteData?.Values?["controller"]?.ToString(),
                            RouteAction = filterContext.RouteData?.Values?["action"]?.ToString(),
                            RouteId = filterContext.RouteData?.Values?["id"]?.ToString(),
                            ActionStartTime = DateTimeOffset.UtcNow
                        };

                    // Check Authenticated.
                    if (!string.IsNullOrEmpty(filterContext.HttpContext?.User?.Identity?.GetUserId()))
                        request.UserGuid = Guid.Parse(filterContext.HttpContext?.User?.Identity?.GetUserId());

                    // Store Request.
                    filterContext.HttpContext.Items["RequestLog"] = request;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            base.OnActionExecuting(filterContext);
        }


        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                // Check Request Log.
                if (filterContext?.HttpContext?.Items["RequestLog"] is IRequestLogEntryDto request)
                {
                    // Set End Time.
                    request.ActionEndTime = DateTimeOffset.UtcNow;

                    // Check Exception.
                    if (filterContext.Exception != null)
                    {
                        // Store Exception Info.
                        request.ActionExceptionMessage = filterContext.Exception?.Message;
                        request.ActionExceptionStackTrace = filterContext.Exception?.StackTrace;

                        // Save Log.
                        Logger.LogRequest(request);
                    }

                    // Update Request.
                    filterContext.HttpContext.Items["RequestLog"] = request;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            try
            {
                // Check Request Log.
                if (filterContext?.HttpContext?.Items["RequestLog"] is IRequestLogEntryDto request)
                {
                    // Set Start Time.
                    request.ResultStartTime = DateTimeOffset.UtcNow;

                    // Update Request.
                    filterContext.HttpContext.Items["RequestLog"] = request;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            try
            {
                // Check Request Log.
                if (filterContext?.HttpContext?.Items["RequestLog"] is IRequestLogEntryDto request)
                {
                    // Set End Time.
                    request.ResultEndTime = DateTimeOffset.UtcNow;

                    // Check Exception.
                    if (filterContext.Exception != null)
                    {
                        // Store Exception Info.
                        request.ResultExceptionMessage = filterContext.Exception?.Message;
                        request.ResultExceptionStackTrace = filterContext.Exception?.StackTrace;
                    }

                    // Save Log.
                    Logger.LogRequest(request);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            base.OnResultExecuted(filterContext);
        }
    }
}