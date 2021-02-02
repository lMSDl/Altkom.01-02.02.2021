using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck.Components
{
    public class RandomHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var minute = DateTime.UtcNow.Minute;

            if(minute % 2 == 0) {
                return Task.FromResult(HealthCheckResult.Healthy("I.m OK!", new Dictionary<string, object>() {{"key", 1}, {"key2", "value"}}));
            }

            if(minute % 3 == 0) {
                return Task.FromResult(HealthCheckResult.Degraded());
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("DEAD"));
        }
    }
}