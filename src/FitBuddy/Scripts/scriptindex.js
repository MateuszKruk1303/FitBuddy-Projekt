let a = document.querySelector(".welcome");
let b = document.querySelector(".desc");
b.style.top = "40%";
let flag = 0;
let i = 0;
let j = 40;

const funk = function() {

    a.style.opacity = i;
    i += 0.01;

    if (i >= 1) {

        i = 0;
        clearInterval(int1);

        const funk2 = function () {

            b.style.opacity = i;
            i += 0.01;
            if (i >= 1) {

                console.log("auuu");
                clearInterval(int2);
            }
        }

        const funk3 = function () {

            b.style.top = `${j}` + "%";
            j-=2;
            if (j <= 12) {

                clearInterval(int3);

            }

        }

        let int2 = setInterval(funk2, 10);
        let int3 = setInterval(funk3, 20);
        
        
    }

}

var int1 = setInterval(funk, 10);




