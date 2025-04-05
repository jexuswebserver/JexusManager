using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;

namespace JexusManager
{
    public class TextBoxLoggerProvider : ILoggerProvider
    {
        private readonly RichTextBox _textBox;
        private readonly ConcurrentQueue<(string text, Color color)> _pendingMessages = new ConcurrentQueue<(string, Color)>();
        private readonly object _lock = new object();
        private bool _isInitialized;

        public TextBoxLoggerProvider(RichTextBox textBox)
        {
            _textBox = textBox;
            _textBox.HandleCreated += (s, e) => Initialize();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new TextBoxLogger(this, categoryName);
        }

        public void Dispose()
        {
            // No resources to dispose
        }

        internal void LogMessage(string message, Color color)
        {
            if (!_isInitialized)
            {
                _pendingMessages.Enqueue((message + Environment.NewLine, color));
                return;
            }

            AppendTextSafe(message + Environment.NewLine, color);
        }

        private void Initialize()
        {
            lock (_lock)
            {
                if (!_isInitialized)
                {
                    _isInitialized = true;
                    // Process any pending messages
                    while (_pendingMessages.TryDequeue(out var message))
                    {
                        AppendTextSafe(message.text, message.color);
                    }
                }
            }
        }

        private void AppendTextSafe(string text, Color color)
        {
            if (_textBox.IsDisposed)
                return;

            if (_textBox.InvokeRequired)
            {
                try
                {
                    _textBox.Invoke(new Action(() => AppendTextSafe(text, color)));
                }
                catch (ObjectDisposedException)
                {
                    // Form might be closing
                }
                return;
            }

            int start = _textBox.TextLength;
            _textBox.AppendText(text);
            int end = _textBox.TextLength;

            // Color the new text
            _textBox.Select(start, end - start);
            _textBox.SelectionColor = color;
            _textBox.SelectionLength = 0; // Clear selection

            // Keep last 1000 lines
            while (_textBox.Lines.Length > 1000)
            {
                var index = _textBox.GetFirstCharIndexFromLine(0);
                var length = _textBox.GetFirstCharIndexFromLine(1) - index;
                if (length > 0)
                    _textBox.Text = _textBox.Text.Remove(index, length);
            }

            _textBox.SelectionStart = _textBox.TextLength;
            _textBox.ScrollToCaret();
        }
    }

    public class TextBoxLogger : ILogger
    {
        private readonly TextBoxLoggerProvider _provider;

        public TextBoxLogger(TextBoxLoggerProvider provider, string categoryName)
        {
            _provider = provider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            // Convert log levels to short names and get appropriate colors
            var (levelText, color) = logLevel switch
            {
                LogLevel.Trace => ("trc", Color.Gray),
                LogLevel.Debug => ("dbg", Color.LightGray),
                LogLevel.Information => ("inf", Color.White),
                LogLevel.Warning => ("wrn", Color.Yellow),
                LogLevel.Error => ("err", Color.Red),
                LogLevel.Critical => ("crt", Color.Magenta),
                _ => ("unk", Color.DarkGray)
            };
            
            var message = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{levelText}] {formatter(state, exception)}";
            if (exception != null)
                message += Environment.NewLine + exception;

            _provider.LogMessage(message, color);
        }
    }
}
