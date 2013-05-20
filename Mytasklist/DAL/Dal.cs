using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mytasklist.Models;
using MongoDB.Driver;
using System.Configuration;

namespace MyTaskListApp
{
    public class Dal : IDisposable
    {
        private MongoServer mongoServer = null;
        private bool disposed = false;

        // To do: update the connection string with the DNS name
        // or IP address of your server. 
        //For example, "mongodb://testlinux.cloudapp.net"
        private string connectionString = "mongodb://mangotest.cloudapp.net:28017";

        // This sample uses a database named "Tasks" and a 
        //collection named "TasksList".  The database and collection 
        //will be automatically created if they don't already exist.
        private string dbName = "Test";
        private string collectionName = "Roshan";

        // Default constructor.        
        public Dal()
        {
        }

        // Gets all Task items from the MongoDB server.        
        public List<Task> GetAllTasks()
        {
            try
            {
                MongoCollection<Task> collection = GetTasksCollection();
                return collection.FindAll().ToList<Task>();
            }
            catch (MongoConnectionException)
            {
                return new List<Task>();
            }
        }

        // Creates a Task and inserts it into the collection in MongoDB.
        public void CreateTask(Task task)
        {
            MongoCollection<Task> collection = GetTasksCollectionForEdit();
            try
            {
                collection.Insert(task, SafeMode.True);
            }
            catch (MongoCommandException ex)
            {
                string msg = ex.Message;
            }
        }

        private MongoCollection<Task> GetTasksCollection()
        {
            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase database = server[dbName];
            MongoCollection<Task> todoTaskCollection = database.GetCollection<Task>(collectionName);
            return todoTaskCollection;
        }

        private MongoCollection<Task> GetTasksCollectionForEdit()
        {
            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase database = server[dbName];
            MongoCollection<Task> todoTaskCollection = database.GetCollection<Task>(collectionName);
            return todoTaskCollection;
        }

        # region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (mongoServer != null)
                    {
                        this.mongoServer.Disconnect();
                    }
                }
            }

            this.disposed = true;
        }

        # endregion
    }
}