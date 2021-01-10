using System;

namespace Topdev.OpenSubtitles.Client
{
    public class UnknownTypeException : Exception
    {
        public UnknownTypeException(string message) : base(message) { }
    }
}