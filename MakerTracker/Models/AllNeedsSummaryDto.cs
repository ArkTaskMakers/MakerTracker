namespace MakerTracker.Models
{
    [TypeScriptModel]
    public class AllNeedsSummaryDto
    {
        public int NeedsMet { get; set; }

        public int OutstandingNeeds { get; set; }
    }
}