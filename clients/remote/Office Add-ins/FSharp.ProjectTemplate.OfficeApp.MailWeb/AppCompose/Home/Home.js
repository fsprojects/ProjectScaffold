
/// <reference path="../App.js" />

(function () {
    'use strict';

    // The initialize function must be run each time a new page is loaded
    Office.initialize = function (reason) {
        $(document).ready(function () {
            app.initialize();

            //$('#set-subject').click(setSubject);
            $('#get-subject').click(getSubject);
            $('#add-to-recipients').click(addToRecipients);
        });
    };

    function setSubject() {
        Office.cast.item.toItemCompose(Office.context.mailbox.item).subject.setAsync("Hello world!");
    }

    function getSubject() {
        Office.cast.item.toItemCompose(Office.context.mailbox.item).subject.getAsync(function (result) {
            app.showNotification('The current subject is', result.value)
        });
    }

    function addToRecipients() {
        var item = Office.context.mailbox.item;
        var addressToAdd = {
            displayName: Office.context.mailbox.userProfile.displayName,
            emailAddress: Office.context.mailbox.userProfile.emailAddress
        };
 
        if (item.itemType === Office.MailboxEnums.ItemType.Message) {
            Office.cast.item.toMessageCompose(item).to.addAsync([addressToAdd]);
        } else if (item.itemType === Office.MailboxEnums.ItemType.Appointment) {
            Office.cast.item.toAppointmentCompose(item).requiredAttendees.addAsync([addressToAdd]);
        }
    }

    $(document).ready(function () {
        var AppCompose__setSubject$, AppCompose__op_Dynamic$JQuery_JQuery_, AppCompose__main$, AppCompose__log_info$String_String, AppCompose__log_enable$Unit_Unit_, AppCompose__jq$, AppCompose__hello$String_String;
AppCompose__hello$String_String = (function(a)
{
    app.showNotification('F# App Notification',a);
});
AppCompose__jq$ = (function(selector)
{
    return ((window.$)(selector));
});
AppCompose__log_enable$Unit_Unit_ = (function(b)
{
    log.enableAll();
});
AppCompose__log_info$String_String = (function(a)
{
    log.info(a);
});
AppCompose__main$ = (function(unitVar0)
{
    var _1;
    AppCompose__log_enable$Unit_Unit_(_1);
    AppCompose__log_info$String_String("application started");
    var ignored0 = (AppCompose__op_Dynamic$JQuery_JQuery_((function(selector)
    {
      return AppCompose__jq$(selector);
    }), "helloWorld").click((function(_arg1)
    {
      AppCompose__log_info$String_String("button clicked");
      return AppCompose__hello$String_String("Clicked!");
    })));
    var _ignored0 = (AppCompose__op_Dynamic$JQuery_JQuery_((function(selector)
    {
      return AppCompose__jq$(selector);
    }), "set-subject").click((function(_arg2)
    {
      AppCompose__log_info$String_String("button clicked");
      return AppCompose__setSubject$("Hello world!");
    })));
});
AppCompose__op_Dynamic$JQuery_JQuery_ = (function(jq,name)
{
    return jq(("#" + name));
});
AppCompose__setSubject$ = (function(a)
{
    var subjObj = ((Office.cast.item.toItemCompose((((Office.context).mailbox).item))).subject);
    return (subjObj.setAsync(a));
});
AppCompose__main$()
    });

})();
    
