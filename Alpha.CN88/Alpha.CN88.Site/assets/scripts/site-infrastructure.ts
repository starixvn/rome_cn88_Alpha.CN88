"use strict";

declare var numeral: any;
interface JQuery {
    stickyTableHeaders(): void;
}

class SiteInfrastructure {
    pageList = [50, 100, 200, 500];

    customerStatus = {
        all: 0,
        open: 1,
        closed: 3,
        suspended: 2
    };

    closeReasonConstant = {
        LogInFailedOrExpired: 0,
        SecurityCode: 3,
        Upline: 2,
        Admin: 1
    };

    roleId = {
        master: 3,
        agent: 2,
        member: 1
    };
}

const siteInfrastructure = new SiteInfrastructure();