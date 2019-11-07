using System;
using System.Collections.Generic;

namespace EventCalendar.Entities
{
    public class Event
    {
        private Person _invitor;
        private string _title;
        private DateTime _dateTime;
        internal List<Person> _persons;

        public Event(Person invitor, string title, DateTime dateTime)
        {
            _invitor = invitor;
            _title = title;
            _dateTime = dateTime;
            _persons = new List<Person>();
        }

        public string Title {
            get { return _title; }
        }

        public virtual bool AddPerson(Person person)
        {
            bool valid = false;

            if (_persons.Contains(person) == false)
            {
                _persons.Add(person);
                person.NumberOfEvents++;
                valid = true;
            }

            return valid;
        }

        public bool RemovePerson(Person person)
        {
            bool valid = false;

            if (_persons.Contains(person))
            {
                _persons.Remove(person);
                person.NumberOfEvents--;
                valid = true;
            }

            return valid;
        }

        public List<Person> GetParticipator()
        {
            List<Person> persons = new List<Person>();

            foreach (var person in _persons)
            {
                persons.Add(person);
            }
            return persons;
        }

        public bool ContainsPerson(Person person)
        {
            return _persons.Contains(person);
        }
    }
}
