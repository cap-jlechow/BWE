namespace BlazorWasmElectron.Shared {
    using System;
    using System.Collections.Generic;
    using LiteDB;
    using System.Threading.Tasks;

    public class PeopleService {
        private const string FILENAME = "MyData.db";

        public async Task DeletePersonAsync(Person person) {
            await Task.Run(() => {
                using(var db = new LiteDatabase(FILENAME))
                {
                    var collection = db.GetCollection<Person>();
                    collection.Delete(person.Id);
                }
            });
        }

        public async Task<Person> SavePersonAsync(Person person) {
            return await Task.Run(() => {
                using(var db = new LiteDatabase(FILENAME))
                {
                    try {
                        var collection = db.GetCollection<Person>();
                        collection.Upsert(person);
                        collection.EnsureIndex(i => i.Id);
                    }
                    catch(Exception exc) {
                        Console.Error.WriteLine(exc);
                    }
                    return person;
                }
            });
        }

        public async Task<IList<Person>> GetPeopleAsync() {
            return await Task.Run(() => {
                using(var db = new LiteDatabase(FILENAME))
                {
                    var collection = db.GetCollection<Person>();
                    return collection.Query().ToList();
                }
            });
        }
    }
}