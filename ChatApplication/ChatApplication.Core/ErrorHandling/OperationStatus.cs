namespace ChatApplication.Core.ErrorHandling
{
    /// <summary>
    /// Indicates final outcome of method execution
    /// </summary>
    public enum OperationStatus
    {
        /// <summary>
        /// Execution was successful
        /// </summary>
        Success,

        /// <summary>
        /// Execution failed due to a database error
        /// </summary>
        DatabaseError,

        /// <summary>
        /// Execution failed due to a file system error
        /// </summary>
        FileSystemError,

        /// <summary>
        /// Execution failed because certain object related to process was not found
        /// </summary>
        NotFound,

        /// <summary>
        /// Execution failed because of the possible creation of duplicates
        /// </summary>
        Exists,

        /// <summary>
        /// Execution failed because data provided in execution process was not valid
        /// </summary>
        InvalidData,

        /// <summary>
        /// Execution failed due to an unknown error
        /// </summary>
        UnknownError,

        /// <summary>
        /// Execution failed because method is not supported
        /// </summary>
        NotSupported
    }
}
