using System;
using System.Runtime.CompilerServices;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Signifies that it's not a specimen.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="ISpecimenBuilder"/> implementations are expected to return
    /// <see cref="NoSpecimen"/> instances if they can't handle the request. This ensures that
    /// <see langword="null"/> can be used as a proper return value.
    /// </para>
    /// </remarks>
    public class NoSpecimen : IEquatable<NoSpecimen>
    {
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly NoSpecimen Instance = new NoSpecimen();

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpecimen"/> class with a null request.
        /// </summary>
        private NoSpecimen()
        {
        }

        /// <summary>
        /// Indicates whether the current instance is equal to another <see cref="NoSpecimen"/>
        /// instance.
        /// </summary>
        /// <param name="other">
        /// A <see cref="NoSpecimen"/> instance to compare with this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the current instanec is equal to the <paramref name="other"/>
        /// parameter; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(NoSpecimen other)
        {
            if (other == null)
            {
                return false;
            }
        
            return object.Equals(this, other);
        }
    }
}
