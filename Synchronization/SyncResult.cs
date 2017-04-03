
namespace Palmmedia.BackUp.Synchronization
{
    /// <summary>
    /// Enum discribing the state of a <see cref="Palmmedia.BackUp.Synchronization.SyncItems.SyncItemBase"/>.
    /// </summary>
    public enum SyncResult
    {
        /// <summary>
        /// Synchonization was successful.
        /// </summary>
        Successful,

        /// <summary>
        ///  Synchonization has failed.
        /// </summary>
        Error,

        /// <summary>
        ///  Synchonization was not performed yet.
        /// </summary>
        NotSynced
    }
}
