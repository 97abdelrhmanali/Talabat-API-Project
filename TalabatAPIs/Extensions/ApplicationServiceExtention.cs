using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Service_Contract;
using Talabat.Repository;
using Talabat.Repository.Repository;
using Talabat.Service;
using TalabatAPIs.Errors;
using TalabatAPIs.Helper;

namespace TalabatAPIs.Extensions
{
    public static class ApplicationServiceExtention
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddSingleton(typeof(IResponseCashingService),typeof(ResponseCashingService));
            Services.AddScoped(typeof(IOrderService), typeof(OrderServices));
            Services.AddScoped(typeof(IUnitOfWork) , typeof(UnitOfWork));
            Services.AddScoped(typeof(IProductServices),typeof(ProductServices));
            Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            //Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddAutoMapper(typeof(MappingProfile));

            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(E => E.ErrorMessage)
                                                         .ToArray();
                    var vallidationErrorResponse = new ApiVallidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(vallidationErrorResponse);
                };
            });

            return Services;

        }
    }
}
