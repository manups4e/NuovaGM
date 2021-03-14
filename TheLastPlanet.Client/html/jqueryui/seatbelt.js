var chimeAudio = undefined;
var cAudio = undefined;
var sfxAudio = undefined;
var currentStatus = undefined;
var inCar = false;

window.addEventListener('message', function (e) 
{
    if (e.data.transactionType === 'isBuckled') {
        if (inCar != e.data.inCar) {
            if (!e.data.inCar) {
                // Killa l'audio siamo usciti dall'auto!!
                clearInterval(chimeAudio);
                chimeAudio = undefined;
                if (cAudio !== undefined)
                    cAudio.pause();
                if (sfxAudio !== undefined)
                    sfxAudio.pause();
                currentStatus = false;
                return;
            }
            inCar = e.data.inCar;
        }
        if (currentStatus != e.data.transactionValue) {
            if (e.data.transactionValue === true) {
                playSound("sounds/buckle.ogg");
                if (chimeAudio !== undefined) {
                    clearInterval(chimeAudio);
                    chimeAudio = undefined;
                }
            } else {
                // We don't get in and unbuckle.. but the chime should still  happen.
                if (currentStatus != undefined)
                    playSound("sounds/unbuckle.ogg");
                if (chimeAudio !== undefined) {
                    clearInterval(chimeAudio);
                    chimeAudio = undefined;
                }
                chimeAudio = setInterval(function () { playChime(); }, 10000);
            }
            currentStatus = e.data.transactionValue;
        }
    }
});

function playSound(file) {
    if (sfxAudio != undefined) sfxAudio.pause();

    sfxAudio = new Audio(file);
    sfxAudio.volume = 1.0;
    sfxAudio.play();
}
function playChime() {
    cAudio = new Audio("sounds/chime.ogg");
    cAudio.volume = 1.0;
    cAudio.play();
}