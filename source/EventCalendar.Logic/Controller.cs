using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using EventCalendar.Entities;
using static System.String;

namespace EventCalendar.Logic
{
    public class Controller
    {
        private readonly ICollection<Event> _events;
        public int EventsCount { get { return _events.Count;} }

        public Controller()
        {
            _events = new List<Event>();
        }

        /// <summary>
        /// Ein Event mit dem angegebenen Titel und dem Termin wird für den Einlader angelegt.
        /// Der Titel muss innerhalb der Veranstaltungen eindeutig sein und das Datum darf nicht
        /// in der Vergangenheit liegen.
        /// Mit dem optionalen Parameter maxParticipators kann eine Obergrenze für die Teilnehmer festgelegt
        /// werden.
        /// </summary>
        /// <param name="invitor"></param>
        /// <param name="title"></param>
        /// <param name="dateTime"></param>
        /// <param name="maxParticipators"></param>
        /// <returns>Wurde die Veranstaltung angelegt</returns>
        public bool CreateEvent(Person invitor, string title, DateTime dateTime, int maxParticipators = 0)
        {
            bool valid = true;

            if (invitor == null)
                valid = false;

            if (string.IsNullOrEmpty(title))
                valid = false;

            foreach (var events_ in _events)
            {
                if(events_.Title.CompareTo(title) == 0)
                {
                    valid = false;
                    break;
                }
            }

            if (dateTime == null)
                throw new ArgumentNullException(nameof(dateTime));

            if (dateTime < DateTime.Now)
                valid = false;

            if (valid)
            {
                if (maxParticipators == 0)
                {
                    Event newEvent = new Event(invitor, title, dateTime);

                    _events.Add(newEvent);
                }
                else
                {
                    Event newEvent = new EventWithParticipatorLimit(invitor, title, dateTime, maxParticipators);

                    _events.Add(newEvent);
                }
            }
            return valid;
        }


        /// <summary>
        /// Liefert die Veranstaltung mit dem Titel
        /// </summary>
        /// <param name="title"></param>
        /// <returns>Event oder null, falls es keine Veranstaltung mit dem Titel gibt</returns>
        public Event GetEvent(string title)
        {
            Event result = null;

            foreach (var event_ in _events)
            {
                //if (event_.Title.CompareTo(title, StringComparison.CurrentCultureIgnoreCase) == 0)
                if(event_.Title.CompareTo(title) == 0)
                {
                    result = event_;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Person registriert sich für Veranstaltung.
        /// Eine Person kann sich zu einer Veranstaltung nur einmal registrieren.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="ev">Veranstaltung</param>
        /// <returns>War die Registrierung erfolgreich?</returns>
        public bool RegisterPersonForEvent(Person person, Event ev)
        {
            bool valid = false;

            if (person != null && ev != null)
            {
                Event event_ = GetEvent(ev.Title);

                if (event_ != null)
                {
                    valid = event_.AddPerson(person);
                    valid = true;
                }
            }

            return valid;
        }

        /// <summary>
        /// Person meldet sich von Veranstaltung ab
        /// </summary>
        /// <param name="person"></param>
        /// <param name="ev">Veranstaltung</param>
        /// <returns>War die Abmeldung erfolgreich?</returns>
        public bool UnregisterPersonForEvent(Person person, Event ev)
        {
            bool valid = false;

            if (person != null && ev != null)
            {
                Event event_ = GetEvent(ev.Title);

                if (event_ != null)
                {
                    valid = event_.RemovePerson(person);
                }
            }

            return valid;
        }

        /// <summary>
        /// Liefert alle Teilnehmer an der Veranstaltung.
        /// Sortierung absteigend nach der Anzahl der Events der Personen.
        /// Bei gleicher Anzahl nach dem Namen der Person (aufsteigend).
        /// </summary>
        /// <param name="ev"></param>
        /// <returns>Liste der Teilnehmer oder null im Fehlerfall</returns>
        public IList<Person> GetParticipatorsForEvent(Event ev)
        {
            List<Person> persons = null;

            if (ev != null)
            {
                Event event_ = GetEvent(ev.Title);

                if (event_ != null)
                {
                    persons = event_.GetParticipator();
                    persons.Sort();
                }
            }

            return persons;
        }

        /// <summary>
        /// Liefert alle Veranstaltungen der Person nach Datum (aufsteigend) sortiert.
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Liste der Veranstaltungen oder null im Fehlerfall</returns>
        public List<Event> GetEventsForPerson(Person person)
        {
            List<Event> events = null;

            if (person != null)
            {
                events = new List<Event>();

                foreach (var event_ in _events)
                {
                    if (event_.ContainsPerson(person))
                    {
                        events.Add(event_);
                    }
                }

            }

            return events;
        }

        /// <summary>
        /// Liefert die Anzahl der Veranstaltungen, für die die Person registriert ist.
        /// </summary>
        /// <param name="participator"></param>
        /// <returns>Anzahl oder 0 im Fehlerfall</returns>
        public int CountEventsForPerson(Person participator)
        {
            int count = 0;

            foreach (var event_ in _events)
            {
                if (event_.ContainsPerson(participator))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
