﻿var unityObjectUrl = "http://webplayer.unity3d.com/download_webplayer-3.x/3.0/uo/UnityObject2.js";
var config = {
    width: 660,
    height: 400,
    params: { enableDebugging: "0" }
};
var BookModel = {
    ChapterId: "",
    PageId: "",
    Target: "",
    FrameId: "",
    Objects: [],
    Frames: []
};

var Objects = {
    ObjectId: "",
    ImageObj: "",
    ScaleX: "",
    ScaleY: "",
    Origo: "",
    Type: ""
};

var Frames = {
    Bordertype: ""
};

var dialogOpts = {
    position: [300, 5],
    minWidth: 600,
    maxWidth: 600,
    maxHeight: 700,
    minHeight: 700,
    width: 600,
    height: 700,
    close: function() {

    },
    open: function() {
        configurateImgOnTerrain();
    }
};


var draggableId = "No Draggable";
var droppableId = "No droppable";
var actualFrame;
var actualPage;
var actualTarget;
var actualContent;
var actualImg;

var u = new UnityObject2(config);

jQuery(function() {

    var $missingScreen = jQuery("#unityPlayer").find(".missing");
    var $brokenScreen = jQuery("#unityPlayer").find(".broken");
    $missingScreen.hide();
    $brokenScreen.hide();

    u.observeProgress(function(progress) {
        switch (progress.pluginStatus) {
            case "broken":
                $brokenScreen.find("a").click(function(e) {
                    e.stopPropagation();
                    e.preventDefault();
                    u.installPlugin();
                    return false;
                });
                $brokenScreen.show();
                break;
            case "missing":
                $missingScreen.find("a").click(function(e) {
                    e.stopPropagation();
                    e.preventDefault();
                    u.installPlugin();
                    return false;
                });
                $missingScreen.show();
                break;
            case "installed":
                $missingScreen.remove();
                break;
            case "first":
                break;
        }
    });
    u.initPlugin(jQuery("#unityPlayer")[0], "/Content/Resources/WebPlayer/webviewer.unity3d?book=2");

});


$(function() {
    $('.bb-bookblock').booklet({
        width: '65%',
        manual: 'false',
        overlays: true,
        pagePadding: 0,
        height: 600,
        speed: 500,
        tabs: true,
        tabWidth: 180,
        tabHeight: 20
    });
    //   $(".rectangle").jqte();
    $("#accordionConfig").accordion();
    $("#accordionStatus").accordion();
    //$('#userContent').load('/Book/GetAvailableBooks', { fileOption: "newBook" });
    $(".Container3D").addClass("disableControl");
    if ($("#mssgString").html() != '')
        showStatusMssg();

    activateEditorOperations();

});
/*Show status message*/

function showStatusMssg() {
    $("#statusDialog").dialog({
        modal: true,
        buttons: {
            Ok: function() {
                $(this).dialog("close");
            }
        }
    });
}

/*Edit on place*/

$('.addBook').editable('/Book/AddNewBook', {
    type: 'textarea',
    name: 'newFileName',
    submit: 'OK',
    callback: function(value, settings) {
        if (value.systemMssg == "") {
            $("#partialFileResult").html(result.ObjectInquiryView);
            $("#mssgString").html("SUCCES:You create a new book");
        } else {
            $("#mssgString").html("ERROR: create a new book");
        }
        showStatusMssg();

        // alert("value " + value );
    }
});

/*Tree structure for fils and directories*/
$(function() {
    $("#objectsGroup").jstree({
        "plugins": ["themes", "html_data", "contextmenu"],
        "contextmenu": {
            "items": {
                remove: {
                    label: "Delete location",
                    action: function(obj) {
                        alert("Delete Object");
                    }
                },
                "ccp": false,
                "create": false,
                "rename": false
            }
        }
    });
    $("#characterGroup").jstree({
        "plugins": ["themes", "html_data"]
    });
    $("#expressionGroup").jstree({
        "plugins": ["themes", "html_data"]
    });
    $("#userContent").jstree({
        "plugins": ["themes", "html_data", "contextmenu", "types"],
        'types': {
            'types': {
                'file': {
                    'icon': {
                        'image': '/Images/file.png'
                    }
                },
                'default': {
                    'valid_children': 'default'
                }
            }
        },
        "contextmenu": {
            "items": {
                remove: {
                    label: "Delete location",
                    action: function(obj) {
                        $.ajax({
                            url: "/Book/DeleteBook",
                            type: 'POST',
                        data: {
                            "fileToDelete": obj.text()
                        },
                        success: function(result) {

                        },
                        error: function(result) {
                            $("#mssgString").html("Error:Couldn't remove element");
                            showStatusMssg();
                        }
                    });
}
},
                        "ccp": false,
                        "create": false,
                        "rename": false
}
}
});
});


/*Get selectec item of dropdownbox*/
$(".selectpicker").change(function() {
    $('#userContent').load('/Book/GetAvailableBooks', { fileOption: "loadBook" });
});

function setBookModel(chapterId, pageId, frameId, target) {
    BookModel.Objects = [];
    BookModel.ChapterId = chapterId,
    BookModel.PageId = pageId;
    BookModel.FrameId = frameId,
    BookModel.Target = target;
}

function getBookModel() {
    return BookModel;
}

function setObjectModel(objectId, imageObj, scaleX, scaleY, origo, type) {
    BookModel.Objects.push({
        ObjectId: objectId,
        ImageObj: imageObj,
        ScaleX: scaleX,
        ScaleY: scaleY,
        Origo: origo,
        Type: type
    });
}

function SetFrameModel(bordertype) {
    BookModel.Frames.push({
        Bordertype: bordertype
    });
}

/*Check html element is empty*/

function CheckIsEmpty(element) {
    return $.trim($("#" + element).html()).length
}

/*Add img on content in frame*/
$('#objectsGroup img').bind("click", function() {
    var randomnumber = Math.floor(Math.random() * 100);
    $('#' + actualContent + " .contentIntern").append('<img width="35" height="70" style="left: 50%;top: 55%;" class="clonedImg" id="Cloned' + randomnumber + '" src="' + $(this).attr("src") + '" />');
    var valuesId = [];
    configurateImgOnTerrain(false);
    if (actualContent != null) {
        valuesId = GetIdFromString(actualContent);
        setBookModel(valuesId[0], valuesId[1], valuesId[2], valuesId[3]);
        setObjectModel("Object" + randomnumber, $(this).attr("src"), null, null, null, null);
        PostArray("AddObjectToContent", getBookModel());
    }
});
/*Add img on content in frame*/
$('#characterGroup img').bind("click", function() {
    var randomnumber = Math.floor(Math.random() * 1000);
    $('#' + actualContent + " .contentIntern").append('<img width="35" height="70" style="left: 50%;top: 55%;" class="clonedImg" id="Character' + randomnumber + '" src="' + $(this).attr("src") + '" />');
    var valuesId = [];
    configurateImgOnTerrain(false);
    if (actualContent != null) {
        valuesId = GetIdFromString(actualContent);
        setBookModel(valuesId[0], valuesId[1], valuesId[2], valuesId[3]);
        setObjectModel("Character" + randomnumber, $(this).attr("src"), null, null, null, null);
        PostArray("AddCharacter2DToContent", getBookModel());
    }
});
/*Add img on content in frame*/
$('#expressionGroup img').bind("click", function() {
    var randomnumber = Math.floor(Math.random() * 1000);
    $('#' + actualContent + " .contentIntern").append('<img width="25" height="50" style="left: 50%;top: 55%;" class="clonedImg" id="Expression' + randomnumber + '" src="' + $(this).attr("src") + '" />');
    // alert($(this).attr("src"));
    var valuesId = [];
    configurateImgOnTerrain(true);
    if (actualContent != null) {
        valuesId = GetIdFromString(actualContent);
        setBookModel(valuesId[0], valuesId[1], valuesId[2], valuesId[3]);
        setObjectModel("Expression" + randomnumber, $(this).attr("src"), null, null, null, null);
        PostArray("ExpressionToContent", getBookModel());
    }
});

/*Delete object in content*/
$("#ctxMenuDelete").click(function() {
    var objId = $("#valCtxMenu").html();
    var valuesId = [];
    if (actualContent != null) {
        $("#" + actualContent + " .contentIntern").find('img').each(function() {
            if (objId == $(this).attr("id")) {
                $(this).remove();
                //alert(actualContent);
                valuesId = GetIdFromString(actualContent);
                setBookModel(valuesId[0], valuesId[1], valuesId[2], valuesId[3]);
                setObjectModel(objId, null, null, null, null, null);
                PostArray("DeleteObjectFromContent", getBookModel());
            }
            destroyContextMenu();
        });
    } else {
        $("#mssgString").html("ERROR.Can't find element");
        showStatusMssg();

    }
});
/*Start drag effect on element*/

function startDragNoLimit(element) {
    $(element).draggable({
        cursor: 'move',
        containment: "parent",
        drag: function(event, ui) {
            draggableId = $(this).attr("id");

        }
    });
}

/*Strat drag effect on element*/

function startDrag(element) {
    $(element).draggable({
        axis: "x",
        cursor: 'move',
        containment: "parent",
        drag: function(event, ui) {
            draggableId = $(this).attr("id");
            //  alert("StartDrag");
            var offset = $(this).offset();
            var imgHeight = parseInt($("#" + draggableId).css("height"), 10);
            var xPos = offset.left;
            var yPos = offset.top - imgHeight;
            $('#posX_txtbox').val('xw: ' + xPos);
            $('#posY_txtbox').val('yw: ' + yPos);
        }
    });
}

/*Activate editor for frame*/

function activateEditorOperations() {
    $(".editPencil").click(function(e) {
        $(".squareLeft").css({ "-webkit-border-image": "url(/Content/Resources/Images/border.png) 25 25 round" });
        $(".squareRight").css({ "-webkit-border-image": "url(/Content/Resources/Images/border.png) 25 25 round" });
        $(".rectangle").css({ "-webkit-border-image": "url(/Content/Resources/Images/border.png) 25 25 round" });
        $(this).parent().css({ "-webkit-border-image": "url(/Content/Resources/Images/border.png) 8 8 round" });
        if ($("#" + actualContent + " .droppable img") != 'undefined') {
            $("#" + actualContent + " .droppable img").draggable("destroy");
        }
        actualContent = $(this).parent().attr("id");
        configurateImgOnTerrain();
    });
}

/*Add some more functions to added image on content frame*/

function configurateImgOnTerrain(status) {
    $('#' + actualContent + ' .droppable img').each(function() {

        if (status) {
            startDragNoLimit($(this));
        } else {
            startDrag($(this));
        }

        $(this).bind('contextmenu', function(e) {
            $('#context-menu').css('left', e.pageX + 'px');
            $('#context-menu').css('top', e.pageY + 'px');
            $('#context-menu').css('z-index', 10);
            $("#valCtxMenu").html($(this).attr("id"));
            $('#context-menu').show();
            e.preventDefault();
            return false;
        });
    });
}

/*Change background image of terrain*/
$(".backgroundContain").click(function() {
    //  var descriptionArray = [];
    var valuesId = [];
    var bk = $(this).attr("id");
    var imgChosen = $("#" + bk).attr("src");

    if (actualContent != null) {
        valuesId = GetIdFromString(actualContent);
        setBookModel(valuesId[0], valuesId[1], valuesId[2], valuesId[3]);
        setObjectModel(bk, $(this).attr("src"), null, null, null, null);
        $("#" + actualContent + " .contentIntern").css("background-image", "url(" + imgChosen + ")");
        PostArray("AddBackgroundToFrame", getBookModel());

    }
});

/*Add new page*/
$("#btnAddPage").click(function() {
    //var descriptionArray = [];
    setBookModel("chapter1", null, null, null);
    PostArray("AddPage", getBookModel());
    reloadPage();
});


/*Function handle draggable objects. 
             Objects you pickup and drag*/
 
$("#framesGroup .draggable").draggable({
    cursor: 'move',
    helper: 'clone',
    drag: function(event, ui) {
        draggableId = $(this).attr("id");
        //alert("group");
    }
}).addClass("ui-state-highlight");
     
/*Function handle draggable objects. 
                 Objects you pickup and drag*/
         
$("#txtCompGroup .draggable").draggable({
    cursor: 'move',
    helper: 'clone',
    drag: function(event, ui) {
        draggableId = $(this).attr("id");
        // alert(draggableId);
    }
}).addClass("ui-state-highlight");
         

       


/*Function handle droppable frames objects. 
                  Object where you put your draggable*/
$(".frame .droppable").droppable({
    drop: function(event, ui) {
        var htmlContent = "<div class='editPencil'><span class='ui-icon ui-icon-pencil'></span></div>"
            +
            "<div class='droppable contentIntern ui-droppable' style='background-image: url(/Content/Resources/Images/rectangle.png)'></div>";
                    
        if ((draggableId == "rectangle") || (draggableId == "square")) {
                 
            //Add frames in page
            var valuesId = [];
            var originalId = $(this).find(".GeomForm :first-child").attr("id");
            //alert(originalId);
            valuesId = GetIdFromString(originalId);
            setBookModel(valuesId[0], valuesId[1], valuesId[2], draggableId);

            PostArray("AddFrame", getBookModel());
            $(this).find("div").html("");
            if (draggableId == "rectangle") {
                var rectangleId = SplitAndConcanate(originalId, "rectangle");
                $(this).find("div").prepend("<div  id=" + rectangleId + " class='rectangle'>" + htmlContent + "</div>");
            } else if (draggableId == "square") {
                var squareLeftId = SplitAndConcanate(originalId, "left");
                var squareRightId = SplitAndConcanate(originalId, "right");
                $(this).find("div").prepend("<div  id=" + squareLeftId + " class='squareLeft'>" + htmlContent + "</div><div  id=" + squareRightId + " class='squareRight'>" + htmlContent + "</div>");
            } else if (draggableId == "txtBox") {
                alert("droppable contenIntern");
            }
            activateEditorOperations();

            $("#" + draggableId).css({ position: "relative", bottom: 0, left: 0 });
            $("#" + draggableId).show();
        } else if (draggableId == "txtBox") {

            // alert($(this).attr("id"));
            // alert($(this).children('div').children('div').children('div').eq(1).attr("class"));
            var parentId = $(this).children('div').children('div').eq(0).attr("id");
            $(this).children('div').children('div').children('div').eq(0).html('');
            $(this).children('div').children('div').children('div').eq(1).find('img').each(function() {
                $(this).remove();
            });
            $(this).children('div').children('div').children('div').eq(1).find('.bigText').remove();
            $(this).children('div').children('div').children('div').eq(1).prepend(" <div class='bigText' id='" + parentId + "-div_text'>Click here to edit..</div>");
            $('.bigText').editable('/Book/AddTextToContent', {
                type: 'textarea',
                cancel: 'Cancel',
                name: 'model',
                id: 'componentId',
                submit: 'OK',
                indicator: '<img src="/Content/Resources/Images/home-ajax-loader.gif">',
                tooltip: '<<Click to edit...'
            });
            //children('div').eq(2).children('img').attr("id"));
        }
        else {
            // alert( $("#"+draggableId).parent().parent().attr("id"));
            //  var descriptionArray = [];
            valuesId = GetIdFromString($("#" + draggableId).parent().parent().attr("id"));
            setBookModel(valuesId[0], valuesId[1], valuesId[2], valuesId[3]);
            setObjectModel(draggableId, null, null, null, null, null);
            //descriptionArray.push(actualContent + "-" + $($(this).find("img").attr("id")).attr("src"));
            // PostArray("UpdateObjectPosition", GetBookModel());
        }
    }
});
   

function GetIdFromString(stringToSplit) {
    var arrayId = stringToSplit.split('-');
    return arrayId;
}

/*Start Context menu events*/
$('html').click(function(e) {
    $('#context-menu').hide();
    $("#valCtxMenu").text('');
    e.stopPropagation();
});
$('#context-menu').click(function(e) {
    $("#valCtxMenu").text('');
    e.stopPropagation();
});
$(window).resize(function() {
    $('#context-menu').hide();
    $("#valCtxMenu").text('');
});

function destroyContextMenu() {
    $('#context-menu').hide();
    $("#valCtxMenu").text('');
}

/*End of contextmenu events*/


/*Change Id name of element and replace*/

function SplitAndConcanate(stringToSplit, valueToInsert) {
    var splitString = stringToSplit.split('-');
    var result;
    splitString[splitString.length - 1] = valueToInsert;
    result = splitString.join('-');
    return result;
}

/*Toggle between 2D and 3D View*/
/*----------------------------------------------*/
$("#View3D").bind("click", function() {
    $(".pages #frames").each(function(i, elem) {
        $(this).hide(500);
    });
    $("#Container3D").appendTo($("#pages"));
    $(".Container3D").removeClass("disableControl");
    $(".Container3D").addClass("activeControl");
});

$("#View2D").bind("click", function() {
    $(".pages #frames").each(function(i, elem) {
        $(this).show(500);
    });
    $(".Container3D").removeClass("activeControl");
    $(".Container3D").addClass("disableControl");
});
/*--------------------------------------------*/


function reloadPage() {
    $("body").fadeOut(
        function() {
            location.reload(true);
            $(document).ready(function() { $(body).fadeIn(); });
        });
}

/*All post data to controller go thorugh this function*/

function PostArray(url, descriptionArray) {
    jQuery.ajax({
        type: 'POST',
        url: url,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(BookModel),
        async: false,
        dataType: "json",
        success: function(statusMsg) {
            descriptionArray.length = 0;
        },
        error: function(statusMsg) {
            $("#mssgString").html("ERROR.Couldn't send message.Wrong parameters");
            showStatusMssg();
        }
    });
}