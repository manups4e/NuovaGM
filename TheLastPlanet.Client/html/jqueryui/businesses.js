var blockfunds = false;

$(function(){
  var manageid = 0
  
  window.addEventListener("message", function(event){
    var item = event.data;

    if (item.showManager){
      manageid = item.manageid;
      $("#stationname").html(item.stationname);
      $("#thanksmessage").html(item.thanksmessage);
      $("#fuelcost").html(item.fuelcost);
      $("#sellname").val("");
      $("input[type=radio][name=deltype][value=" + item.deltype + "]").prop("checked", true).change();

      if (item.deltype == 3){
        $("#deliverycontainer").show();
      }
      else{
        $("#deliverycontainer").hide();
      }

      $("#stationmoney").html("$" + item.funds);
      processDeliveryList(item.deliverylist);
      $("#maincontainer").show();
    }
    else if (item.closeManager){
      $("#maincontainer").hide();
    }
    else if (item.setStatus){
      $("#statustext").html(item.text);
      blink("#statustext");
      blockfunds = false;
    }
    else if (item.setFunds){
      $("#stationmoney").html("$" + item.stationmoney);
	  blockfunds = false;
	}
  });

  $("#applychange").click(function(){
    $("#statustext").html("");

    if ($("#stationname").val() == ""){
      $("#statustext").html("Devi inserire il nome della stazione prima di uscire!");
      blink("#statustext");
    }
    else if ($("#fuelcost").val() == ""){
      $("#statustext").html("Devi inserire il prezzo del carburante prima di uscire!");
      blink("#statustext");
    }
    else if ($("input[type-radio][name=deltype]:checked").val() == 3 && $("#deliverylist").find("font").length == 0){
      $("#statustext").html("Devi inserire almeno un nome con questo tipo di consegna.");
      blink("#statustext");
    }
    else{
      var deliverylist = "";
      
      if ($("input[type=radio][name=deltype]:checked").val() == 3){
        $("#deliverylist").find("font").each(function(i){
          if ($(this).data("item")){
            deliverylist = deliverylist + $(this).data("item") + ";";
          }
        });        
      }

      console.log("Delivery list: " + deliverylist);

      if (parseInt($("#delbonus").val()) < 0){
        $("#delbonus").val("0");
      }
      
      sendData("lprp:businesses:manage", {manageid: manageid, stationname: $("#stationname").val(), thanksmessage: $("#thanksmessage").val(),fuelcost: $("#fuelcost").val(),
        deltype: $("input[type=radio][name=deltype]:checked").val(), deliverylist: deliverylist, delbonus: parseInt($("#delbonus").val())});
      $("#maincontainer").hide();
    }
  });

  $("#sellstation").click(function(){
    if ($("#sellname").val() == ""){
      $("#statustext").html("Devi inserire il nome di un personaggio per vendere questa stazione.");
      blink("#statustext");
    }
    else{
      sendData("lprp:businesses:sellstation", {sellname: $("#sellname").val(), manageid: manageid});
      
    }
  });

  $("#exitmanager").click(function(){
    $("#statustext").html("");
    $("#maincontainer").hide();
    sendData("menuclosed", "");
  });

  $("input[type=radio][name=deltype]").change(function(){
    if (this.value == "3"){
      $("#deliverycontainer").show();
    }
    else{
      $("#deliverycontainer").hide();
    }
  });

  $("#addplayerbutton").click(function(){
    var item = $("#addplayertolist").val();
    
    if (item == ""){
      $("#statustext").html("Devi inserire il nome di un personaggio.");
      blink("#statustext");
    }
    else{
      $("#statustext").html("");
      $("#addplayertolist").val("");
      $("#deliverylist").append('<div id="litem"><font class="listresult" data-item="' + item + '">' + item + '<font class="listdelbutton" data-sel="' + item + '">X</font></font></div>');
    }
  });

  $("#addfundsbutton").click(function(){
    if (!blockfunds){
      var amount = parseInt($("#addfunds").val());

      if (amount > 0){
        blockfunds = true;
        $("#addfunds").val("0");
        sendData("lprp:businesses:addstationfunds", {manageid: manageid, amount: amount});
      }
    }
  });

  $("#remfundsbutton").click(function(){
    if (!blockfunds){
      var amount = parseInt($("#remfunds").val());

      if (amount > 0){
        blockfunds = true;
        $("#remfunds").val("0");
        sendData("lprp:businesses:remstationfunds", {manageid: manageid, amount: amount});
      }
    }
  });

  $(document).on("click", ".listdelbutton", function(){
    var selected = $(this).data("sel");

    console.log(selected);
    
    $("#deliverylist").find("font").each(function(i){
      if ($(this).data("item") == selected){
        //$(this).remove();
        $(this).closest("div").remove();
      }
    });
  });
});

function sendData(name, data){
    $.post("http://tlp/" + name, JSON.stringify(data), function(datab) {
        console.log(datab);
    });
}

function playSound(sound){
    sendData("playsound", {name: sound});
}

function processDeliveryList(plist){
  if (plist){
    $("#deliverylist").children().detach();
    
    var fmtstr = "";
    var Players = plist.split(";");
    
    for (var i = 0; i < Players.length; i++){
      fmtstr = fmtstr + '<div id="litem"><font class="listresult" data-item="' + Players[i] + '">' + Players[i] + '<font class="listdelbutton" data-sel="' + Players[i] + '">X</font></font></div>';
    }

    //fmtstr.slice(0, -5); // remove trailing <br/>

    $("#deliverylist").append(fmtstr);
  }
}

function blink(selector){
  $(selector).fadeOut('slow', function(){
      $(this).fadeIn('slow', function(){
          blink(this);
      });
  });
}