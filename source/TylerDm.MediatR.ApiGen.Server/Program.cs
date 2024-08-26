namespace TylerDM.MediatR.ApiGen;

public partial class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		var services = builder.Services;
		services.AddControllers();
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();
		services.AddMediatR(x =>
		{
			x.Lifetime = ServiceLifetime.Scoped;
			x.RegisterServicesFromAssemblyContaining<Program>();
		});

		var app = builder.Build();

		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();


		app.MapControllers();

		await app.RunAsync();
	}
}
