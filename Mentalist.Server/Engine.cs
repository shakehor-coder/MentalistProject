using System;
using System.Collections.Generic;
using System.Linq;

namespace Mentalist.Server;

/// <summary>
/// Класс MentalistEngine инкапсулирует логику математического фокуса.
/// </summary>
public class MentalistEngine
{
    private List<string> _items;
    private readonly string _secret;
    private readonly Random _random = new();

    /// <summary>
    /// Возвращает текущий список доступных элементов.
    /// </summary>
    public IReadOnlyList<string> CurrentItems => _items.AsReadOnly();

    /// <summary>
    /// Возвращает текущую реплику ведущего.
    /// </summary>
    public string CurrentPhrase { get; private set; } = "Игра началась: выберите половину элементов.";

    /// <summary>
    /// Флаг завершения игры.
    /// </summary>
    public bool IsFinished => _items.Count == 1;

    /// <summary>
    /// Финальный результат, если игра завершена.
    /// </summary>
    public string? FinalResult => IsFinished ? _items[0] : null;

    /// <summary>
    /// Инициализирует движок игры с заданным набором элементов.
    /// </summary>
    /// <param name="items">Список элементов для игры.</param>
    public MentalistEngine(List<string> items)
    {
        _items = new List<string>(items);
        _secret = _items[_random.Next(_items.Count)];
    }

    /// <summary>
    /// Выполняет ход игрока, удаляя половину элементов согласно правилам фокуса.
    /// </summary>
    /// <param name="chosen">Список элементов, выбранных пользователем.</param>
    public void Eliminate(List<string> chosen)
    {
        if (IsFinished) return;

        if (chosen.Count != _items.Count / 2)
        {
            throw new ArgumentException("Количество выбранных элементов должно составлять ровно половину.");
        }

        if (chosen.Contains(_secret))
        {
            _items = new List<string>(chosen);
            CurrentPhrase = "ваш выбор содержит секрет: удаляем невыбранные элементы.";
        }
        else
        {
            _items = _items.Except(chosen).ToList();
            CurrentPhrase = "ваш выбор не содержит секрет: удаляем выбранные элементы.";
        }

        if (IsFinished)
        {
            CurrentPhrase = $"фокус завершён: загаданное число это {_items[0]}.";
        }
    }
}