using System;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Threading;
using WebApplication1.IRepository;
using WMS;
using WebApplication1.Controllers;

namespace WebApplication1.web
{

    public class NotificationListenerService : BackgroundService
    {
        private readonly NpgsqlConnection _connection;
        private readonly ILogger<NotificationListenerService> _logger;
        //private readonly ICacheService _cacheService;
        private readonly IServiceProvider _serviceProvider;
        public NotificationListenerService(NpgsqlConnection connection, ILogger<NotificationListenerService> logger,IServiceProvider serviceProvider)//ICacheService cacheService)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
          //  _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        
    }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var cmd = new NpgsqlCommand("LISTEN update_notification", _connection))
            {
                cmd.ExecuteNonQuery();
            }

            _connection.Notification += (sender, e) =>
            {
                _logger.LogInformation("Received notification: {Payload}", e.Payload);
                using (var scope = _serviceProvider.CreateScope())
                {
                    var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();
                    bool removed = cacheService.RemoveData("Employee") as bool? ?? false;
                    if (removed)
                    {
                        _logger.LogInformation("Successfully removed cache key: {Payload}", e.Payload);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to remove cache key: {Payload}", e.Payload);
                    }
                }
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Wait asynchronously for a notification
                    await _connection.WaitAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // The operation was canceled, which is expected on service shutdown
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while waiting for notifications.");
                }

                // Delay before the next iteration
                await Task.Delay(5000, stoppingToken); // 5 seconds delay
            
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
            _connection.Close();
        }
    }

}



