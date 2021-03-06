﻿using Microphone.Consul;
using Microsoft.Extensions.Configuration;
using System;

namespace Core.ServiceDiscovery
{
    public class ServiceDiscoveryOptions
    {
        public string Type { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public int Heartbeat { get; set; }
        public string HealthCheckPath { get; set; }
        public Type GetImplementation()
            => System.Type.GetType(Type) ;
    }

    internal static class ServiceDiscoveryOptionsExtensions
    {
        internal static ConsulOptions ToConsulOptions(this ServiceDiscoveryOptions options, ConsulNameResolution nameResolution = ConsulNameResolution.HttpApi)
        {
            if (options == null)
                return null;
            return new ConsulOptions
            {
                Host = options.Host,
                Port = options.Port,
                Heartbeat = options.Heartbeat,
                HealthCheckPath = options.HealthCheckPath,
                NameResolution = nameResolution
            };
        }

        internal static ServiceDiscoveryOptions ToServiceDiscoveryOptions(this IConfiguration configuration)
        {
            var section = configuration.GetSection("serviceDiscoveryOptions");

            return new ServiceDiscoveryOptions
            {
                Type = section.GetValue<string>("type") ?? throw new NullReferenceException("Type cannot be null. Check your configuration"),
                Host = section.GetValue<string>("host"),
                Port = section.GetValue<int>("port"),
                HealthCheckPath = section.GetValue<string>("healthCheckPath"),
                Heartbeat = section.GetValue<int>("heartbeat")
            };
        }
    }
}
