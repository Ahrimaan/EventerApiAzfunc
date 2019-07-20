using EventerAPI.DbModel;
using EventerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using EventerAPI.Helper;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;

namespace EventerAPI
{
    public static class AddUserGroup
    {
        [FunctionName("AddUserGroup")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "addusergroup")] HttpRequest req,
            [Table("UserGroups", "TABLESTORAGE_CONNSTTRING")] IAsyncCollector<UserGroupModel> userGroups,
            [Table("Groups", "TABLESTORAGE_CONNSTTRING")] IAsyncCollector<GroupModel> groups,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var inputModel = JsonConvert.DeserializeObject<AddNewGroupModel>(requestBody);
            StringValues userId;
            if(!req.Headers.TryGetValue("userId",out userId))
            {
                return new BadRequestObjectResult("Whoops, there is no UserId");
            }
            var group = new GroupModel(inputModel.GroupName, userId);
            try
            {
                await groups.AddAsync(group);
            }
            catch(Exception e)
            {
                log.LogError(e, $"Error in inserting group by user { userId }");
                return new StatusCodeResult(500);
            }
            var userGroup = new UserGroupModel(userId,group.RowKey);
            try
            {
                await userGroups.AddAsync(userGroup);
            }
            catch (Exception e)
            {
                log.LogError(e, $"Error in inserting usergroup by user { userId } and groupId { group.RowKey }");
                return new StatusCodeResult(500);
            }


            return new JsonResult(new { groupid = group.RowKey });
        }
    }
}
