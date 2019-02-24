function fillClientTime() {
	var myHidden = document.getElementsByClassName("essign_txtClientTime")[0];
	var now = new Date();
	myHidden.value = now.toUTCString();
	return false;
}

function loadInfo() {
	try {
		vnptToken.applet = document.getElementById("vnptTokenApplet");
		var xmlList = vnptToken.applet.loadCertificateInfo();
		if (xmlList != "") {
			var xmlDoc = xml2json.parser(xmlList);
			//document.getElementById("serialNumber").value = xmlDoc.certificate.serialnumber;
			//document.getElementById("base64").value = xmlDoc.certificate.base64;
			alert(xmlDoc.certificate.serialnumber);
		}
	}
	catch (err) {
		alert(err);
	}
}

function signDocxBase64(inputs) {
	try {
		var xmlList = vnptToken.applet.loadCertificateInfo();
		if (xmlList == "")
			throw "The action was cancelled by the user.";

		var serialnumber = xml2json.parser(xmlList).certificate.serialnumber;
		var arrBase64 = inputs.split(";");

		var outputs = "";
		for (var i = 0; i < arrBase64.length; ++i) {
			var output = vnptToken.applet.signDocxBase64(serialnumber, arrBase64[i]);
			if (output == "")
				throw "The action was cancelled by the user.";
			outputs += output + ";";
		}

		document.getElementsByClassName("essign_txtBase64")[0].value = outputs;
		document.getElementsByClassName("essign_btnUpload")[0].click();
	}
	catch (err) {
		alert(err);
		document.getElementsByClassName("essign_btnUpload")[0].click();
	}
}

function signPdfBase64(inputs) {
	try {
		var xmlList = vnptToken.applet.loadCertificateInfo();
		if (xmlList == "")
			throw "The action was cancelled by the user.";

		var serialnumber = xml2json.parser(xmlList).certificate.serialnumber;
		var arrBase64 = inputs.split(";");

		var outputs = "";
		for (var i = 0; i < arrBase64.length; ++i) {
			var output = vnptToken.applet.signPdfBase64(serialnumber, arrBase64[i], 0, 0, 200, 50);
			if (output == "")
				throw "The action was cancelled by the user.";
			outputs += output + ";";
		}

		document.getElementsByClassName("essign_txtBase64")[0].value = outputs;
		document.getElementsByClassName("essign_btnUpload")[0].click();
	}
	catch (err) {
		alert(err);
		document.getElementsByClassName("essign_btnUpload")[0].click();
	}
}

function signXmlBase64(inputs) {
	try {
		var xmlList = vnptToken.applet.loadCertificateInfo();
		if (xmlList == "")
			throw "The action was cancelled by the user.";

		var serialnumber = xml2json.parser(xmlList).certificate.serialnumber;
		var arrBase64 = inputs.split(";");

		var outputs = "";
		for (var i = 0; i < arrBase64.length; ++i) {
			var output = vnptToken.applet.signXml(serialnumber, arrBase64[i]);
			if (output == "")
				throw "The action was cancelled by the user.";
			outputs += output + ";";
		}

		document.getElementsByClassName("essign_txtBase64")[0].value = outputs;
		document.getElementsByClassName("essign_btnUpload")[0].click();
	}
	catch (err) {
		alert(err);
		document.getElementsByClassName("essign_btnUpload")[0].click();
	}
}

function loadCertificate() {
    try {
        var xmlList = vnptToken.applet.loadCertificateInfo();
        if (xmlList == "")
            throw "The action was cancelled by the user.";

        var outputs = xml2json.parser(xmlList).certificate.base64 + ";";        

        document.getElementsByClassName("essign_txtBase64")[0].value = outputs;
        document.getElementsByClassName("essign_btnUpload")[0].click();
    }
    catch (err) {
        alert(err);
        document.getElementsByClassName("essign_btnUpload")[0].click();
    }
}

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};

function submitformBC() {
    $("#btn_SubmitBC").click();
};

function CallApp(callApp, KeySign, CAVersion) {
    if (callApp) {
        $("#aSign").attr("href", "TTDSignApp:" + KeySign + "@~" + CAVersion);
        performWinAppSign(10);
        console.log($("#aSign").attr('href'));
    }
}

//function performAppletSign(timeout, fileExt) {
//	vnptToken.applet = document.getElementById("vnptTokenApplet");
//	var inputs = document.getElementsByClassName("essign_txtBase64")[0].value;
//	document.getElementsByClassName("essign_txtBase64")[0].value = "";

//	performAppletSign_Loop(timeout, fileExt);

//	if (checkAppletReady(timeout)) {
//		if (fileExt == '.xlsx' || fileExt == '.docx')
//			signDocxBase64(inputs);
//		else if (fileExt == '.pdf')
//			signPdfBase64(inputs);
//		else if (fileExt == '.xml' || fileExt == '.bid')
//		    signXmlBase64(inputs);
//		else if (fileExt == '.cer')
//		    loadCertificate();
//		else {
//			alert('File extension is not supported.');
//			document.getElementsByClassName("essign_btnUpload")[0].click();
//		}
//	}
//	else {
//		alert('Applet failed to load.');
//		document.getElementsByClassName("essign_btnUpload")[0].click();
//	}
//}

//function checkAppletReady(timeout, fileExt) {
//	try {
//		var b = vnptToken.applet.isActive();
//		return b;
//	}
//	catch (err) {
//		if (timeout > 0)
//			setTimeout(function () { checkAppletReady(--timeout); }, 1000);
//		else
//			return false;
//	}
//}

function performWinAppSign(timeout) {
    //document.getElementsByClassName("essign_txtBase64")[0].value = "";
    document.getElementsByClassName("essign_btnSign")[0].click();
    
    //performWinAppSign_Loop(timeout);
}

//function performWinAppSign_Loop(timeout) {
//    //Kiểm tra file báo app đã chạy trong AppData
//    var has_run = false;
//    //try {
//    //    var isDone = vnptToken.applet.isActive();
//    //} catch (err) {
//    //    isDone = false;
//    //}

//    if (!has_run && timeout > 0) {
//        setTimeout(function () { performAppletSign_Loop(--timeout); }, 1000);
//    }
//    else if (has_run) {
//        //if (fileExt == '.xlsx' || fileExt == '.docx')
//        //    signDocxBase64(vnptToken.inputs);
//        //else if (fileExt == '.pdf')
//        //    signPdfBase64(vnptToken.inputs);
//        //else if (fileExt == '.xml' || fileExt == '.bid')
//        //    signXmlBase64(vnptToken.inputs);
//        //else if (fileExt == '.cer')
//        //    loadCertificate();
//        //else {
//        //    alert('File extension is not supported.');
//        //    document.getElementsByClassName("essign_btnUpload")[0].click();
//        //}
//    }
//    else {
//        alert('Signing App failed to load.');
//        document.getElementsByClassName("essign_btnUpload")[0].click();
//    }
//}