using System;

namespace Topdev.OpenSubtitles.Client
{
    public class RpcException : Exception
    {
        public RpcException(string message) : base(message) { }
    }
}