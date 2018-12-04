var Scientist = Scientist || {};

$(document).ready(function () {
    Scientist.Booking.Init();
});

Scientist.Booking = {
    Init: function () {

        $(".booking-button").click(function () {

            Scientist.Booking.TrackBookingEvent(this);

        });

    },

    TrackBookingEvent: function (element) {

        var dataContainer = $(element);

        var jsonObject = {
            PageEventId: dataContainer.data("goal"),
            EventName: "Hotel Booking",
            Data: "Booking",
            Datakey: "HotelName",
            Text:"Booking button clicked"
        };
        var analyticsEvent = new AnalyticsPageEvent(jsonObject);
        analyticsEvent.trigger();
    }
};