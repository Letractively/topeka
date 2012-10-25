using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Topeka
{
    /// <summary>
    /// This class represents an Assembly with embedded resources that the server will load from
    /// </summary>
    class ResourceAssembly
    {
        internal Assembly assembly;
        internal string assembly_namespace;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="assembly">The Assembly to load resources from</param>
        /// <param name="assembly_namespace">The Namespace from the assembly</param>
        public ResourceAssembly(Assembly assembly, string assembly_namespace)
        {
            this.assembly = assembly;
            this.assembly_namespace = assembly_namespace;
        }
    }
}
