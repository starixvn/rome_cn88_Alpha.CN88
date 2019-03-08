"use strict";
class Kickout {
    init() {
        let messageElement: any = document.getElementById("message");
        let message: string = messageElement != null
            ? (<HTMLInputElement>messageElement).value
            : "Oops... you are here because of a system error, please notify the administrators.";

        alert(message);
        siteInfrastructure.logout();
    }
}

$(() => {
    const kickout: Kickout = new Kickout();
    kickout.init();
});