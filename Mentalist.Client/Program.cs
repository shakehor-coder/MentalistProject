using System.Net.Http.Json;
using Mentalist.Shared;

namespace Mentalist.Client;

/// <summary>
/// Основной класс консольного клиента для взаимодействия с сервером игры.
/// </summary>
public class Program
{
    private static readonly HttpClient _httpClient = new() { BaseAddress = new Uri("http://localhost:5000") };

    /// <summary>
    /// Точка входа в приложение.
    /// </summary>
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Выберите режим игры: 1 - Числа, 2 - Карты, 3 - Слова");
        var input = Console.ReadLine();
        
        string mode = input switch
        {
            "1" => "numbers",
            "2" => "cards",
            "3" => "words",
            _ => "numbers"
        };

        var startResponse = await _httpClient.PostAsJsonAsync("/game/start", new StartGameRequest { Mode = mode });
        var session = await startResponse.Content.ReadFromJsonAsync<GameSessionDto>();

        if (session == null) return;

        while (!session.IsFinished)
        {
            Console.WriteLine($"Текущие элементы: {string.Join(", ", session.CurrentItems)}");
            Console.WriteLine(session.CurrentPhrase);
            Console.WriteLine("Введите элементы для удаления через запятую:");
            var choiceInput = Console.ReadLine() ?? "";
            var chosen = choiceInput.Split(',').Select(s => s.Trim()).ToList();

            var moveResponse = await _httpClient.PostAsJsonAsync($"/game/{session.Id}/move", new MoveRequest { SessionId = session.Id, ChosenItems = chosen });
            session = await moveResponse.Content.ReadFromJsonAsync<GameSessionDto>();
            
            if (session == null) break;
        }

        Console.WriteLine($"Игра окончена. Загаданный секрет: {session?.FinalResult}");
    }
}