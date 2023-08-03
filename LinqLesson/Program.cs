using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace LinqLesson
{

    class Program
    {
        static IEnumerable<Person> persons = JsonConvert.DeserializeObject<IEnumerable<Person>>(File.ReadAllText("data.json"));

        static void Main ()
        {
            FindLocation();
            Console.WriteLine(new string('-', 30));
            DistanceBetween();
            Console.WriteLine(new string('-', 30));
            SimilarWords();
            Console.WriteLine(new string('-', 30));
            SameFriends();
        }

        static void FindLocation ()
        {

            var farthestNorth = persons.OrderByDescending(pers => pers.Latitude).FirstOrDefault();
            Console.WriteLine($"Farthest North: {farthestNorth.Name}, Latitude: {farthestNorth.Latitude}");

            var farthestSouth = persons.OrderBy(pers => pers.Latitude).FirstOrDefault();
            Console.WriteLine($"Farthest South: {farthestSouth.Name}, Latitude: {farthestSouth.Latitude}");

            var farthestWest = persons.OrderBy(pers => pers.Longitude).FirstOrDefault();
            Console.WriteLine($"Farthest West: {farthestWest.Name}, Longitude: {farthestWest.Longitude}");

            var farthestEast = persons.OrderByDescending(pers => pers.Longitude).FirstOrDefault();
            Console.WriteLine($"Farthest East: {farthestEast.Name}, Longitude: {farthestEast.Longitude}");
        }

        static void DistanceBetween ()
        {

            double maxDist = double.MinValue;
            double minDist = double.MaxValue;

            for (int i = 0; i < persons.Count(); i++) {
                for (int j = i + 1; j < persons.Count(); j++) {
                    var distance = Math.Sqrt(Math.Pow(persons.ElementAt(i).Latitude - persons.ElementAt(j).Latitude, 2) +
                                             Math.Pow(persons.ElementAt(i).Longitude - persons.ElementAt(j).Longitude, 2));

                    if (distance > maxDist)
                        maxDist = distance;

                    if (distance < minDist)
                        minDist = distance;
                }
            }

            Console.WriteLine($"Max Distance: {maxDist}");
            Console.WriteLine($"Min Distance: {minDist}");
        }

        static void SimilarWords ()
        {

            var maxWords = 0;
            Person personWithWords1 = new();
            Person personWithWords2 = new();

            foreach (var person1 in persons) {
                foreach (var person2 in persons) {
                    if (person1.Id != person2.Id) {

                        var wordsCount = person1.About.Split(' ')
                            .Intersect(person2.About.Split(' ')).Count();

                        if (wordsCount > maxWords) {
                            maxWords = wordsCount;
                            personWithWords1 = person1;
                            personWithWords2 = person2;
                        }
                    }
                }
            }

            Console.WriteLine($"Person 1: {personWithWords1.Name}");
            Console.WriteLine($"Person 2: {personWithWords2.Name}");
            Console.WriteLine($"Number of Similar Words: {maxWords}");
        }

        static void SameFriends ()
        {
            var personsWithSameFriends = persons
                .SelectMany(person => person.Friends.Select(friend => new { Person = person, FriendName = friend.Name }))
                .GroupBy(p => p.FriendName)
                .Where(c => c.Count() >= 1)
                .SelectMany(c => c.Select(p => p.Person))
                .Distinct();

            foreach (var person in personsWithSameFriends) {
                Console.WriteLine($"Person '{person.Name}' has the same friends");
            }
        }


    }


}


