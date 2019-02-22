namespace Alpha.CN88.Models
{
    using System;
    using Extensions;

    public class AnnouncementModel
    {
        public AnnouncementModel()
        {
        }

        public AnnouncementModel(DateTime affectedDate, string content)
        {
            AffectedDate = affectedDate;
            Content = content;
        }

        public DateTime AffectedDate { get; }

        public string Content { get; }

        public string AffectedDateStr => AffectedDate.ToShortDateFormat();
    }
}
