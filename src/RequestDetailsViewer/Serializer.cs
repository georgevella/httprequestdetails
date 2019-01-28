using Newtonsoft.Json;

namespace RequestDetailsViewer
{
	public class Serializer
	{
		internal static JsonSerializer Json = JsonSerializer.Create(new JsonSerializerSettings()
		{
			Formatting = Formatting.Indented,
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore
		});
	}
}