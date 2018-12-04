var eventTracker = false;

function AnalyticsPageEvent(jsonData) {
    this.jsonData = jsonData;
    this.trigger = function () {
        var queryString = '';

        if (!this.jsonData) {
            return;
        }
        queryString += '&' + 'jsonData' + '=' + JSON.stringify(this.jsonData);
        if (queryString != '') {
            var url = '/sitecore/analytics/ClientEventTracker.ashx' + '?ra=' + eventTracker.randomstring() + queryString;
            eventTracker.request(url);
        }
    };
}

function EventTracker() {
    this.request = function (url) {
        var script = new ClientEventScript(url, true);
        script.load();
    };

    this.randomstring = function () {
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var text = "";

        for (var i = 0; i < 32; i++) {
            text += possible.charAt(Math.floor(Math.random() * possible.length));
        }

        return text;
    };
}

function ClientEventScript(src, async) {
    this.src = src;
    this.async = async;

    this.load = function () {
        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = this.src;
        script.async = this.async;

        var ssc = document.getElementsByTagName('script')[0];
        ssc.parentNode.insertBefore(script, ssc);
    };
}
eventTracker = new EventTracker();
