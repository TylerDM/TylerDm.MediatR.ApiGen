using TylerDM.MediatR.ApiGen;
using TylerDM.MediatR.ApiGen.Test;

namespace TylerDm.MediatR.ApiGen.Tests;

public class Tests
{
	[Fact]
	public async Task TestAsync()
	{
		var factory = new WebApplicationFactory<Program>();
		var client = factory.CreateClient();

		var request = new TestRequest("Test");
		var expected = await new TestRequestHandler().Handle(request, default);

		var httpResponse = await client.PostAsJsonAsync(TestRequestHandler.Route, request);
		httpResponse.EnsureSuccessStatusCode();
		var responseJson = await httpResponse.Content.ReadAsStringAsync();
		var response = JsonSerializer.Deserialize<TestResponse>(responseJson);

		Assert.Equal(expected.message, response?.message);
	}
}
