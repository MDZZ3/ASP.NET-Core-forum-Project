var returnUrl = window.location.pathname + window.location.search;
$("#LoginBtn").attr("href", "/Account/Login?returnUrl=" + encodeURIComponent(returnUrl));

