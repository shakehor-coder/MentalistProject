namespace Mentalist.Shared;

/// <summary>
/// DTO для запроса начала новой игры.
/// </summary>
public class StartGameRequest
{
    public string Mode { get; set; } = string.Empty;
}