declare var Cleave: any;

class CreditPopup extends PopupBase {
	cleaveCredit: any;
	constructor($: JQueryStatic, topWindow: any) {
		super($, topWindow);
	};

	init = () => {
		this.registerEvents();
		this.cleaveCredit = new Cleave('#credit', {
			numeral: true,
			numeralDecimalMark: '.',
			delimiter: ','
		});
	};

	registerEvents() {
		super.registerEvents();
		this.$(".btn-submit").bind("click", this.updateCredit);
	};

	updateCredit = () => {
		let minCredit = parseFloat(this.$("#MinCredit").val());
		let maxCredit = parseFloat(this.$("#MaxCredit").val());
		let credit = parseFloat(this.cleaveCredit.getRawValue());
		let custId = this.$("#CustId").val();

		if (isNaN(credit) || credit < minCredit || credit > maxCredit) {
			this.updateFailed("Invalid Value");
			return;
		}

		this.submit("UpdateCredit", {"CustId": custId, "CreditAmount": credit});
	};
};

$(() => {
	const creditPopup: any = new CreditPopup($, window.top);
	creditPopup.init();
});
