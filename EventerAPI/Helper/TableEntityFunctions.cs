using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace EventerAPI.Helper
{
    public static class TableEntityFunctions
    {
        public static async void InsertEntity<T>(T entity,CloudTable table, bool forInsert = true) where T : TableEntity, new()
        {
            try
            {
                if (forInsert)
                {
                    var insertOperation = TableOperation.Insert(entity);
                    await table.ExecuteAsync(insertOperation);
                }
                else
                {
                    var insertOrMergeOperation = TableOperation.InsertOrReplace(entity);
                    await table.ExecuteAsync(insertOrMergeOperation);
                }
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
    }
}
