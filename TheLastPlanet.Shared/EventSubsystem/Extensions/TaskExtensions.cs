using System.Threading.Tasks;


namespace TheLastPlanet.Shared.Internal.Extensions
{
    
    public static class TaskExtensions
    {
        public static async void InvokeAndForget(this Task task)
        {
            await task;
        }
    }
}