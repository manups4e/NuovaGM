var resname;
$(function () {
	var characters = [];
	var freeSlot = 0;

	window.addEventListener('message', function (event) {
		if (event.data.resname) {
			resname = event.data.resname;
			return;
		}
		if (event.data.type == "toggleMenu") {
			var menuStatus = event.data.menuStatus;
			var menu = event.data.menu;
			// show document
			document.body.style.display = menuStatus ? "block" : "none";
			if (menu == "") return;
			var selected;
			$('.container').show();
			$('.create').hide();
			$('.characters').show();
			$('#select').show();
			$('#delete').show();
			$('a[href="http://'+ resname +'/new-character"]').hide();
			$('a[href="http://'+ resname +'/new-character-submit"]').hide();
			$("#go").show();
			$("#birth").hide();
			$("#spawn-select-back").hide();
			$(".spawn-select").hide();
			$("#badly-placed-spawn-select-header").hide();
			$('a[href="http://'+ resname +'/disconnect"]').show();
			$('#back').hide();
			$('.characters #character').remove();

			// event.data.characters = event.data.data.reverse();
			event.data.characters = JSON.parse(event.data.data);
			characters = event.data.characters;

			$('#new').on('click', function () {
				$('.create').show();
				$('.characters').hide();
				$('#select').hide();
				$('#delete').hide();
				$('#go').hide();
				$('#birth').show();
				$('#back').show();
				$('a[href="http://'+ resname +'/new-character-submit"]').show();
				$('a[href="http://'+ resname +'/disconnect"]').hide();
				$('a[href="http://'+ resname +'/list"]').show();

				$('.option p').on("click", function () {
					clicked = this;
					$('.option p').each(function () {
						if (clicked != this) $(this).removeClass("selected");
					})
					$(this).addClass("selected");
				})

				if (typeof characters != 'undefined') {
					for (var x = 0; x < characters.length; x++) {
						freeSlot = x;
						const character = characters[freeSlot];
						if (!character.firstName) break;
					}
				}

				$('#birth').on('click', function () {
					var firstname = $("#first_name").val();
					var lastname = $("#last_name").val();
					var date = new Date($("#date_of_birth").val());
					dt = (date.getDate() < 10 ? '0' : '') + date.getDate();
					mn = ((date.getMonth() + 1) < 10 ? '0' : '') + (date.getMonth() + 1);
					yy = date.getFullYear();
					var dob = dt + '/' + mn + '/' + yy;
					var sex = $("input[name='gender']:checked").val();
	
					$.post('http://'+ resname +'/new-character', JSON.stringify({
						nome:firstname,
						cogn:lastname,
						dob:dob,
						sesso:sex,
					}));
					$('.container').hide();
				})
			});

			$('#back').on('click', function () {
				$.post('http://'+ resname +'/back-indietro');
			});

			if (!event.data.characters) return;

			if (event.data.characters.length == 3) {
				var t = 0;
				for (var i = 0; i < event.data.characters.length; i++) {
					if (event.data.characters[i].info.firstname) t++;
				}
				if (t === 3) $('#new').css("display", "none");
			}

			$.each(event.data.characters.sort().reverse(), function (key, value) {
				var info = JSON.parse(value.info);
				if (!info.firstname) return;
				var div = document.createElement("div");
				div.id = "character"
				div.classList.add('character');
				div.dataset.selected = key;
				var first_name = document.createElement("h3");
				first_name.innerHTML = "<b>Nome:</b> " + info.firstname
				div.appendChild(first_name);
				var last_name = document.createElement("h3");
				last_name.innerHTML = "<b>Cognome:</b> " + info.lastname
				div.appendChild(last_name);
				var cash = document.createElement("h3");
				cash.innerHTML = "<b>Soldi:</b> <span>$</span>" + (value.Money).toLocaleString('it');//.formatMoney(2, '.', ',');
				cash.style = "padding-top: 25px"
				div.appendChild(cash);
				var bank = document.createElement("h3");
				bank.innerHTML = "<b>Banca:</b> <span>$</span>" + (value.Bank).toLocaleString('it');//.formatMoney(2, '.', ',');
				div.appendChild(bank);
				var dob = document.createElement("h3");
				dob.innerHTML = "<b>Data di nascita:</b> " + info.dateOfBirth;
				dob.style = "padding-top: 25px";
				div.appendChild(dob);
				$('.characters').prepend(div);
			});

			$('.characters #character').on("click", function () {
				clicked = this;
				$(this).addClass("selected");
				$("button#select").removeClass("disabled");
				$("button#delete").removeClass("disabled");
				$('.characters #character').each(function () {
					if (clicked != this) $(this).removeClass("selected");
				})
				selected = this.dataset.selected;
			})

			var selected_spawn = "Paleto Bay";
			$("button#disconnect").on('click', function () {
				$.post('http://'+ resname +'/disconnect');
			});
			$("button#select").on('click', function () {
				if (!$("button#select").hasClass("disabled")) {
					$.post('http://'+ resname +'/previewChar', JSON.stringify({
						id: characters[parseInt(selected)].ID
					}))
				};
				$("button#go").removeClass("disabled");
				/* store which character index was selected */

				/* CAMBIARE E AGGIUNGERE DAI PLAYER E ANTEPRIMA*/

				/* vado ai dettagli del personaggio 
				$('.characters').hide();
				$('#select').hide();
				$('#delete').hide();
				$('a[href="http://'+ resname +'/disconnect"]').hide();
				*/

				/* go to spawn selection screen 
				$('.characters').hide();
				$('#select').hide();
				$('#delete').hide();
				$('a[href="http://'+ resname +'/disconnect"]').hide();

				$('.spawn-select').show();
				$('#go').show();
				$("#spawn-select-back").show();
				$("#badly-placed-spawn-select-header").show();

				if (characters[selected].spawn) {
					$("#spawn-point--saved section").text("Saved Location");
					$("#spawn-point--saved").show();
				} else {
					$("#spawn-point--saved").hide();
				}
				*/
			})

			$("button#go").on('click', function() {
				if (!$(this).hasClass("disabled")) {

					$.post('http://'+ resname +'/char-select', JSON.stringify({
						id: characters[parseInt(selected)].ID
					}));
					$('.container').hide();
				}
			});

		} else if (event.data.type == "error") {
			document.getElementsByClassName('notification')[0].style.display = "block";
		}
	});
});
