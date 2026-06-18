using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Mentalist.Server;
using Mentalist.Shared;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System;

var builder = WebApplication.CreateBuilder(args);
var sessions = new ConcurrentDictionary<string, MentalistEngine>();

var app = builder.Build();

app.MapPost("/game/start", ([FromBody] StartGameRequest request) =>
{
    try
    {
        var id = Guid.NewGuid().ToString();
        var deck = DeckFactory.CreateDeck(request.Mode);
        var engine = new MentalistEngine(deck.Items);
        sessions[id] = engine;

        return Results.Ok(new GameSessionDto
        {
            Id = id,
            CurrentItems = engine.CurrentItems.ToList(),
            CurrentPhrase = engine.CurrentPhrase,
            IsFinished = engine.IsFinished
        });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapPost("/game/{id}/move", (string id, [FromBody] MoveRequest request) =>
{
    if (!sessions.TryGetValue(id, out var engine))
        return Results.NotFound();

    try
    {
        engine.Eliminate(request.ChosenItems);
        return Results.Ok(new GameSessionDto
        {
            Id = id,
            CurrentItems = engine.CurrentItems.ToList(),
            CurrentPhrase = engine.CurrentPhrase,
            IsFinished = engine.IsFinished,
            FinalResult = engine.FinalResult
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.Run();