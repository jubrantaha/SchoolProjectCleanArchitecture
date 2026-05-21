namespace SchoolProject.Data.AppMetaData
{
    public static class Router
    {
        public const string SingleRoute = "/{id}";
        public const string root = "Api";
        public const string version = "v1";
        public const string Roule = root + "/" + version + "/";

        public static class StudentRouter
        {
            public const string prefix = Roule + "Student";
            public const string List = prefix + "/List";
            public const string GetByID = prefix + SingleRoute;
            public const string Create = prefix + "/Create";
            public const string Edit = prefix + "/Edit";
            public const string Delete = prefix + "/Delete";
            public const string Paginated = prefix + "/Paginated";
        }

        public static class DepartmentRouter
        {
            public const string prefix = Roule + "Department";
            public const string GetByID = prefix + "/Id";
        }

        public static class ApplicationUserRouter
        {
            public const string prefix = Roule + "User";
            public const string Create = prefix + "/Create";
            public const string Paginated = prefix + "/Paginated";
            public const string GetByID = prefix + SingleRoute;
            public const string Edit = prefix + "/Edit";

        }
    }
}
