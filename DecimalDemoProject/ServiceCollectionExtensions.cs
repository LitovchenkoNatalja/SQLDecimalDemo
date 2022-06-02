namespace DecimalDemoProject
{
    using DecimalDemoProject.DataAccess;
    using DecimalDemoProject.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Register(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDecimalValuesQueryHandler, DecimalValuesQueryHandler>();
            serviceCollection.AddTransient<IDecimalValuesService, DecimalValuesService>();
            serviceCollection.AddTransient<IMathCalculationsQueryHandler, MathCalculationsQueryHandler>();

            return serviceCollection;
        }
    }
}
