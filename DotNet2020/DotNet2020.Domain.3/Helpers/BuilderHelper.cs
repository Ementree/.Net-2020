namespace DotNet2020.Domain._3.Helpers
{
    public class BuilderHelper
    {
        public BuilderHelper()
        {
            IsRight = false;
            Skip = false;
        }
        public bool Skip { get; set; }
        public int NumberQuestion { get; set; }
        public string Question { get; set; }
        public string Commentary { get; set; }
        public bool IsRight { get; set; }

        public override string ToString()
        {
            if (Commentary == null)
            {
                return $"{Skip.ToString()}>>>{NumberQuestion.ToString()}>>>{Question.ToString()}>>>Нет комментария>>>{IsRight.ToString()}&&&";
            }
            else
            {
                return $"{Skip.ToString()}>>>{NumberQuestion.ToString()}>>>{Question.ToString()}>>>{Commentary.ToString()}>>>{IsRight.ToString()}&&&";
            }
        }
    }
}