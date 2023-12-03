using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Mime;
using System.Text;
using Talabat.Core.Service_Contract;

namespace TalabatAPIs.Helper
{
    public class CashedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expiredTimeInSeconds;

        public CashedAttribute(int ExpiredTimeInSeconds)
        {
            _expiredTimeInSeconds = ExpiredTimeInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CashedService = context.HttpContext.RequestServices.GetRequiredService<IResponseCashingService>();

            var cashKey = await GenerateCashingKey(context.HttpContext.Request);

            var response = await CashedService.GetCashedResponseAsync(cashKey);

            //We Have Content
            if (!string.IsNullOrEmpty(response))
            {
                //var DezerializedResponse = j
                var content = new ContentResult()
                {
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = content;
                return;
            }

            var ExecuteContext = await next.Invoke();//Execute Action
            if (ExecuteContext.Result is OkObjectResult objectResult)
                await CashedService.CashResponseAsync(cashKey, objectResult.Value, TimeSpan.FromSeconds(_expiredTimeInSeconds));
        }

        private async Task<string> GenerateCashingKey(HttpRequest request)
        {
            var result = new StringBuilder();
            result.Append(request.Path);

            foreach (var (key,value) in request.Query.OrderBy(x => x.Key))
            {
                result.Append($"|{key}-{value}");
            }

            return result.ToString();
        }
    }
}
