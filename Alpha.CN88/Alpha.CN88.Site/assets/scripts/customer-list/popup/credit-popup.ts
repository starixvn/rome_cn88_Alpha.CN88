class CreditPopup extends PopupBase {
	constructor($: JQueryStatic, topWindow: any) {
		super($, topWindow);
	};

	init = () => {
		this.registerEvents();
	};

	registerEvents() {
		super.registerEvents();
		this.$(".btn-submit").bind("click", this.updateCredit);
	};

	updateCredit = () => {
		let minCredit = parseFloat(this.$("#MinCredit").val());
		let maxCredit = parseFloat(this.$("#MaxCredit").val());
		let credit = parseFloat(this.$("#credit").val());

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
