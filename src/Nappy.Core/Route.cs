namespace Nappy.Core
{
    public struct Route
    {
        public HttpVerbs HttpVerb { get; }
        public string Url { get; }

        public Route(HttpVerbs httpVerb, string url)
        {
            HttpVerb = httpVerb;
            Url = url;
        }
    }
}