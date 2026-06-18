namespace Mentalist.Shared;

/// <summary>
/// DTO для передачи состояния сессии между клиентом и сервером.
/// </summary>
public class GameSessionDto
{
    public string Id { get; set; } = string.Empty;
    public List<string> CurrentItems { get; set; } = new();
    public string CurrentPhrase { get; set; } = string.Empty;
    public bool IsFinished { get; set; }
    public string? FinalResult { get; set; }
}