namespace DotNet2020.Domain._5.Entities
{
    public class WorkItem
    {
        /// <summary>
        /// Work item id (PK for EF)
        /// </summary>
        public int WorkItemId { get; set; }

        /// <summary>
        /// Issue id (FK for EF)
        /// </summary>
        public int IssueId { get; set; }

        /// <summary>
        /// Spent time (in hours)
        /// </summary>
        public int SpentTime { get; set; }
        
        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }
    }
}
