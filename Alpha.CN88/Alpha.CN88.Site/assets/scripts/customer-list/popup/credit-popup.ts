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
		let minCredit = this.$("#MinCredit").val();
		let maxCredit = this.$("#MaxCredit").val();
		let credit = this.$("#credit").val();

		if (credit < minCredit || credit > maxCredit) {
			this.$("#error-message").html('').text("Invalid Value").removeClass("hidden");
			return;
		}
		this.submit("UpdateCredit", {});
	};
};

$(() => {
	const creditPopup: any = new CreditPopup($, window.top);
	creditPopup.init();
});
