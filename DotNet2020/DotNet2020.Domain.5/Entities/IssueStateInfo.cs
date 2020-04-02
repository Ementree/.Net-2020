namespace DotNet2020.Domain._5.Entities
{
    public struct IssueStateInfo
    {
        public int Priority { get; private set; }
        public int Type { get; private set; }
        public int State { get; private set; }
        public int BlockingState { get; private set; }

        public IssueStateInfo(int priority, int type, int state, int blockingState)
        {
            Priority = priority;
            Type = type;
            State = state;
            BlockingState = blockingState;
        }
    }
}
