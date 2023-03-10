using MongoDB.Driver;
using MongoDbAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDbAPI.Services
{
    public class StudentService : IStudentService
    {
        private IMongoCollection<Student> _students;

        public StudentService(IStudentDatastoreDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _students = database.GetCollection<Student>(settings.StudentCourseCollectionName);     
        
        }

        public Student Create(Student student)
        {
            _students.InsertOne(student);
            return student; 
        }

        public List<Student> Get()
        {
            //because we want all students we just type true in find linq method
            return _students.Find(student => true).ToList();
        }

        public Student Get(string id)
        {
            return _students.Find(student => student.Id == id).FirstOrDefault();
        }

        public void Remove(string id)
        {
            _students.DeleteOne(student => student.Id == id); 
        }

        public void Update(string id, Student student)
        {
            _students.ReplaceOne(student => student.Id == id, student); 
        }
    }
}
