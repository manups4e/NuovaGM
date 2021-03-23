using System;
using System.Linq;
using System.Text;

using TheLastPlanet.Shared.Internal.Extensions;

namespace TheLastPlanet.Shared.Internal.Diagnostics
{
    
    public abstract class BaseLogger
    {
        private string _source;

        protected BaseLogger()
        {
        }

        protected BaseLogger(string source)
        {
            _source = source;
        }

        protected abstract void Write(Severity severity, string value);

        public void Info(params object[] values)
        {
            Write(Severity.Info, Format(GetSeverityName(Severity.Info), values ?? new object[] { null }));
        }

        public void Debug(params object[] values)
        {
            Write(Severity.Debug, Format(GetSeverityName(Severity.Debug), values ?? new object[] { null }));
        }

        public void Error(string message, Exception exception = null)
        {
            var builder = new StringBuilder(message ?? string.Empty);

            if (builder.Length > 0 && exception != null)
                builder.Append(": ");

            if (exception != null)
                builder.Append(exception.FlattenException());

            Write(Severity.Error, Format(GetSeverityName(Severity.Error), builder.ToString()));
        }

        private string GetSeverityName(Severity severity)
        {
            return severity switch
            {
                Severity.Info => "^9Info",
                Severity.Debug => "^3Debug",
                Severity.Error => "^1Error",
                _ => null
            };
        }

        private string Format(string severity, object[] values) => Format(severity, string.Join(", ", values.Select(self => self?.ToString() ?? "null")));

        private string Format(string severity, string message) =>
            $"[^9{DateTime.UtcNow:yyyy'/'MM'/'dd HH:mm:ss'.'fff}] [{severity}] {(!string.IsNullOrEmpty(_source) ? $"[^5{_source}] " : string.Empty)}{message}";
    }
}