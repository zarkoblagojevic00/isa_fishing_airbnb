using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Io.Cucumber.Messages;

namespace IntegrationTests
{
    public class Retry<T>
    {
        private readonly T input;

        public Retry(T input)
        {
            this.input = input;
        }

        public TResult Do<TResult>(Func<T, TResult> action, TimeSpan timeout, TimeSpan pollingInterval)
        {
            var exceptions = new List<Exception>();

            var doTimeout = DateTime.Now.Add(timeout);

            while (true)
            {
                try
                {
                    TResult result = action(input);

                    if (result != null)
                        return result;
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }

                if (DateTime.Now < doTimeout)
                {
                    Thread.Sleep(pollingInterval);
                    continue;
                }

                exceptions.Add(new TimeoutException(nameof(action)));
                break;
            }

            throw new AggregateException(exceptions);
        }
    }
}
