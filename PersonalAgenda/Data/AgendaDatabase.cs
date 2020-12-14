using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using PersonalAgenda.Models;
using System;
using System.Collections;

namespace PersonalAgenda.Data
{
    public class AgendaDatabase
    {
        readonly SQLiteAsyncConnection _database;
        public AgendaDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Agenda>().Wait();
            _database.CreateTableAsync<Activity>().Wait();
            _database.CreateTableAsync<NoteActivity>().Wait();
        }

        public Task<int> SaveActivityAsync(Activity activity)
        {
            if (activity.ID != 0)
            {
                return _database.UpdateAsync(activity);
            }
            else
            {
                return _database.InsertAsync(activity);
            }
        }
        public Task<int> DeleteActivityAsync(Activity activity)
        {
            return _database.DeleteAsync(activity);
        }
        public Task<List<Activity>> GetActivitiesAsync()
        {
            return _database.Table<Activity>().ToListAsync();
        }

        public Task<List<Agenda>> GetNotesAsync()
        {
            return _database.Table<Agenda>().ToListAsync();
        }

        public Task<Agenda> GetNoteAsync(int id)
        {
            return _database.Table<Agenda>()
            .Where(i => i.ID == id)
           .FirstOrDefaultAsync();
        }
        public Task<int> SaveNoteAsync(Agenda note)
        {
            if (note.ID != 0)
            {
                return _database.UpdateAsync(note);
            }
            else
            {
                return _database.InsertAsync(note);
            }
        }
        public Task<int> DeleteNoteAsync(Agenda note)
        {
            return _database.DeleteAsync(note);
        }

        public Task<int> SaveNoteActivityAsync(NoteActivity noteactivity)
        {
            if (noteactivity.ID != 0)
            {
                return _database.UpdateAsync(noteactivity);
            }
            else
            {
                return _database.InsertAsync(noteactivity);
            }
        }
        public Task<List<Activity>> GetNoteActivitiesAsync(int agendaid)
        {
            return _database.QueryAsync<Activity>(
            "select A.ID, A.Description from Activity A"
            + " inner join NoteActivity NA"
            + " on A.ID = NA.ActivityID where NA.AgendaID = ?",
            agendaid);
        }

    }
}