namespace TestTask.Tools
{
    public class LogableObject
    {
        public Logger Logger { get; private set; }

        public LogableObject()
        {
            Logger = Logger.Get(GetType());
        }
    }
}
