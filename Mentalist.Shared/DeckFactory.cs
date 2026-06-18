using System;
using System.Collections.Generic;
using System.Linq;

namespace Mentalist.Shared
{
    public interface IDeck
    {
        string Name { get; }
        List<string> Items { get; }
    }

    public class NumberDeck : IDeck
    {
        public string Name => "Числовая колода";
        public List<string> Items { get; }

        public NumberDeck()
        {
            Items = Enumerable.Range(1, 16).Select(i => i.ToString()).ToList();
        }
    }

    public class CardDeck : IDeck
    {
        public string Name => "Колода карт";
        public List<string> Items { get; }

        public CardDeck()
        {
            string[] suits = { "♠", "♥", "♦", "♣" };
            string[] ranks = { "7", "8", "9", "10" };
            Items = new List<string>();
            foreach (var s in suits)
            {
                foreach (var r in ranks)
                {
                    Items.Add($"{r}{s}");
                }
            }
        }
    }

    public class WordsDeck : IDeck
    {
        public string Name => "Словесная колода";
        public List<string> Items { get; }

        public WordsDeck()
        {
            Items = new List<string> { "Яблоко", "Книга", "Экран", "Код", "Свет", "Дом", "Путь", "Мир", "День", "Ночь", "Игра", "Сон", "Звук", "Цвет", "Век", "Дух" };
        }
    }

    public static class DeckFactory
    {
        public static IDeck CreateDeck(string type)
        {
            return type.ToLower() switch
            {
                "numbers" => new NumberDeck(),
                "cards" => new CardDeck(),
                "words" => new WordsDeck(),
                _ => throw new ArgumentException("Неизвестный тип колоды", nameof(type))
            };
        }
    }
}