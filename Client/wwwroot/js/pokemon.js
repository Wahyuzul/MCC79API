$.ajax({
    url: "https://pokeapi.co/api/v2/pokemon/"
}).done((result) => {
    console.log(result);
    let temp = "";
    $.each(result.results, (key, val) => {
        temp += `<tr>
                    <td>${key + 1}</td>
                    <td>${val.name}</td>
                    <td><button onclick="detail('${val.url}')" data-bs-toggle="modal" data-bs-target="#modalpoke" class="btn btn-outline-dark" id="actionbtn">Detail</button></td>
                </tr>`;
    })
    $("#tbodypoke").html(temp);
})
function detail(stringURL) {
    $.ajax({
        url: stringURL
    }).done(res => {
        $("#image").attr("src", res.sprites.other["official-artwork"].front_default);
        $("#pokename").html(res.name);
        let weightConvert = (res.weight / 10);
        $(".weight").html(`${weightConvert} kg`);
        let heightConvet = (res.height / 10)
        $(".height").html(`${heightConvet} m`);

        let stats = "";
        res.stats.forEach((stat) => {
            let statWidth = (stat.base_stat / 150) * 100;
            let barColor = getBarColor(stat.stat.name);
            stats += `<li class="list-group-item">
                         ${stat.stat.name}:
                         <div class="progress">
                            <div class="progress-bar" role="progressbar" style="width: ${statWidth}%; background-color: ${barColor}" aria-valuenow="${stat.base_stat}" aria-valuemin="0" aria-valuemax="255">
                           ${stat.base_stat}
                            </div>
                        </div>
                    </li>`;
        });
        $(".stats").html(`<ul class="list-group">${stats}</ul>`);

        let types = "";
        res.types.forEach((type) => {
            let typeColor = getTypeColor(type.type.name);
            types += `<span class="badge rounded-pill" style="background-color:${typeColor}">${type.type.name}</span>`;
        });
        $(".types").html(types);

        let abilities = "";
        res.abilities.forEach((ability) => {
            abilities += `<li class="list-group-item">${ability.ability.name} </li>`;
        });
        $(".abilities").html(`<ul class="list-group">${abilities}</ul>`);

    })
}
function getTypeColor(typeName) {
    let typeColor = "";
    switch (typeName) {
        case "water":
            typeColor = "#6390F0";
            break;
        case "fire":
            typeColor = "#EE8130";
            break;
        case "grass":
            typeColor = "#7AC74C";
            break;
        case "poison":
            typeColor = "#A33EA1";
            break;
        case "flying":
            typeColor = "#A98FF3";
            break;
        case "bug":
            typeColor = "#A6B91A";
            break;
        default:
            typeColor = "#A8A77A";
            break;
    }
    return typeColor;
}
function getBarColor(statName) {
    let barColor = "";
    switch (statName) {
        case "hp":
            barColor = "#FF0000";
            break;
        case "attack":
            barColor = "#F08030";
            break;
        case "defense":
            barColor = "#F8D030";
            break;
        case "special-attack":
            barColor = "#6890F0";
            break;
        case "special-defense":
            barColor = "#78C850";
            break;
        case "speed":
            barColor = "#F85888";
        default:
            barColor = "#fc6898";
            break;
    }
    return barColor;
}

//reset aktif tab pane ke stats
let modal = document.getElementById('modalpoke');
modal.addEventListener('show.bs.modal', function (event) {
  
    let tab = document.getElementById('stats-tab');
    tab.classList.add('active');
    tab.setAttribute('aria-selected', 'true');

    let tabContent = document.getElementById('stats');
    tabContent.classList.add('show', 'active');
});

modal.addEventListener('hide.bs.modal', function (event) {
    let tab = document.getElementById('abilities-tab');
    tab.classList.remove('active');
    tab.setAttribute('aria-selected', 'false');

    let tabContent = document.getElementById('abilities');
    tabContent.classList.remove('show', 'active');
});
