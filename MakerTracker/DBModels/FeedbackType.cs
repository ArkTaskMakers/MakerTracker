namespace MakerTracker.DBModels
{
    /// <summary>
    ///     Enuemration representing types of user feedback.
    /// </summary>
    [TypeScriptModel]
    public enum FeedbackType
    {
        Comment,
        Question,
        Problem,
        MissingProduct
    }
}
