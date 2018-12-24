class PopupBase {
	public $: JQueryStatic;
	public topWindow: any;

	constructor($: JQueryStatic, topWindow: any) {
		this.$ = $;
		this.topWindow = topWindow;
	}

	public registerEvents() {
		this.$(".btn-cancel").bind("click", this.closePopup);
	}

	closePopup = () => {
		this.topWindow.ageWnd.close();
	}

	public submit(url: string, data: any) {
		let self = this;

		this.$.ajax({
			type: "POST",
			url: url,
			data: data,
			dataType: "json",
			beforeSend: function (xhr) {
				xhr.setRequestHeader("__RequestVerificationToken", $("input[name=\"__RequestVerificationToken\"]").val());
			}})
			.done(function (data) {
				data.IsSuccess ? self.updateSuccesfully() : self.updateFailed(data.Message);
			})
			.always(function () { });
	}

	public updateSuccesfully() {
		this.$("#error-message").html('')
			.text(this.$("#UpdateSuccessfully").val())
			.removeClass("hidden")
			.removeClass("msg-error")
			.addClass("msg-success");
	}

	public updateFailed(message: any) {
		this.$("#error-message").html('')
			.text(message)
			.removeClass("hidden")
			.removeClass("msg-success")
			.addClass("msg-error");
	}
};