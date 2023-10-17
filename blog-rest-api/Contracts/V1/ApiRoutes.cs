namespace blog_rest_api.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = $"{Root}/{Version}";

        public static class Blogs
        {
            public const string GetAll = Base + "/blogs";
            public const string Get = Base + "/blog/{blogId}";
            public const string Create = Base + "/blogs";
        }
    }
}
