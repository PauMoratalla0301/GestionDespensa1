using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;

namespace GestionDespensa1.Server.Servicios
{
    public class BackupService
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public BackupService(Context context, IWebHostEnvironment env, IConfiguration config)
        {
            _context = context;
            _env = env;
            _config = config;
        }

        public async Task<BackupInfoDTO> CrearBackup()
        {
            try
            {
                string backupFolder = Path.Combine(_env.ContentRootPath, "Backups");

                // Crear carpeta si no existe
                if (!Directory.Exists(backupFolder))
                {
                    Directory.CreateDirectory(backupFolder);
                }

                string fileName = $"Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
                string filePath = Path.Combine(backupFolder, fileName);

                // Obtener cadena de conexión
                string connectionString = _config.GetConnectionString("DefaultConnection");

                // Extraer nombre de la base de datos
                var builder = new SqlConnectionStringBuilder(connectionString);
                string dbName = builder.InitialCatalog;

                // Comando SQL para backup
                string sql = $@"
                    BACKUP DATABASE [{dbName}] 
                    TO DISK = '{filePath}' 
                    WITH FORMAT, 
                    MEDIANAME = 'SQLServerBackups', 
                    NAME = 'Full Backup of {dbName}';
                ";

                // Ejecutar backup
                await _context.Database.ExecuteSqlRawAsync(sql);

                // Limpiar backups antiguos (mantener solo últimos 7)
                await LimpiarBackupsAntiguos(backupFolder);

                return new BackupInfoDTO
                {
                    Success = true,
                    FileName = fileName,
                    FilePath = filePath,
                    FileSize = new FileInfo(filePath).Length,
                    CreationDate = DateTime.Now,
                    Message = "Backup creado exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new BackupInfoDTO
                {
                    Success = false,
                    Message = $"Error al crear backup: {ex.Message}"
                };
            }
        }

        public async Task<List<BackupInfoDTO>> ObtenerBackups()
        {
            string backupFolder = Path.Combine(_env.ContentRootPath, "Backups");

            if (!Directory.Exists(backupFolder))
            {
                return new List<BackupInfoDTO>();
            }

            var backups = Directory.GetFiles(backupFolder, "*.bak")
                .Select(f => new BackupInfoDTO
                {
                    FileName = Path.GetFileName(f),
                    FilePath = f,
                    FileSize = new FileInfo(f).Length,
                    CreationDate = new FileInfo(f).CreationTime,
                    Success = true
                })
                .OrderByDescending(b => b.CreationDate)
                .ToList();

            return backups;
        }

        public async Task<byte[]> DescargarBackup(string fileName)
        {
            string backupFolder = Path.Combine(_env.ContentRootPath, "Backups");
            string filePath = Path.Combine(backupFolder, fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("El archivo de backup no existe");
            }

            return await File.ReadAllBytesAsync(filePath);
        }

        public async Task<bool> RestaurarBackup(string fileName)
        {
            try
            {
                string backupFolder = Path.Combine(_env.ContentRootPath, "Backups");
                string filePath = Path.Combine(backupFolder, fileName);

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("El archivo de backup no existe");
                }

                string connectionString = _config.GetConnectionString("DefaultConnection");
                var builder = new SqlConnectionStringBuilder(connectionString);
                string dbName = builder.InitialCatalog;

                // Cambiar a single user y restaurar
                string sql = $@"
                    USE master;
                    ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    RESTORE DATABASE [{dbName}] FROM DISK = '{filePath}' WITH REPLACE;
                    ALTER DATABASE [{dbName}] SET MULTI_USER;
                ";

                await _context.Database.ExecuteSqlRawAsync(sql);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al restaurar: {ex.Message}");
                return false;
            }
        }

        private async Task LimpiarBackupsAntiguos(string backupFolder)
        {
            try
            {
                var backups = Directory.GetFiles(backupFolder, "*.bak")
                    .Select(f => new FileInfo(f))
                    .OrderByDescending(f => f.CreationTime)
                    .Skip(7) // Mantener solo los últimos 7
                    .ToList();

                foreach (var backup in backups)
                {
                    File.Delete(backup.FullName);
                    Console.WriteLine($"🗑️ Backup antiguo eliminado: {backup.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al limpiar backups antiguos: {ex.Message}");
            }
        }
    }

    public class BackupInfoDTO
    {
        public bool Success { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public long FileSize { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Message { get; set; }
        public string FileSizeFormatted => FormatearTamaño(FileSize);

        private string FormatearTamaño(long bytes)
        {
            string[] sufijos = { "B", "KB", "MB", "GB", "TB" };
            int contador = 0;
            decimal numero = bytes;

            while (Math.Round(numero / 1024) >= 1)
            {
                numero /= 1024;
                contador++;
            }

            return $"{numero:n1} {sufijos[contador]}";
        }
    }
}