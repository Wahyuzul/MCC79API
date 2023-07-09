console.log("Latihan Javascript");

function changeColor() {
    let row1 = document.getElementById("row1");
    let colors = ["yellow", "blue", "green", "aqua"];
    let currentColor = row1.style.backgroundColor;
    let currentIndex = colors.indexOf(currentColor);
    let nextIndex = (currentIndex + 1) % colors.length;
    row1.style.backgroundColor = colors[nextIndex];
}
function show() {
    let image = document.getElementById("image");
    image.src = "https://cdn.discordapp.com/attachments/717652523246420008/1126080794117554278/mike.png"
    document.getElementById("btnID")
        .style.display = "none";
}

document.getElementById("btn3").onclick = function () { myFunction() };
function myFunction() {
    document.getElementById("row2").innerHTML = "YOU CLICKED ME!!!";
    document.getElementById("row2").style.backgroundColor = "black";
    document.getElementById("row2").style.color = "red";
    document.getElementById("row2").style.fontSize = "40px";
}

let arrayMhsObj = [
    { nama: "budi", nim: "a112015", umur: 20, isActive: true, fakultas: { name: "komputer" } },
    { nama: "joko", nim: "a112035", umur: 22, isActive: false, fakultas: { name: "ekonomi" } },
    { nama: "herul", nim: "a112020", umur: 21, isActive: true, fakultas: { name: "komputer" } },
    { nama: "herul", nim: "a112032", umur: 25, isActive: true, fakultas: { name: "ekonomi" } },
    { nama: "herul", nim: "a112040", umur: 21, isActive: true, fakultas: { name: "komputer" } },
]

let fakultasKomputer = [];
for (i = 0; i < arrayMhsObj.length; i++) {
    if (arrayMhsObj[i].fakultas.name == "komputer") 
        fakultasKomputer.push(arrayMhsObj[i])
}
console.log(fakultasKomputer);

let selectByNim = [];
for (i = 0; i < arrayMhsObj.length; i++) {
    selectByNim.push(arrayMhsObj[i]);
    if (parseInt(arrayMhsObj[i].nim.slice(-2)) >= 30) {
        selectByNim[i].isActive = false;
    }
   
}
console.log(selectByNim);


