"use strict";
class Kickout {
    init() {
        let messageElement: any = document.getElementById("message");
        let message: string = messageElement != null
            ? (<HTMLInputElement>messageElement).value
            : (<HTMLInputElement>document.getElementById("errorMessage")).value;

        alert(message);
        siteInfrastructure.logout();
    }
}

$(() => {
    const kickout: Kickout = new Kickout();
    kickout.init();
});