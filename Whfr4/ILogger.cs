namespace Whfr4
{
    public interface ILogger
    {
        void Info(string format,params object[] args);
        void Debug(string format, params object[] args);
    }
}
