
using System.Threading.Tasks;

namespace TheLastPlanet.Shared
{
	public class ThreadLock
	{
		private volatile TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

		public Task Wait()
		{
			return _tcs.Task;
		}

		public void Unlock()
		{
			_tcs.TrySetResult(true);
		}
	}
}