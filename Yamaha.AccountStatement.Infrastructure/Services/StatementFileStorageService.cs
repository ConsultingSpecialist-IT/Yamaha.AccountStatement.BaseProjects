using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yamaha.AccountStatement.Infrastructure.Contracts;

namespace Yamaha.AccountStatement.Infrastructure.Services
{
    public class StatementFileStorageService: IStatementFileStorageService
    {
        public async Task<string> SaveAsync(
            string basePath,
            string period,
            string fileName,
            byte[] content,
            CancellationToken cancellationToken)
        {
            string folderPath = Path.Combine(basePath, period);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fullPath = Path.Combine(folderPath, fileName);

            await File.WriteAllBytesAsync(fullPath, content, cancellationToken);

            return fullPath;
        }
    }
}
