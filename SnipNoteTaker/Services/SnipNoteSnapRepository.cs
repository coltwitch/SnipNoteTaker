using SnapDb;
using SnipNoteTaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnipNoteTaker.Services
{
    public class SnipNoteSnapRepository
    {
        public SnapRepository<SnipNote> Repository { get; set; }
        public SnipNoteSnapRepository()
        {
            var dbFilePath = "projects.json";
            Repository = new SnapRepository<SnipNote>(dbFilePath);
        }

        public void Save(SnipNote note)
        {
            var existing = Repository.Get();
            if (!existing.Any(x => x.ProjectName == note.ProjectName))
            {
                // Insert
                Repository.Insert(note);
                Repository.SaveChanges(); // SaveChanges writes the record to the store.
            }
        }

        public SnipNote Get(string projectName)
        {
            var existing = Repository.Get();
            var current = existing.FirstOrDefault(x => x.ProjectName == projectName);
            if (current == null)
            {
                current = new SnipNote(projectName);
                Repository.Insert(current);
                Repository.SaveChanges();
            }
            return current;
        }

        public List<SnipNote> GetAll()
        {
            var existing = Repository.Get();
            return existing.ToList();
        }

        //// Get all records
        //var allPeople = repository.Get();

        //// Search for a records using linq
        //Person doeFamilyMembers = repository.Get(p => p.LastName = "Doe");

        //// Updating records is as easy as changing the object and calling SaveChanges()
        //Person firstPerson = repository.Get().First();
        //firstPerson.LastName = "Smith";
        //repository.SaveChanges();

        //// Delete records
        //Person firstPerson = repository.Get().First();
        //repository.Delete(firstPerson);
        //repository.SaveChanges();
        
    }
}
