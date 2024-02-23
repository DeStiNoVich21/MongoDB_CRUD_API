namespace MongoDB_CRUD_API.Configuration
{
    public class MongoDbConfiguration
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string EmployeesCollectionName { get; set; } = null!;
        public string UserRolesCollectionName { get; set; } = null!;
        public string UserDepartmentCollectionName { get; set; } = null!;
    }
}
