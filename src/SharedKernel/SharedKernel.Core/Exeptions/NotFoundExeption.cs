namespace SharedKernel.Exeptions
{
    public class NotFoundException : Exception
    {
        public string EntityName { get; }
        public string Key { get; }

        public NotFoundException(string entityName, string key) : base($"Entity \"{entityName}\" with ID ({key}) was not found.")
        {
            EntityName = entityName;
            Key = key;
        }
    }

}
