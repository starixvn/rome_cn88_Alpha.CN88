class PopupBase {
	public $: JQueryStatic;
	public topWindow: any;

	constructor($: JQueryStatic, topWindow: any) {
		this.$ = $;
		this.topWindow = topWindow;
	};

	public registerEvents() {
		this.$(".btn-cancel").bind("click", this.closePopup);
	};

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
			}
		})
			.done(function (data) {
				if (data.IsSuccess) {
					self.$("#error-message")
						.text(self.$("#UpdateSuccessfully").val())
						.removeClass("hidden")
						.removeClass("msg-error")
						.addClass("msg-success");
				}
				else {
					self.$("#error-message")
						.text(data.Message)
						.removeClass("hidden")
						.removeClass("msg-success")
						.addClass("msg-error");
				}
			})
			.always(function () { });
	};
};