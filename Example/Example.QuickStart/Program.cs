﻿using NanoRabbit.NanoRabbit;
using System.Text;

var pool = new RabbitPool();
pool.RegisterConnection("Connection1", new ConnectOptions
{
    HostName = "localhost",
    Port = 5672,
    UserName = "admin",
    Password = "admin",
    VirtualHost = "DATA"
});

while (true)
{
    pool.Send("Connection1", "BASIC.TOPIC", "BASIC.KEY", Encoding.UTF8.GetBytes("Hello from Send()!"));
    pool.Publish<string>("Connection1", "BASIC.TOPIC", "BASIC.KEY", "Hello from Publish<T>()!");

    Console.WriteLine("Sent to RabbitMQ");

    await Task.Delay(1000);
}

//while (true)
//{
//    pool.Receive("Connection1", "BASIC_QUEUE", body =>
//    {
//        Console.WriteLine(Encoding.UTF8.GetString(body));
//    });
//    Task.Delay(1000);
//}