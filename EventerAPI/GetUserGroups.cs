using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using EventerAPI.DbModel;
using System.Collections.Generic;
using EventerAPI.Models;

namespace EventerAPI
{
    /*TODO:
     * If groups > 0 extract groupnames to list
     * if not , return empty list
    */
    public static class GetUserGroups
    {
        [FunctionName("GetUserGroups")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getusergroups/{userId}")] HttpRequest req,
            [Table("UserGroups", "TABLESTORAGE_CONNSTTRING")] CloudTable userGroupTable,
            [Table("Groups", "TABLESTORAGE_CONNSTTRING")] CloudTable groupTable,
            ILogger log,string userId)
        {
            var userGroupResults = userGroupTable.ExecuteQuerySegmentedAsync(new TableQuery<UserGroupModel>(), null);
            var usergroups = userGroupResults.Result.Where(x => x.PartitionKey == userId);
            var groups = groupTable.ExecuteQuerySegmentedAsync(new TableQuery<GroupModel>(), null).Result;
            var result = groups.Where(x => usergroups.FirstOrDefault(y => y.GroupId == x.RowKey) != null)
                .Select(x =>
                    new GetUserGroupsModel
                    {
                        Groupname = x.GroupName,
                        GroupId = x.RowKey
                    });
            return new JsonResult(result);
        }
    }
}
