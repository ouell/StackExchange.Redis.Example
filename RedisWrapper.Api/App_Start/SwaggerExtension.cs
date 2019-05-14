using Microsoft.Extensions.DependencyInjection;

namespace RedisWrapper.Api
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1",
                    new Swashbuckle.AspNetCore.Swagger.Info
                    {
                        Title = "RedisWrapper.API",
                        Version = "v1",
                        Description = "Api responsável por realizar testes com o Redis"
                    });
            });

            return services;
        }
    }
}