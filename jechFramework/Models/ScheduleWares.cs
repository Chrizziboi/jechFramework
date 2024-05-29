using System;

namespace jechFramework.Models
{
    /// <summary>
    /// Representerer typen tidsplan for varer som skal sendes ut.
    /// </summary>
    public enum ScheduleType
    {
        /// <summary>
        /// Daglig tidsplan.
        /// </summary>
        Daily,

        /// <summary>
        /// Ukentlig tidsplan.
        /// </summary>
        Weekly,

    }

    /// <summary>
    /// Representerer en plan for å sende ut varer.
    /// </summary>
    public class ScheduleWares
    {
        /// <summary>
        /// Henter eller setter ID-en for tidsplanen.
        /// </summary>
        public int ScheduleId { get; set; }

        /// <summary>
        /// Henter eller setter typen tidsplan.
        /// </summary>
        public ScheduleType Type { get; set; }

        /// <summary>
        /// Henter eller setter tidspunktet for tidsplanen.
        /// </summary>
        public DateTime ScheduleTime { get; set; }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="ScheduleWares"/> klassen.
        /// </summary>
        /// <param name="scheduleId">ID-en for tidsplanen.</param>
        /// <param name="type">Typen tidsplan.</param>
        /// <param name="scheduleTime">Tidspunktet for tidsplanen.</param>
        public ScheduleWares(int scheduleId, ScheduleType type, DateTime scheduleTime)
        {
            ScheduleId = scheduleId;
            Type = type;
            ScheduleTime = scheduleTime;
        }
    }
}