using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HexMaster.Threading
{
    internal sealed class StaticThreadTaskScheduler : TaskScheduler, IDisposable
    {
        private BlockingCollection<Task> _tasksCollection;
        private readonly List<Thread> _threadsList;

        public StaticThreadTaskScheduler(int numberOfThreads)
        {
            if (numberOfThreads < 1) throw new ArgumentOutOfRangeException("concurrencyLevel");
            _tasksCollection = new BlockingCollection<Task>();

            _threadsList = Enumerable.Range(0, numberOfThreads).Select(i =>
            {
                var thread = new Thread(() =>
                {
                    foreach (var t in _tasksCollection.GetConsumingEnumerable())
                    {
                        TryExecuteTask(t);
                    }
                });
                thread.IsBackground = true;
                thread.SetApartmentState(ApartmentState.STA);
                return thread;
            }).ToList();

            _threadsList.ForEach(t => t.Start());
        }

        protected override void QueueTask(Task task)
        {
            _tasksCollection.Add(task);
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _tasksCollection.ToArray();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return Thread.CurrentThread.GetApartmentState() == ApartmentState.STA && TryExecuteTask(task);
        }

        public override int MaximumConcurrencyLevel
        {
            get { return _threadsList.Count; }
        }

        public void Dispose()
        {
            if (_tasksCollection != null)
            {
                _tasksCollection.CompleteAdding();

                foreach (var thread in _threadsList) thread.Join();

                _tasksCollection.Dispose();
                _tasksCollection = null;
            }
        }
    }
}
