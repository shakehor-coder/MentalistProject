namespace Mentalist.Shared;

/// <summary>
/// DTO для передачи хода пользователя серверу.
/// </summary>
public class MoveRequest
{
    public string SessionId { get; set; } = string.Empty;
    public List<string> ChosenItems { get; set; } = new();
}