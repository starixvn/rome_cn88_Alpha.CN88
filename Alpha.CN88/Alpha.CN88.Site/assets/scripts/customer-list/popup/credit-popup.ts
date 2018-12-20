class CreditPopup extends PopupBase {
	constructor($: JQueryStatic, topWindow: any) {
		super($, topWindow);
	};

	init = () => {
		this.registerEvents();
	};

	registerEvents = (): void => {
		this.$("#save-btn").bind("click", this.updateCredit);
	};

	updateCredit = () => {
		this.submit("UpdateCredit", {});
	};
};

$(() => {
	const creditPopup: any = new CreditPopup($, window.top);
	creditPopup.init();
});
