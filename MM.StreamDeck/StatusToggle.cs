using System;
using System.Drawing;
using System.Threading.Tasks;
using BarRaider.SdTools;
using MM.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MM.Api.Status;

namespace MM.StreamDeck;

[PluginActionId("com.mongeon.mm.statustoggle")]
public class StatusToggle : PluginBase
{
	private class PluginSettings
	{
		public static PluginSettings CreateDefaultSettings()
		{
			var instance = new PluginSettings
			{
				BackgroundColor = "#000000",
				BackgroundImage = null,
				RefreshSeconds = 60,
				Username = string.Empty,
				Password = string.Empty,
				ApiUrl = string.Empty,
				IsToggle = false,
			};
			return instance;
		}

		[JsonProperty(PropertyName = "backgroundColor")]
		public string BackgroundColor { get; set; }

		[FilenameProperty]
		[JsonProperty(PropertyName = "backgroundImage")]
		public string BackgroundImage { get; set; }
		[JsonProperty(PropertyName = "refreshSeconds")]
		public int RefreshSeconds { get; set; }
		[JsonProperty(PropertyName = "username")]
		public string Username { get; set; }
		[JsonProperty(PropertyName = "password")]
		public string Password { get; set; }
		[JsonProperty(PropertyName = "apiUrl")]
		public string ApiUrl { get; set; }
		[JsonProperty(PropertyName = "isToggle")]
		public bool IsToggle { get; set; }
		[JsonProperty(PropertyName = "toggleStatus")]
		public UserStatus ToggleStatus { get; set; }

		[JsonProperty(PropertyName = "dndDuration")]
		public int DndDuration { get; set; }
	}

	#region Private Members

	private readonly PluginSettings settings;
	private DateTime lastRefresh;

	#endregion
	public StatusToggle(SDConnection connection, InitialPayload payload) : base(connection, payload)
	{
		if (payload.Settings == null || payload.Settings.Count == 0)
		{
			this.settings = PluginSettings.CreateDefaultSettings();
			SaveSettings();
		}
		else
		{
			this.settings = payload.Settings.ToObject<PluginSettings>();
		}
	}

	public override void Dispose()
	{
		Logger.Instance.LogMessage(TracingLevel.INFO, "Destructor called");
	}

	public override async void KeyPressed(KeyPayload payload)
	{
		Logger.Instance.LogMessage(TracingLevel.INFO, "Key Pressed");

		if (settings.IsToggle)
		{
			var status = await ToggleStatus();
			await DrawStatus(status);
		}
		else
		{
			await DrawStatus(await GetStatus());
		}

	}
	private async Task<UserStatus> ToggleStatus()
	{
		var status = await GetStatus();
		var newStatus = UserStatus.Online;
		if (status == UserStatus.Online)
		{
			newStatus = settings.ToggleStatus;
		}
		var client = await GetAuthenticatedClient();
		return await client.UpdateUserStatus(newStatus, settings.DndDuration);
	}

	private async Task<UserStatus> GetStatus()
	{
		var client = await GetAuthenticatedClient();
		return await client.GetStatus();
	}

	private async Task<Client> GetAuthenticatedClient()
	{
		var client = new Client(new Settings
		{
			ApiUrl = settings.ApiUrl,
			Login = settings.Username,
			Password = settings.Password,
		});

		await client.Login();
		return client;
	}

	public override void KeyReleased(KeyPayload payload) { }

	public override async void OnTick()
	{
		if ((DateTime.Now - lastRefresh).TotalSeconds >= settings.RefreshSeconds) // Delay added if market is closed to preserve API calls
		{
			try
			{
				lastRefresh = DateTime.Now;
				await DrawStatus(await GetStatus());
			}
			catch (Exception ex)
			{
				Logger.Instance.LogMessage(TracingLevel.ERROR, $"{this.GetType()} OnTick Exception: {ex}");
			}
		}
	}

	public override void ReceivedSettings(ReceivedSettingsPayload payload)
	{
		Tools.AutoPopulateSettings(settings, payload.Settings);
		SaveSettings();
	}

	public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

	#region Private Methods

	private Task SaveSettings()
	{
		return Connection.SetSettingsAsync(JObject.FromObject(settings));
	}

	private async Task DrawStatus(UserStatus status)
	{
		const int STARTING_TEXT_Y = 5;
		using Bitmap bmp = Tools.GenerateGenericKeyImage(out Graphics graphics);

		int height = bmp.Height;
		int width = bmp.Width;
		Brush stockBrush = Brushes.Blue;
		string statusText = "Unknown";
		string imagePath = "unknown";
		Font fontStock = new Font("Verdana", 22, FontStyle.Bold, GraphicsUnit.Pixel);

		float stringHeight = STARTING_TEXT_Y;

		switch (status)
		{
			case UserStatus.Online:
				statusText = "Online";
				stockBrush = Brushes.Green;
				imagePath = "online";
				break;
			case UserStatus.Away:
				statusText = "Away";
				stockBrush = Brushes.Goldenrod;
				imagePath = "away";
				break;
			case UserStatus.DoNotDisturb:
				statusText = "Do not disturb";
				stockBrush = Brushes.DarkRed;
				imagePath = "dnd";
				break;
			case UserStatus.Offline:
				statusText = "Offline";
				stockBrush = Brushes.White;
				imagePath = "offline";
				break;
		}

		float stringWidth = graphics.GetTextCenter(statusText, width, fontStock);
		stringHeight = graphics.DrawAndMeasureString(statusText, fontStock, stockBrush, new PointF(stringWidth, stringHeight));
		imagePath += (height > 120 - stringHeight) ? "@2x" : "";

		Image image = Image.FromFile(@$"images\{imagePath}.png");
		graphics.DrawImage(image, (width - image.Width) / 2, stringHeight);
		await Connection.SetImageAsync(bmp);
		fontStock.Dispose();
	}
	#endregion
}
