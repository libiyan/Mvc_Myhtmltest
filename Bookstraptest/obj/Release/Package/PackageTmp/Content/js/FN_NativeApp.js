//<meta name="apple-itunes-app" content="app-id=966143669, app-argument=nbbank://" appdownurl="MyAppDown" />
//<meta name="apple-itunes-app" content="app-id=966143669, affiliate-data=myAffiliateData, app-argument=myURL">
/*
 *	通过scheme协议来实现Wap对App的调用
 *  注意配置的scheme协议 时ios与adr 保持一致如 
 */

var agent = navigator.userAgent.toLowerCase();
var startTime = Date.now();
var config = {
    /*scheme:必须*/
    scheme_IOS: 'qianduanzy://',
    scheme_Adr: 'qianduanzy://qianduanzy_app', //'ningbo://nb_app/openwith?Name=user1&&PW=12345'
    download_url: AppDownUrl(),
    timeout: 400
};

function GoAppDownUrl() {
    window.parent.location.href = AppDownUrl();
}

function AppDownUrl() { 
    var androidUrl = "/Static/wx/index.html";
    var iosUrl = $('meta[name="apple-itunes-app"]').attr("appdownurl"); 
    if (/(iPhone|iPad|iPod|iOS)/i.test(agent)) { 
        return iosUrl;
    } 
   return androidUrl; 
}

function IsWeiXin() {
    if (agent.match(/MicroMessenger/i) == "micromessenger") {
        return true;
    } else {
        return false;
    }
}
function OpenApp(url) {
    var ifr = document.createElement('iframe');
    ifr.src = url;
    ifr.style.display = 'none';
    document.body.appendChild(ifr);
}
function WaitAppDown() {
    window.setTimeout(function () {
        var endTime = Date.now();
        if (!startTime || (endTime - startTime) < (config.timeout + 200)) {
            window.parent.location.href = config.download_url;
        } else { }
    }, config.timeout);
}

function AppDown() {
    startTime = Date.now();
    var url = agent.indexOf('os') > 0 ? config.scheme_IOS : config.scheme_Adr; //'Android'
    if (IsWeiXin()) {//微信暂不支持
        //encodeURI 方法不会对下列字符进行编码：":"、"/"、";" 和 "?"。请使用 encodeURIComponent 方法对这些字符进行编 
        url = 'http://mp.weixin.qq.com/mp/redirect?url=' + encodeURIComponent(config.download_url);
    } else {
        OpenApp(url); 
    } 
    WaitAppDown();
}

