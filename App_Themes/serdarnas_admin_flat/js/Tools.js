/// <reference path="jquery-1.4.1-vsdoc.js" />


///////////////
// Alert Box //
///////////////

function ShowAlert(tip, message) {

    var className = "error_box";
    if (tip == 1)
        className = "warning_box";
    else if (tip == 2)
        className = "valid_box";

    var item = '<div class="' + className + '"> ' + message + '  </div>';
    var itemDiv = $(item);
    $("div.right_content").prepend(itemDiv);

    itemDiv.fadeOut(8000, function () { $(this).remove(); });
}
function ShowErrorBox(message) {
    ShowAlert(0, message);
}
function ShowWarningBox(message) {
    ShowAlert(1, message);
}
function ShowValidBox(message) {
    ShowAlert(2, message);
}

///////////////
///Side Box ///
///////////////

function ShowSideBox(tip, header, message, close) {


    var boxHtml = "";
    boxHtml = '<div class="sidebar_box">';
    boxHtml += '  <div class="sidebar_box_top"></div>';
    boxHtml += '  <div class="sidebar_box_content">';
    if (tip == 1) {
        boxHtml += ' <h4>' + header + '</h4>';
        boxHtml += '     <img src="../Style/images/notice.png" alt="" title="" class="sidebar_icon_right" />';

    }
    else {
        boxHtml += ' <h3>' + header + '</h3>';
        boxHtml += '     <img src="../Style/images/info.png" alt="" title="" class="sidebar_icon_right" />';

    }
    boxHtml += '      <p>' + message + '</p>';
    boxHtml += '   </div>';
    boxHtml += '   <div class="sidebar_box_bottom"></div>';
    boxHtml += '</div>';
    var boxDiv = $(boxHtml);

    $("div.left_content").append(boxDiv);
    if (close)
        boxDiv.fadeOut(10000, null);

}

////////////////////////////////////////
///////Fuar Ekleme//////////////////////
////////////////////////////////////////
function AddDate() {
    var date = dateEdit.GetText();
    if (date.length > 0) {
        if (lBoxDate.FindItemByText(date) == null)
            lBoxDate.AddItem(date);
    }

    return false;
}
function RemoveDate() {
    lBoxDate.RemoveItem(lBoxDate.GetSelectedIndex());
}
function popUp(URL) {
    day = new Date();
    id = day.getTime();

    eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=1,statusbar=0,menubar=0,resizable=1,width=650,height=500,left = 395,top = 200');");
}

function ShowFileManager(sender) {
    popUp('FileManager.aspx');

}
function ShowFileFlashManager(sender) {
    popUp('FileManager.aspx?path=flash');

}
function SelectFile() {
    var file = fileManager.GetSelectedFile();
    if (file != null) {
        opener.tBoxLogo.SetValue('~/' + fileManager.GetSelectedFile().GetFullName().replace('\\', '/'));

        window.close();
    }
    else
        alert("Seçim Yapınız.");
}


