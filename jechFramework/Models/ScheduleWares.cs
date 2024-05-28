using System;

namespace jechFramework.Models
{
    public enum ScheduleType
    {
        Daily,
        Weekly
    }

    public class ScheduleWares
    {
        public int ScheduleId { get; set; }
        public ScheduleType Type { get; set; }
        public DateTime ScheduleTime { get; set; }

        public ScheduleWares(int scheduleId, ScheduleType type, DateTime scheduleTime)
        {
            ScheduleId = scheduleId;
            Type = type;
            ScheduleTime = scheduleTime;
        }
    }
}