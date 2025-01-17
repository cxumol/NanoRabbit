﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NanoRabbit.Connection;
using NanoRabbit.Consumer;

namespace NanoRabbit.DependencyInjection
{
    public static class RabbitConsumerExtensions
    {
        /// <summary>
        /// RabbitConsumer Dependency Injection Method (ConnectionName, ConsumerName)
        /// </summary>
        /// <typeparam name="TConsumer"></typeparam>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="services"></param>
        /// <param name="connectionName"></param>
        /// <param name="consumerName"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsumer<TConsumer, TMessage>(this IServiceCollection services, string connectionName, string consumerName)
            where TConsumer : RabbitConsumer<TMessage>
        {
            services.AddSingleton<TConsumer>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<RabbitConsumer<TMessage>>>();
                var consumer = ActivatorUtilities.CreateInstance<TConsumer>(provider, connectionName, consumerName, provider.GetRequiredService<IRabbitPool>(), logger);
                
                return consumer;
            });
            return services;
        }

        /// <summary>
        /// RabbitConsumer Dependency Injection Method (QueueName)
        /// </summary>
        /// <param name="services"></param>
        /// <param name="queueName"></param>
        /// <typeparam name="TConsumer"></typeparam>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        public static IServiceCollection AddConsumer<TConsumer, TMessage>(this IServiceCollection services,
            string queueName)
            where TConsumer : RabbitConsumer<TMessage>
        {
            services.AddSingleton<TConsumer>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<RabbitConsumer<TMessage>>>();
                var consumer = ActivatorUtilities.CreateInstance<TConsumer>(provider, queueName, provider.GetRequiredService<IRabbitPool>(), logger);
                
                return consumer;
            });
            return services;
        }

        /// <summary>
        /// RabbitConsumer BackgroundService method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TMsg"></typeparam>
        private class RabbitConsumerBackgroundService<T, TMsg> : BackgroundService where T : RabbitConsumer<TMsg>
        {
            private readonly T _consumer;

            public RabbitConsumerBackgroundService(T consumer)
            {
                _consumer = consumer;
            }

            protected override Task ExecuteAsync(CancellationToken stoppingToken)
            {
                return Task.CompletedTask;
            }

            public override Task StartAsync(CancellationToken cancellationToken)
            {
                _consumer.StartConsuming();
                return base.StartAsync(cancellationToken);
            }

            public override async Task StopAsync(CancellationToken cancellationToken)
            {
                await base.StopAsync(cancellationToken);
                _consumer.Dispose();
            }
        }
    }
}
