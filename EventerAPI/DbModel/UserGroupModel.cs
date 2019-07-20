using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace EventerAPI.DbModel
{
    public class UserGroupModel: TableEntity
    {
        public string GroupId { get;set; }
        public string UserId { get;set; }

        public UserGroupModel()
        {

        }

        public UserGroupModel(string userId,string groupId)
        {
            this.RowKey = Guid.NewGuid().ToString();
            this.UserId = userId;
            this.GroupId = groupId;
            this.PartitionKey = userId;
            this.Timestamp = DateTime.Now;
        }
    }
}
