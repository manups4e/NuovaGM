using System;
using System.Reflection;
using System.Text;

using TheLastPlanet.Shared.Internal.Diagnostics;

namespace TheLastPlanet.Shared.Internal.Extensions
{
    
    public static class ExceptionExtensions
    {
        public static ExceptionSnapshot ToSnapshot(this Exception exception)
        { 
            return new ExceptionSnapshot
            {
                Name = exception.GetType().Name,
                Source = Assembly.GetExecutingAssembly().GetName().Name,
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                InnerException = exception.InnerException?.ToSnapshot()
            };
        }
        public static string FlattenException(this Exception exception)
        {
            return exception.ToSnapshot().FlattenException();
        }
        
        public static string FlattenException(this ExceptionSnapshot snapshot)
        {
            var builder = new StringBuilder();

            while (snapshot != null)
            {
                builder.AppendLine(snapshot.Message);
                builder.AppendLine(snapshot.StackTrace);

                snapshot = snapshot.InnerException;
            }

            return builder.ToString();
        }
    }
}