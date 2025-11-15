using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using frontend.Proxy;
using Godot;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nordtap.GameManager.Extension;
using Nordtap.Hub.Proxy;
using Shared.Data.Entities;
using Shared.Hubs.Interfaces;

namespace Nordtap.Hub;

public partial class ChatHub : Label, IChatHubClient
{
    private HubConnection? hubConnection;
    private IChatHub hubProxy;
    private List<ChatMessage> _messages = [];
    private string? messageInput;
    private string? hubGroup;
    private string password = string.Empty;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public override void _Process(double delta)
    {
        if (_messages.Count > 0)
        {
            Text = string.Join('\n', _messages);
        }
        
        base._Process(delta);
    }

    public override void _Ready()
    {
        Task.Run(async () =>
        {
            await JoinGroup();
        }, _cancellationTokenSource.Token);
        
        base._Ready();
    }

    public override void _EnterTree()
    {
        //Console.WriteLine("Enter tree");
        var serviceProvider = this.GetServiceProvider();
        var logger = serviceProvider.GetRequiredService<ILogger<ChatHub>>();
        
        logger.LogInformation("Entering ChatHub");
        
        Task.Run(async () =>
            {
                hubConnection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5296/ChatHub", (opt) =>
                    { 
                        //opt.HttpMessageHandlerFactory = _ => CookieDelegatingHandler;
                    })
                    .Build();
                hubProxy = new ChatHubServerProxy(hubConnection);
                hubConnection.RegisterClient(this);
                await hubConnection.StartAsync();
            }, 
            _cancellationTokenSource.Token);
        
        base._EnterTree();
    }

    public Task MessagesLoaded(List<ChatMessage> messages)
    {
        _messages = messages;
        return Task.CompletedTask;
    }

    public Task MessageDeleted(Guid id)
    {
        var hubMessages = _messages.Where(msg => msg.Id != id);
        _messages = hubMessages.ToList();
        return Task.CompletedTask;
    }

    public Task MessagesDeleted(List<ChatMessage> messages)
    {
        var hubMessages = _messages.Where(msg => messages.Contains(msg) == false);
        _messages = hubMessages.ToList();
        return Task.CompletedTask;
    }
    
    public Task ChatMessageReceived(ChatMessage message)
    {
        _messages.Add(message);
        return Task.CompletedTask;
    }
    
    private async Task JoinGroup()
    {
        if (hubGroup is null)
        {
            return;
        }
        
        var addToGroupResult = await hubProxy.AddToGroup("test", "test");
        string status = String.Empty;
        if (addToGroupResult.IsSuccess == false)
        {
            Text += addToGroupResult.Error;
            Console.WriteLine(addToGroupResult.Error);
            return;
        }

        Text += $"Joined room: {hubGroup}";
    }
}