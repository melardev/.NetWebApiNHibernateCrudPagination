using System;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Event;

namespace WebApiNHibernateCrudPagination.Entities
{
    public class EntitySaveListener : IPreInsertEventListener, IPreUpdateEventListener
    {
        private void HandleTimeStamps(AbstractPreDatabaseOperationEvent @event, bool isCreating)
        {
            var now = DateTime.Now;

            if (@event.Entity is TimestampedEntity entity)
                if (isCreating)
                {
                    if (entity.CreatedAt == null)
                        entity.CreatedAt = now;
                    if (entity.UpdatedAt == null)
                        entity.UpdatedAt = now;
                }
                else
                {
                    entity.UpdatedAt = now;
                }
        }


        public virtual async Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
        {
            HandleTimeStamps(@event, true);
            return false;
        }

        public virtual bool OnPreInsert(PreInsertEvent @event)
        {
            HandleTimeStamps(@event, true);
            return false;
        }

        public virtual async Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
        {
            HandleTimeStamps(@event, false);
            return false;
        }

        public virtual bool OnPreUpdate(PreUpdateEvent @event)
        {
            HandleTimeStamps(@event, false);
            return false;
        }
    }
}