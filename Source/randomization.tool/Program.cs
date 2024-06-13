using randomization.tool.Service;
using randomization.tool.Service.interfaces;
using randomization.tool.Services;
using randomization.tool.Services.interfaces;

namespace randomization.tool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            AddServices(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        public static void AddServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRandomizationService, RandomizationService>();
            serviceCollection.AddScoped<IExperimentService, ExperimentService>();
        }
    }
}
