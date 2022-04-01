namespace MM.Api;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using MM.Api.Status;

public class Client
{
	private readonly ISettings _settings;
	private readonly HttpClient _client;
	private string _token;
	private string? _userId;

	public Client(ISettings settings)
	{
		_settings = settings;
		_client = new HttpClient() { BaseAddress = new Uri($"https://{settings.ApiUrl}/api/v4/") };
	}
	public async Task Login()
	{
		var result = await _client.PostAsJsonAsync("users/login", _settings);

		if (result.IsSuccessStatusCode)
		{
			var response = await result.Content.ReadFromJsonAsync<LoginResponse>();
			var token = result.Headers.FirstOrDefault(x => x.Key == "Token").Value;
			_token = token.FirstOrDefault() ?? string.Empty;
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			_userId = response?.UserId;
		}
	}
	public async Task<UserStatus> GetStatus()
	{
		var result = await _client.GetAsync($"users/{_userId}/status");
		if (result.IsSuccessStatusCode)
		{
			var response = await result.Content.ReadFromJsonAsync<StatusResponse>();
			return response.Status;
		}
		return UserStatus.Unknown;
	}

	public async Task<UserStatus> UpdateUserStatus(UserStatus newStatus, int dndEndTime = 0)
	{
		return await UpdateUserStatus(_userId, newStatus, dndEndTime);
	}
	public async Task<UserStatus> UpdateUserStatus(string userId, UserStatus newStatus, int dndEndTime = 0)
	{
		var request = new UpdateUserStatusPayload()
		{
			UserId = userId,
			Status = newStatus,
			DndEndTime = dndEndTime
		};
		var result = await _client.PutAsJsonAsync($"users/{userId}/status", request);
		if (result.IsSuccessStatusCode)
		{
			var response = await result.Content.ReadFromJsonAsync<StatusResponse>();
			return response.Status;
		}
		return UserStatus.Unknown;
	}
}
