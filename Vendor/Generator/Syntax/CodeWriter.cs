using System;
using System.Text;

namespace TheLastPlanet.Generators.Syntax
{
    public class CodeWriter
    {
        public int Scope;

        private readonly StringBuilder _content;
        private int _indentation;
        private readonly ScopeTracker _tracker;

        public CodeWriter()
        {
            _content = new StringBuilder();
            _tracker = new ScopeTracker(this, null);
        }

        public ScopeTracker Encapsulate()
        {
            return new(this, Scope);
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

        public IDisposable BeginScope(string line)
        {
            AppendLine(line);

            return BeginScope();
        }

        public IDisposable BeginScope()
        {
            Open(false);

            return _tracker;
        }

        public override string ToString() => _content.ToString();
    }

    public class ScopeTracker : IDisposable
    {
        private int? Scope { get; }
        private CodeWriter Parent { get; }
        private int _references;

        public bool HasReferences => _references > 0;

        public ScopeTracker(CodeWriter parent, int? scope)
        {
            Parent = parent;
            Scope = scope;
        }

        public ScopeTracker Reference()
        {
            // Is scopeable tracker?
            if (Scope != null)
                _references++;

            return this;
        }

        public void Dispose()
        {
            if (HasReferences)
            {
                _references--;

                return;
            }
            
            if (Scope.HasValue)
            {
                while (Parent.Scope > Scope)
                {
                    Parent.Close();
                    Parent.Scope--;
                }
            }
            else
            {
                Parent.Close();
            }
        }
    }
}