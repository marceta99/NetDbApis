using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDbAPI.Models
{
    public class StudentDatastoreDatabaseSettings : IStudentDatastoreDatabaseSettings
    {
        public string StudentCourseCollectionName { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty; 
    }
}
