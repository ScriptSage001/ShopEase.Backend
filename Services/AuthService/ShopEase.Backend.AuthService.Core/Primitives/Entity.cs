namespace ShopEase.Backend.AuthService.Core.Primitives
{
    /// <summary>
    /// Abstract Entity Class
    /// </summary>
    public abstract class Entity : IEquatable<Entity>
    {
        #region Properties

        /// <summary>
        /// Entity Identifier
        /// </summary>
        public Guid Id { get; private init; }

        #endregion

        #region Constructor

        /// <summary>
        /// Protected Constructor For Entity
        /// </summary>
        /// <param name="id"></param>
        protected Entity(Guid id)
        {
            Id = id;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Custom == operator
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator ==(Entity? first, Entity? second)
        {
            return first is not null && second is not null && first.Equals(second);
        }

        /// <summary>
        /// Custom != operator
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator !=(Entity? first, Entity? second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Overridden Equals for Entity type
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Entity? other)
        {
            if (other is null) return false;

            if (other.GetType() != GetType()) return false;

            return other.Id == Id;
        }

        /// <summary>
        /// Overridden Equals for Object type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object? obj)
        {
            if (obj is null) return false;

            if (obj.GetType() != GetType()) return false;

            if (obj is not Entity entity) return false;

            return entity.Id == Id;
        }

        /// <summary>
        /// Overridden GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode() * 11;  
        }

        #endregion
    }
}
