var unityObjectUrl = "http://webplayer.unity3d.com/download_webplayer-3.x/3.0/uo/UnityObject2.js";
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
    OrigoX: "",
    OrigoY:"",
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
var zIndexCounter = 1000;

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


$(document).ready(function () {
    $('.bb-bookblock').booklet({
        width: '65%',
        manual: 'false',
        hoverWidth:0,
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
    $('.bigText').bind("click", "tom", editableBox);
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

function setObjectModel(objectId, imageObj, scaleX, scaleY, origoX,origoY, type) {
    BookModel.Objects.push({
        ObjectId: objectId,
        ImageObj: imageObj,
        ScaleX: scaleX,
        ScaleY: scaleY,
        OrigoX: origoX,
        OrigoY:origoY,
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
    return $.trim($("#" + element).html()).length;
}


/*Add img on content in frame*/
$('#txtCompGroup #txtBox').bind("click", function () {
    if (actualContent == null) {
        showInfoMessage("Click on <div border='1'><span class='ui-icon ui-icon-pencil'></span></div>  to edit a content!!");
    } else

        if (actualContent != null) {
            //   alert(actualContent);
            $("#" + actualContent).children('div').eq(1).find('.bigText').remove();
            $("#" + actualContent).children('div').eq(1).find('img').each(function () {
                $(this).remove();
            });
            $("#" + actualContent).children('div').eq(1).prepend(" <p class='bigText' contenteditable='true' id='" + actualContent + "-div_text'>Click here to edit..</p>");
            $('.bigText').bind("click", { componentId: actualContent }, function (evento) {
                var data = evento.data;
                editableBox(data.componentId,"textBox");
            });
        } else {
            showInfoMessage("Click on <div border='1'><span class='ui-icon ui-icon-pencil'></span></div>  to edit a content!!");
        }
});
/*Add img on content in frame*/
$('#txtCompGroup .bubbleText').bind("click", function () {
    var objId=$(this).attr("id");
    if (actualContent == null) {
        showInfoMessage("Click on <div border='1'><span class='ui-icon ui-icon-pencil'></span></div>  to edit a content!!");
    } else {
        AddSpeechBubble("speechBubble", $(this));
    }
});

/*Add img on content in frame*/
$('#objectsGroup img').bind("click", function () {
    
    if (actualContent == null) {
        showInfoMessage("Click on <div border='1'><span class='ui-icon ui-icon-pencil'></span></div>  to edit a content!!");
    } else
    AddObject("CharacterRes", $(this));
});

/*Add img on content in frame*/
$('#characterGroup img').bind("click", function () {
    if (actualContent == null) {
        showInfoMessage("Click on <div border='1'><span class='ui-icon ui-icon-pencil'></span></div>  to edit a content!!");
    }else
    AddObject("Character2DRes", $(this));

});
/*Add img on content in frame*/
$('#expressionGroup img').bind("click", function () {
    if (actualContent == null) {
        showInfoMessage("Click on <div border='1'><span class='ui-icon ui-icon-pencil'></span></div>  to edit a content!!");
    } else
    AddObject("ExpressionRes", $(this));
});

function editableBox(componentId,boxType,form) {
    // alert(componentId);
    $('.bigText').editable('/Book/AddTextToContent', {
        type: 'textarea',
        cancel: 'Cancel',
        name: 'model',
        id: 'componentId',
        submit: 'OK',
        submitdata:function (value,settings) {
            return {type:boxType,form:form};
        },
        data: function (value, settings) {
            var retval = value.replace(/<br[\s\/]?>/gi, '\n');
            //var retval = value;
            return retval;
        },
        indicator: '<img src="/Content/Resources/Images/home-ajax-loader.gif">',
        tooltip: 'Click to edit...'
    });
}


function AddObject(type, element) {
    //alert(draggableId);
    var parentId = $("#" + draggableId).parent().parent().attr("id");
    zIndexCounter++;
   //alert(zIndexCounter);
    var origoX = "0%";
    var origoY = "0%";
    var scaleX = "10%";
    var scaleY = "25%";
    var valuesId = [];
    var randomnumber = Math.floor(Math.random() * 100);
    if (actualContent != null) {
        valuesId = GetIdFromString(actualContent);
        setBookModel(valuesId[0], valuesId[1], valuesId[2], valuesId[3]);
  
        $('#' + actualContent + " .contentIntern").append('<img width="' + scaleX + '" height="' + scaleY + '"  style="left: "' + origoX + '";top: "' + origoY + '";" class="clonedImg" id="Object' + randomnumber + '" src="' + element.attr("src") + '" />');
    configurateImgOnTerrain(true);}
    
        setObjectModel("Object" + randomnumber, element.attr("src"),scaleX, scaleY, origoX, origoY, type);
        PostArray("AddObjectToContent", getBookModel());
    
}
function AddSpeechBubble(type, element) {

    var parentId = $("#" + draggableId).parent().parent().attr("id");
    var origoX = "0%";
    var origoY = "0%";
    var scaleX = "10%";
    var scaleY = "25%";
    var valuesId = [];
    var randomnumber = Math.floor(Math.random() * 100);
    if (actualContent != null) {
        valuesId = GetIdFromString(actualContent);
        setBookModel(valuesId[0], valuesId[1], valuesId[2], valuesId[3]);
    
        $('#' + actualContent + " .contentIntern").append('<div class="speechContainer"><p class="smallText" contenteditable="true" id="' + actualContent + '-speech' + randomnumber + '">Click here to edit..</p></div>');
           /* $("#"+actualContent+"-speech" + randomnumber).parent().css({
                // "background": "url('../Content/Resources/Images/speechTalk.png')",
                "background": "url(" + element.attr("src") + ")",
                "background-size": "100%",
                "background-repeat": "no-repeat"
            });*/
            $('.smallText').bind("click", { componentId: actualContent, boxType: "speechBubbla", boxForm: draggableId }, function (evento) {
                var data = evento.data;
                speechTxtBox(data.componentId, data.boxType, data.boxForm);
            });
            configurateObjOnTerrain(true);

        setObjectModel(actualContent+"-speech" + randomnumber, element.attr("src"), scaleX, scaleY, origoX, origoY, type);
        PostArray("AddSpeechBubbleObject", getBookModel());
    }
}

function speechTxtBox(componentId, boxType, form) {
    // alert(componentId);
    $('.smallText').editable('/Book/AddTextToBubble', {
        type: 'textarea',
        cancel: 'Cancel',
        name: 'model',
        id: 'componentId',
        submit: 'OK',
        submitdata: function (value, settings) {
            return { type: boxType, form: form };
        },
        data: function (value, settings) {
            var retval = value.replace(/<br[\s\/]?>/gi, '\n');
            //var retval = value;
            return retval;
        },
        indicator: '<img src="/Content/Resources/Images/home-ajax-loader.gif">',
        tooltip: 'Click to edit...'
    });
}

/*Add some more functions to added image on content frame*/

function configurateObjOnTerrain(status) {
    $('#' + actualContent + ' .droppable div').each(function () {

        if (status) {
            startDragNoLimit($(this));
        } else {
            startDrag($(this));
        }
    });
}

/*Add some more functions to added image on content frame*/

function configurateImgOnTerrain(status) {
    $('#' + actualContent + ' .droppable img').each(function () {

        if (status) {
            startDragNoLimit($(this));
        } else {
            startDrag($(this));
        }

        $(this).bind('contextmenu', function (e) {
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
/*Delete book*/
$('.deleteFile').on('click',function () {
    var aFile = this.id;

    $.post("/Book/DeleteBook", { fileToDelete: aFile}, function (data) {
        $("#partialFileResult").html(data);

        //$("#userContent").jstree("refresh");
        showInfoMessage("SUCCES:You delete this book");
    }).fail(function () {
        showInfoMessage("ERROR: Couldn't delete this book.");
    });   
});

/*Add new Book*/

$('#AddNewBook_btn').bind('click', function () {
    var fileName = $("#newFileName").val();
    if (fileName == "") {
        showInfoMessage("Please! Insert a name for the file.");
    
    } else {
         $.post("/Book/AddNewBook", { newFileName: fileName }, function(data) {
              $("#partialFileResult").html(data);
              showInfoMessage("SUCCES:You create a new book");
        }).fail(function() {
            showInfoMessage("ERROR: Couldn't create a book.");
        });
 }
}); 
/*Delete object in content*/
$("#ctxMenuFlip").click(function () {
    var objId = $("#valCtxMenu").html();
    // alert(objId);
    var valuesId = [];
    if (actualContent != null) {
        $("#" + actualContent + " .contentIntern").find('img').each(function () {
            if (objId == $(this).attr("id")) {
                $(this).addClass('flipped');
                //alert(actualContent);
               // valuesId = GetIdFromString(actualContent);
                //setBookModel(valuesId[0], valuesId[1], valuesId[2], valuesId[3]);
                //setObjectModel(objId, null, null, null, null, null, null);
                //PostArray("DeleteObjectFromContent", getBookModel());
            }
            destroyContextMenu();
        });
    } else {
        showInfoMessage("ERROR.Can't find element");


    }
});

/*Delete object in content*/
$("#ctxMenuDelete").click(function() {
    var objId = $("#valCtxMenu").html();
   // alert(objId);
    var valuesId = [];
    if (actualContent != null) {
        $("#" + actualContent + " .contentIntern").find('img').each(function() {
            if (objId == $(this).attr("id")) {
                $(this).remove();
                //alert(actualContent);
                valuesId = GetIdFromString(actualContent);
                setBookModel(valuesId[0], valuesId[1], valuesId[2], valuesId[3]);
                setObjectModel(objId, null, null, null, null, null,null);
                PostArray("DeleteObjectFromContent", getBookModel());
            }
            destroyContextMenu();
        });
    } else {
        showInfoMessage("ERROR.Can't find element");


    }
});

/*Increase size object*/
$("#ctxMenuIncreaseSize").click(function() {
    var objId = $("#valCtxMenu").html();
    // alert(objId);
    var valuesId = [];
    if (actualContent != null) {
        $("#" + actualContent + " .contentIntern").find('img').each(function () {
            if (objId == $(this).attr("id")) {
                $(this).height("+=10");
                $(this).width("+=10");
            }
            destroyContextMenu();
        });
    } else {
        showInfoMessage("ERROR.Can't find element");


    }
    
});
/*Increase size object*/
$("#ctxMenuDecreaseSize").click(function () {
    var objId = $("#valCtxMenu").html();
    // alert(objId);
    var valuesId = [];
    if (actualContent != null) {
        $("#" + actualContent + " .contentIntern").find('img').each(function () {
            if (objId == $(this).attr("id")) {
                $(this).height("-=10");
                $(this).width("-=10");
            }
            destroyContextMenu();
        });
    } else {
        showInfoMessage("ERROR.Can't find element");


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
        cursor: 'move',
        containment: "parent",
        drag: function(event, ui) {
            draggableId = $(this).attr("id");
           // alert(draggableId);
            //  alert("StartDrag");
            //var offset = $(this).offset();
            //var imgHeight = parseInt($("#" + draggableId).css("height"), 10);
          //  var xPos = offset.left;
           // var yPos = offset.top - imgHeight;
           
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


/*Change background image of terrain*/
$(".backgroundContain").click(function () {
    
    //  var descriptionArray = [];
    var valuesId = [];
    var bk = $(this).attr("id");
    var imgChosen = $("#" + bk).attr("src");

    if (actualContent != null) {
        valuesId = GetIdFromString(actualContent);
        setBookModel(valuesId[0], valuesId[1], valuesId[2], valuesId[3]);
        setObjectModel(bk, $(this).attr("src"), null, null, null, null,null);
        $("#" + actualContent + " .contentIntern").css("background-image", "url(" + imgChosen + ")");
        PostArray("AddBackgroundToFrame", getBookModel());

    } else { showInfoMessage("Click on <div border='1'><span class='ui-icon ui-icon-pencil'></span></div>  to edit a content!!"); }
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
    stack:'div',
    drag: function(event, ui) {
        draggableId = $(this).attr("id");
        //alert("group");
    }
}).addClass("ui-state-highlight");
     
    


/*Function handle droppable frames objects. 
                  Object where you put your draggable*/
$(".frame .droppable").droppable({
    drop: function (event, ui) {
       // alert("löd,");
        var htmlContent = "<div class='editPencil'><span class='ui-icon ui-icon-pencil'></span></div>"
            +
            "<div class='droppable contentIntern ui-droppable' style='background-image: url(/Content/Resources/Images/rectangle.png)'></div>";
                    
        if ((draggableId == "rectangle") || (draggableId == "square")) {
                 
            //Add frames in page
            var valuesId = [];
            var originalId = $(this).find(".GeomForm :first-child").attr("id");
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
            }
            activateEditorOperations();
            $("#" + draggableId).css({ position: "relative", bottom: 0, left: 0 });
            $("#" + draggableId).show();
        } else {
   
            startDragNoLimit($("#".draggableId));
            var parentId = $("#" + draggableId).parent().parent().attr("id");
            //alert(parentId);
            var parentWidth = $("#" + parentId).width();
            var objWidth = $("#" + draggableId).width();
            var parentPosition = $("#" + parentId).position();
            var objPosition = $("#" + draggableId).position();
            var parentHeight = $("#" + parentId).height();
            var objHeight = $("#" + draggableId).height();
            var parentPosTop = parentPosition.top;
            var objPosTop = objPosition.top;
            var scaleX = getPercentage(parentWidth, objWidth);
            var scaleY = getPercentage(parentHeight, objHeight);
            var origoX = getPercentage(parentWidth, (objPosition.left-objWidth)-parentPosition.left);
            var origoY = getPercentage(parentHeight, objPosition.top - parentPosition.top);
            
            /*For debugging purpose. Shows data on Status in acccordion menu*/
            $('#parentX_txtbox').val(parentPosition.left);
            $('#parentY_txtbox').val(parentPosition.top);
            $('#posX_txtbox').val(objPosition.left);
            $('#posY_txtbox').val(objPosition.top-parentPosition.top );
            $('#pageName_txtbox').val(origoX);
            $('#frameName_txtbox').val(origoY);
            $('#parentWidth_txtbox').val(parentWidth);
            $('#parentHeight_txtbox').val(parentHeight);
            $('#objWidth_txtbox').val(objWidth);
            $('#objHeight_txtbox').val(objHeight);
            /*End of debug part*/
            
            valuesId = GetIdFromString(parentId);
            setBookModel(valuesId[0], valuesId[1], valuesId[2], valuesId[3]);
            setObjectModel(draggableId, null, scaleX, scaleY, origoX, origoY,null);
              PostArray("UpdateObjectPosition", getBookModel());
        }
    }
});

function showInfoMessage(mssg) {
    $("#mssgString").html(mssg);
    showStatusMssg();
}


function getPercentage(frameValue,objValue) {
    var percentage;
    percentage= Math.floor((objValue / frameValue) * 100);
    if (percentage > 88.0)
        percentage = 96.0;
    if (percentage < 0.0)
        percentage = 0.0;
    return (percentage).toFixed(1) + "%";
}

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
        error: function (statusMsg) {
         
            showInfoMessage("ERROR.Couldn't send message.Wrong parameters");
           
        }
    });
}