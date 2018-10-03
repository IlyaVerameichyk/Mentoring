using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mentoring1
{
    public class Task7
    {
        public static void Run()
        {
            //Console.WriteLine("Success task");
            //AttachToTask(Task.CompletedTask);

            //Console.WriteLine("Faulted task");
            //AttachToTask(Task.Factory.StartNew(() =>
            //{
            //    Console.WriteLine("Run faulted task");
            //    Console.WriteLine($"thread id: {Thread.CurrentThread.ManagedThreadId}");
            //    throw new Exception();
            //}));

            Console.WriteLine("Cancel task");
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            tokenSource.CancelAfter(TimeSpan.FromSeconds(3));
            AttachToTask(Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"thread id: {Thread.CurrentThread.ManagedThreadId}");
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                }
            }, token));
        }

        private static void AttachToTask(Task parent)
        {
            parent.ContinueWith(prev =>
            {
                Console.WriteLine("Regardless task");
            });

            parent.ContinueWith(prev =>
            {
                Console.WriteLine("Task when finished without success");
            }, TaskContinuationOptions.NotOnRanToCompletion);

            parent.ContinueWith(prev =>
            {
                Console.WriteLine($"Parent task failed. Recreate parent id {prev.Id}");
                Console.WriteLine($"thread id: {Thread.CurrentThread.ManagedThreadId}");
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.AttachedToParent);

            parent.ContinueWith(prev =>
            {
                //??
                Console.WriteLine("Continue cancel thread");
                Console.WriteLine($"thread id: {Thread.CurrentThread.ManagedThreadId}");
            },TaskContinuationOptions.DenyChildAttach);
        }
    }
}