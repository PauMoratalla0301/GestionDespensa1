using System;

namespace GestionDespensa1.Shared.DTO
{
    public class BackupInfoDTO
    {
        public bool Success { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public long FileSize { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Message { get; set; }

        public string FileSizeFormatted
        {
            get
            {
                string[] sufijos = { "B", "KB", "MB", "GB", "TB" };
                int contador = 0;
                decimal numero = FileSize;

                while (Math.Round(numero / 1024) >= 1)
                {
                    numero /= 1024;
                    contador++;
                }

                return $"{numero:n1} {sufijos[contador]}";
            }
        }
    }
}