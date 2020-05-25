using System;
using System.Collections.Generic;
using System.Text;

namespace TeduCoreApp.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Call save change db context
        /// </summary>
        void Commit();
    }
}
