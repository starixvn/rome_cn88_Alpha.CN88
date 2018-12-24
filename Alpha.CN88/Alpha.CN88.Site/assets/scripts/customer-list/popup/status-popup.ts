class StatusPopup extends PopupBase {
	constructor($: JQueryStatic, topWindow: any) {
		super($, topWindow);
	}

	init = () => {
		this.registerEvents();
		super.getResource();
	}

	registerEvents() {
		super.registerEvents();
		this.$(".btn-submit").bind("click", this.updateStatus);
	}

	updateStatus = () => {
		let custId = this.$("#CustId").val(),
			updateStatusId = this.$("#dropdown-status").val(),
			data = { "CustId": custId, "UpdateStatusId": updateStatusId };
		
		this.submit("UpdateCustomerStatus", data, custId, updateStatusId);
	}

	reloadCallback(custId: any, newValue: any) {
		this.topWindow.main.customerList.reloadStatusValue(custId, newValue);
	}
	
};

$(() => {
	const statusPopup: any = new StatusPopup($, window.top);
	statusPopup.init();
});
