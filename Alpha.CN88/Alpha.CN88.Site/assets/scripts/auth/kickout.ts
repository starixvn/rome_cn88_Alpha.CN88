"use strict";
class Kickout {
    init() {
        const message = siteInfrastructure.getInputElementvalue("message") || siteInfrastructure.getInputElementvalue("errorMessage");

        alert(message);
        siteInfrastructure.logout();
    }
}

$(() => {
    const kickout: Kickout = new Kickout();
    kickout.init();
});