using System;
using System.Text;

namespace TheLastPlanet.Events.Generator.Syntax
{
    public class CodeWriter
    {
        public int Scope;

        private readonly StringBuilder _content;
        private int _unique;
        private int _indentation;
        private readonly ScopeTracker _tracker;

        public CodeWriter()
        {
            _content = new StringBuilder();
            _tracker = new ScopeTracker(_unique, this, null);
        }

        public ScopeTracker Encapsulate()
        {
            _unique++;

            return new ScopeTracker(_unique, this, Scope);
        }

        public void Append(string line) => _content.Append(line);
        public void AppendLine(string line) => _content.Append(new string('\t', _indentation)).AppendLine(line);
        public void AppendLine() => _content.AppendLine();

        public void Open(bool scope = true)
        {
            _content.Append(new string('\t', _indentation)).AppendLine("{");
            _indentation++;

            if (scope)
            {
                Scope++;
            }
        }

        public void Close()
        {
            _indentation--;
            _content.Append(new string('\t', _indentation)).AppendLine("}");
        }

        public IDisposable BeginScope(string line, bool scope = false)
        {
            AppendLine(line);

            return BeginScope(scope);
        }

        public IDisposable BeginScope(bool scope = false)
        {
            Open(scope);

            return _tracker;
        }

        public override string ToString() => _content.ToString();
    }

    public class ScopeTracker : IDisposable
    {
        private readonly CodeWriter _parent;
        private readonly int _unique;
        private int? _recordedScope;
        private int _references;

        public ScopeTracker(int unique, CodeWriter parent, int? scope)
        {
            _unique = unique;
            _parent = parent;
            _recordedScope = scope;
        }

        public ScopeTracker Reference()
        {
            if (_recordedScope != null)
                _references++;

            return this;
        }

        public void Dispose()
        {
            if (_references > 0)
            {
                _references--;

                return;
            }

            if (_recordedScope.HasValue)
            {
                while (_parent.Scope > _recordedScope)
                {
                    _parent.Close();
                    _parent.Scope--;
                }
            }
            else
            {
                _parent.Close();
            }
        }
    }
}