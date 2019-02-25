namespace Alpha.CN88.Services
{
    using System.Collections.Generic;
    using Models;
    using Models.Home;

    public interface HomeService
    {
        AccountOverview GetAccountOverview(int custId);

        StatusSummaryReport GetStatusSummary(int custId, UserLevel userLevel);

        IList<AnnouncementModel> GetImportantAnnouncement(int custId, string language);

        IList<AnnouncementModel> GetGeneralAnnouncement(int custId, string language);
    }
}
