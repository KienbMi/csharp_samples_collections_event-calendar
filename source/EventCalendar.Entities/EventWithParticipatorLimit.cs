﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EventCalendar.Entities
{
    public class EventWithParticipatorLimit : Event
    {
        private int _maxParticipators;

        public EventWithParticipatorLimit(Person invitor, string title, DateTime dateTime, int maxParticipators) : base(invitor, title, dateTime)
        {
            _maxParticipators = maxParticipators;
        }

        public override bool AddPerson(Person person)
        {
            return (_maxParticipators > _persons.Count) ? base.AddPerson(person) : false;
        }
    }
}
