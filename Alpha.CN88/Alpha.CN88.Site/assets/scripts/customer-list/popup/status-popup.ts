class StatusPopup extends PopupBase {
	constructor($: JQueryStatic, topWindow: any) {
		super($, topWindow);
	};

	init = () => {
		this.registerEvents();
	};

	registerEvents() {
		super.registerEvents();
		this.$("#save-btn").bind("click", this.updateStatus);
	};

	updateStatus = () => {
		let url = this.$("#dropdown-status").val() == 2 ? "UpdateSuspended" : "UpdateClosed";
		super.submit(url, {});
	};
};

$(() => {
	const statusPopup: any = new StatusPopup($, window.top);
	statusPopup.init();
});
