﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EventCalendar.Entities
{

    /// <summary>
    /// Person kann sowohl zu einer Veranstaltung einladen,
    /// als auch an Veranstaltungen teilnehmen
    /// </summary>
    public class Person : IComparable<Person>
    {
        public string LastName { get; }
        public string FirstName { get; }
        public string MailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfEvents { get; set; }


        public Person(string lastName, string firstName)
        {
            LastName = lastName;
            FirstName = firstName;
        }

        public int CompareTo(Person other)
        {
            int result = -(NumberOfEvents.CompareTo(other.NumberOfEvents));

            if (result == 0)
            {
                result = LastName.CompareTo(other.LastName);

                if (result == 0)
                {
                    result = FirstName.CompareTo(other.FirstName);
                }
            }

            return result;
        }
    }
}