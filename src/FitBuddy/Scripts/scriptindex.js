let a = document.querySelectorAll("h1.slid.main")
let b = document.querySelectorAll("h1.slid.sub")
let flag = 0;
let i = 0;

const funk = function() {

    a[0].style.opacity = i;
    a[1].style.opacity = i;
    a[2].style.opacity = i;
    i += 0.01;

    if (i >= 1) {

        i = 0;
        clearInterval(int1);

        const funk2 = function () {

            b[0].style.opacity = i;
            b[1].style.opacity = i;
            b[2].style.opacity = i;
            i += 0.01;

            if (i >= 1) {

                i = 0;
                clearInterval(int2);
            }

        }

        var int2 = setInterval(funk2, 10);
       
    }

}

var int1 = setInterval(funk, 10);

document.querySelectorAll('#scroll1[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();

        document.querySelector(this.getAttribute('href')).scrollIntoView({
            behavior: 'smooth'
        });
    });
});

document.querySelectorAll('#scroll2[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();

        document.querySelector(this.getAttribute('href')).scrollIntoView({
            behavior: 'smooth'
        });
    });
});

document.querySelectorAll('#scroll3[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();

        document.querySelector(this.getAttribute('href')).scrollIntoView({
            behavior: 'smooth'
        });
    });
});

document.querySelectorAll('#scrolltop1[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();

        document.querySelector(this.getAttribute('href')).scrollIntoView({
            behavior: 'smooth'
        });
    });
});

document.querySelectorAll('#scrolltop2[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();

        document.querySelector(this.getAttribute('href')).scrollIntoView({
            behavior: 'smooth'
        });
    });
});

document.querySelectorAll('#scrolltop3[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {

        e.preventDefault();

        document.querySelector(this.getAttribute('href')).scrollIntoView({
            behavior: 'smooth'
        });
    });
});
