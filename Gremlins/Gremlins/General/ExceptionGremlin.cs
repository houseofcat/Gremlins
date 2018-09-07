using Gremlins.Utilities.Implementations;
using System;
using System.Threading.Tasks;
using static Gremlins.Utilities.Helpers.ExceptionHelpers;

namespace Gremlins.General
{
    public static class ExceptionGremlin
    {
        private static ThreadSafeRandomNumberGenerator _random = new ThreadSafeRandomNumberGenerator();

        public static async Task ThrowsSystemException()
        {
            await ThrowsRandomSystemExceptionAsync();
        }

        public static async Task ThrowsNetworkException()
        {
            await ThrowsRandomNetworkExceptionAsync();
        }

        public static async Task ThrowsSqlException()
        {
            await ThrowsRandomSqlExceptionAsync();
        }

        public static async Task ThrowsRandomException()
        {
            var random = new Random();

            switch (random.Next(1, 3))
            {
                case 1: await ThrowsRandomSystemExceptionAsync(); break;
                case 2: await ThrowsRandomNetworkExceptionAsync(); break;
                case 3: await ThrowsRandomSqlExceptionAsync(); break;
                default: break;
            }
        }
    }
}
