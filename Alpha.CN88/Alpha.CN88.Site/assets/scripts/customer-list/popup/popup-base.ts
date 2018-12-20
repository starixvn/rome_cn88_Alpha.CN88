class PopupBase {
	$: JQueryStatic;
	topWindow: any;

	constructor($: JQueryStatic, topWindow: any) {
		this.$ = $;
		this.topWindow = topWindow;
	};

	submit = (url: string, data: any) => {
		let self = this;
		this.$.ajax({
			type: "POST",
			url: url,
			data: data,
			dataType: "json",
			beforeSend: function (xhr) {
				//	xhr.setRequestHeader("__RequestVerificationToken", requestVerificationToken);
			}
		})
			.done(function (data) {
				if (data.IsSuccessful) {
				}
				else {
					self.$("#error-message").text(data.Message).removeClass("hidden");
				}
			})
			.always(function () { });
	};
};