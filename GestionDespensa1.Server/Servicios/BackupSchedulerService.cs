namespace GestionDespensa1.Server.Servicios
{
    public class BackupSchedulerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackupSchedulerService> _logger;
        private Timer? _timer;

        public BackupSchedulerService(
            IServiceProvider serviceProvider,
            ILogger<BackupSchedulerService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Programar backup diario a las 3:00 AM
            var now = DateTime.Now;
            var nextRun = now.Date.AddDays(1).AddHours(3); // Mañana a las 3 AM

            if (now > nextRun)
            {
                nextRun = nextRun.AddDays(1);
            }

            var initialDelay = nextRun - now;
            _timer = new Timer(DoWork, null, initialDelay, TimeSpan.FromDays(1));

            _logger.LogInformation($"Backup programado para: {nextRun:dd/MM/yyyy HH:mm:ss}");
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            try
            {
                _logger.LogInformation("Iniciando backup automático...");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var backupService = scope.ServiceProvider.GetRequiredService<BackupService>();
                    var result = await backupService.CrearBackup();

                    if (result.Success)
                    {
                        _logger.LogInformation($"✅ Backup automático completado: {result.FileName} ({result.FileSizeFormatted})");
                    }
                    else
                    {
                        _logger.LogError($"❌ Error en backup automático: {result.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"💥 Error en backup automático: {ex.Message}");
            }
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }
}