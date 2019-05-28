﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Teronis.Data
{
    public class ParentsContainer
    {
        public ParentCollection Parents { get; private set; }
        public Type WantedType { get; private set; }

        /// <summary>
        /// If <paramref name="wantedType"/> is null, then all parents may attach themselves.
        /// </summary>
        /// <param name="wantedType"></param>
        public ParentsContainer(Type wantedType)
        {
            Parents = new ParentCollection();
            WantedType = wantedType;
        }

        /// <summary>
        /// The <paramref name="parent"/> will be added to the <see cref="Parents"/>, 
        /// if the type of <paramref name="parent"/> the same type of 
        /// <see cref="WantedType"/> or if none <see cref="WantedType"/> is provided.
        /// </summary>
        public void AddParent(object parent)
        {
            parent = parent ?? throw new ArgumentNullException(nameof(parent));

            if (WantedType == null || WantedType.IsAssignableFrom(parent.GetType()))
                Parents.Add(parent);
            else
                throw new InvalidOperationException("Parent is not of type " + WantedType.ToString());
        }

        public class ParentCollection : Collection<object> { }
    }
}
