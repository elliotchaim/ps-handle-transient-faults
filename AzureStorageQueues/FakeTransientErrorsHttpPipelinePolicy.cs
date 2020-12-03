using System;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;

namespace CarvedRockSoftware.Seeder.AzureStorageQueues
{
    public class FakeTransientErrorsHttpPipelinePolicy : HttpPipelinePolicy
    {
        private int _count = 0;

        public override void Process(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {
            ProcessNext(message, pipeline);
        }

        public override async ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {
            _count += 1;

            await ProcessNextAsync(message, pipeline);

            if (_count > 3 && new Random().Next(3) == 0)
            {
                message.Response = new FakeErrorResponse();
            }
        }
    }
}
