class StatusPopup extends PopupBase {
	constructor($: JQueryStatic, topWindow: any) {
		super($, topWindow);
	};

	init = () => {
		this.registerEvents();
	};

	registerEvents() {
		super.registerEvents();
		this.$(".btn-submit").bind("click", this.updateStatus);
	};

	updateStatus = () => {
		this.submit("UpdateCustomerStatus", {"CustId": this.$("#CustId").val(), "UpdateStatusId": this.$("#dropdown-status").val() });
	};
};

$(() => {
	const statusPopup: any = new StatusPopup($, window.top);
	statusPopup.init();
});
