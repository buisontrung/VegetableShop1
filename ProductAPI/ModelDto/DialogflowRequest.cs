
namespace ProductAPI.ModelDto
{

	public class DialogflowResponse
	{
		public string? ResponseId { get; set; }
		public QueryResult? QueryResult { get; set; }
		public WebhookStatus? WebhookStatus { get; set; }
	}
	public class WebhookStatus
	{
		public int Code { get; set; }
		public string? Message { get; set; }
	}
	public class DialogflowRequest
	{
		
	public string? ResponseId { get; set; }
		public QueryResult? QueryResult { get; set; }
		public OriginalDetectIntentRequest? OriginalDetectIntentRequest { get; set; }
		public string? Session { get; set; }
	}

	public class QueryResult
	{
		public string? QueryText { get; set; }
		public Dictionary<string, object>? Parameters { get; set; }
		public bool? AllRequiredParamsPresent { get; set; }
		public List<OutputContext>? OutputContexts { get; set; }
		public Intent? Intent { get; set; }
		public float? IntentDetectionConfidence { get; set; }
		public string? LanguageCode { get; set; }
	}
	public class Payload
	{
		public string? token { get; set; }
		// Các thuộc tính khác nếu cần
	}
	public class OutputContext
	{
		public string? Name { get; set; }
		public Dictionary<string, object>? Parameters { get; set; }
	}

	public class Intent
	{
		public string? Name { get; set; }
		public string? DisplayName { get; set; }
	}

	public class OriginalDetectIntentRequest
	{
		public string? Source { get; set; }
		public Dictionary<string, object>? Payload { get; set; }
	}

}
