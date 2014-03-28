using System;
using System.Globalization;

namespace JPB.Extentions.Extensions
{
    public static class DateHelperExtention
    {
        public static int GetCalendarWeek(this DateTime date)
        {
            // Aktuelle Kultur ermitteln
            CultureInfo currentCulture = CultureInfo.CurrentCulture;

            // Aktuellen Kalender ermitteln
            Calendar calendar = currentCulture.Calendar;

            // Kalenderwoche über das Calendar-Objekt ermitteln
            int calendarWeek = calendar.GetWeekOfYear(date,
                                                      currentCulture.DateTimeFormat.CalendarWeekRule,
                                                      currentCulture.DateTimeFormat.FirstDayOfWeek);

            // Überprüfen, ob eine Kalenderwoche größer als 52
            // ermittelt wurde und ob die Kalenderwoche des Datums
            // in einer Woche 2 ergibt: In diesem Fall hat
            // GetWeekOfYear die Kalenderwoche nicht nach ISO 8601 
            // berechnet (Montag, der 31.12.2007 wird z. B.
            // fälschlicherweise als KW 53 berechnet). 
            // Die Kalenderwoche wird dann auf 1 gesetzt
            if (calendarWeek > 52)
            {
                date = date.AddDays(7);
                int testCalendarWeek = calendar.GetWeekOfYear(date,
                                                              currentCulture.DateTimeFormat.CalendarWeekRule,
                                                              currentCulture.DateTimeFormat.FirstDayOfWeek);
                if (testCalendarWeek == 2)
                    calendarWeek = 1;
            }

            // Das Jahr der Kalenderwoche ermitteln
            int year = date.Year;
            if (calendarWeek == 1 && date.Month == 12)
                year++;
            if (calendarWeek >= 52 && date.Month == 1)
                year--;

            // Die ermittelte Kalenderwoche zurückgeben
            return calendarWeek;
        }
    }
}