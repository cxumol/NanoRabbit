﻿using Microsoft.Extensions.Logging;
using NanoRabbit.Connection;
using NanoRabbit.Consumer;

namespace Example.Autofac
{
    public class FooFirstQueueConsumer : RabbitConsumer<string>
    {
        private readonly ILogger _logger;

        public FooFirstQueueConsumer(string connectionName, string consumerName, IRabbitPool pool, ILogger<RabbitConsumer<string>> logger) : base(connectionName, consumerName, pool, logger)
        {
            _logger = logger;
        }

        public override void MessageHandler(object message)
        {
            _logger.LogInformation($"ConsumerLogging: Receive: {message}");
        }
    }
}