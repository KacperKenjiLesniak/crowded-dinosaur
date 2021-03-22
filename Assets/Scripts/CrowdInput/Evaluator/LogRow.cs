namespace DefaultNamespace.Evaluator
{
    public struct LogRow<T>
    { 
        public T log;
        public float timestamp;

        public LogRow(T log, float timestamp)
        {
            this.log = log;
            this.timestamp = timestamp;
        }
    }
}