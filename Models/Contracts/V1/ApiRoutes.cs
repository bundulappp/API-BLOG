namespace Models.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = $"{Root}/{Version}";

        public static class Blogs
        {
            public const string GetAll = Base + "/blog";
            public const string Get = Base + "/blog/{blogId}";
            public const string Create = Base + "/blog";
            public const string Update = Base + "/blog/{blogId}";
            public const string Delete = Base + "/blog/{blogId}";
            public const string GetAllCommentForBlog = Base + "/blog/{blogId}/comment";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
            public const string Refresh = Base + "/identity/refresh";

        }

        public static class Tags
        {
            public const string GetAll = Base + "/tag";
            public const string Create = Base + "/tag";
            public const string Delete = Base + "/tag/{tagId}";
        }

        public static class Comments
        {
            public const string Get = Base + "/comment/{commentId}";
            public const string Create = Base + "/comment";
            public const string Reply = Base + "/comment-reply";
            public const string Update = Base + "/comment/{commentId}";
            public const string Delete = Base + "/comment/{commentId}";

        }
    }
}
