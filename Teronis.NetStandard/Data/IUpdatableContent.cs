﻿

namespace Teronis.Data
{

    public interface IUpdatableContent<ContentType> : IContentUpdateSequenceStatus, IUpdatableContentBy<ContentType>
    {
        event UpdatingEventHandler<ContentType> ContainerUpdating;
        event UpdatedEventHandler<ContentType> ContainerUpdated;

        bool IsContentUpdatable(IUpdate<ContentType> update);
    }
}
