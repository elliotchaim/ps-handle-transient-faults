using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CarvedRockSoftware.Seeder.AzureSql
{
    public class FakeTransientErrorsInterceptor : DbCommandInterceptor
    {
        private int _count = 0;

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command,
            CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            _count += 1;

            if (_count > 5 && new Random().Next(3) == 0)
            {
                throw new TimeoutException();
            }

            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }
    }
}
