using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Azure;
using Azure.Core;

namespace CarvedRockSoftware.Seeder.AzureStorageQueues
{
    public class FakeErrorResponse : Response
    {
        public override int Status => 500;

        public override string ReasonPhrase => "Internal Server Error";

        public override Stream ContentStream
        {
            get
            {
                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                writer.Write("<html></html>");
                writer.Flush();
                stream.Position = 0;
                return stream;
            }
            set { }
        }
        public override string ClientRequestId { get => string.Empty; set { } }

        public override void Dispose()
        {
        }

        protected override bool ContainsHeader(string name)
        {
            return false;
        }

        protected override IEnumerable<HttpHeader> EnumerateHeaders()
        {
            return new HttpHeader[0];
        }

        protected override bool TryGetHeader(string name, [NotNullWhen(true)] out string value)
        {
            value = null;
            return false;
        }

        protected override bool TryGetHeaderValues(string name, [NotNullWhen(true)] out IEnumerable<string> values)
        {
            values = null;
            return false;
        }
    }
}
