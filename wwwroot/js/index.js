$(document).ready(function () {
	console.log("Hello PS I hid the form");



	var theForm = $("#theForm");
	theForm.hide();

	var button = $("#buyButton");
	button.on("click", function () {
		console.log("Buying Item");
	});


	var productInfo = $(".product-props li");

	productInfo.on("click", function () {
		console.log("You clicked on " + $(this).text());
	});


	var $loginToggle = $("#loginToggle");
	var $popupForm = $(".popup-form");

	$loginToggle.on("click", function () {
		$popupForm.slideToggle(200);
	});

});