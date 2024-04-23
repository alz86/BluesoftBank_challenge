namespace Bluesoft.Bank.Model
{
    /// <summary>
    /// Base class for all entities in the system
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        /// <remarks>This property is used for
        /// concurrency checks. While it could be
        /// added as Shadow Property in Entity framework
        /// I preferred to put here just in case it would
        /// be usefull for checks in the future</remarks>
        public byte[] RowVersion { get; set; }
    }

}
