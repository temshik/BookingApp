﻿using Application.Interfaces.IServices;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Handlers;

namespace TelegramBot.Services;

public class HandleUpdateService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IAppUserService _userService;
    private readonly IStateService _stateService;

    public HandleUpdateService(
        ITelegramBotClient botClient, 
        IAppUserService userService, 
        IStateService stateService)
    {
        _botClient = botClient;
        _userService = userService;
        _stateService = stateService;
    }

    public async Task Handle(Update update)
    {
        
        if (update.Message != null)
        {
            var user = await _userService.GetByTelegramIdAsync(update.Message.From.Id);
            if (user == null)
            {
                await _botClient.SendTextMessageAsync(update.Message.From.Id, "You are not authorized.");
                return;
            }
        }

        switch (update.Type)
        {
            case UpdateType.Message:
                await MessageHandler.HandleAsync(update.Message, _botClient, _userService, _stateService);
                return;
        }
    }
}

