using System.Threading.Tasks;

namespace Gremlins.General
{
    public static class ExceptionGremlin
    {
        public static Task ThrowsNetworkException()
        {

            return Task.CompletedTask;
        }
    }
}
