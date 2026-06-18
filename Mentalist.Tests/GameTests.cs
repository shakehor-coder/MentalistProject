using Xunit;
using System.Collections.Generic;
using System.Linq;
using Mentalist.Server;

namespace Mentalist.Tests;

/// <summary>
/// Набор тестов для проверки логики игрового движка.
/// </summary>
public class GameTests
{
    private readonly List<string> _defaultItems = new() { "1", "2", "3", "4" };

    /// <summary>
    /// Проверяет, что движок корректно инициализирует состояние игры.
    /// </summary>
    [Fact]
    public void Engine_ShouldInitializeCorrectly()
    {
        var engine = new MentalistEngine(_defaultItems);
        Assert.NotNull(engine.CurrentItems);
        Assert.Equal(4, engine.CurrentItems.Count);
    }

    /// <summary>
    /// Проверяет, что после выполнения хода количество элементов сокращается вдвое.
    /// </summary>
    [Fact]
    public void Engine_ShouldReduceItemsByHalf()
    {
        var engine = new MentalistEngine(_defaultItems);
        var initialCount = engine.CurrentItems.Count;
        var toEliminate = engine.CurrentItems.Take(initialCount / 2).ToList();
        
        engine.Eliminate(toEliminate);
        
        Assert.Equal(initialCount / 2, engine.CurrentItems.Count);
    }

    /// <summary>
    /// Проверяет, что игра завершается при достижении одного элемента.
    /// </summary>
    [Fact]
    public void Engine_ShouldFinishWhenOneItemRemains()
    {
        var engine = new MentalistEngine(_defaultItems);
        while (engine.CurrentItems.Count > 1)
        {
            var toEliminate = engine.CurrentItems.Take(engine.CurrentItems.Count / 2).ToList();
            engine.Eliminate(toEliminate);
        }
        
        Assert.True(engine.IsFinished);
        Assert.NotNull(engine.FinalResult);
    }
}