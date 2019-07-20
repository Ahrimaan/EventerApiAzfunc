using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace EventerAPI.DbModel
{
    public class GroupModel: TableEntity
    {
        public string GroupName { get; set; }
        public string CreatedById { get; set; }
        
        public GroupModel()
        {

        }
        public GroupModel(string groupName,string createdById)
        {
            this.PartitionKey = createdById;
            this.RowKey = Guid.NewGuid().ToString();
            this.GroupName = groupName;
            this.CreatedById = createdById;
        }
    }
}
